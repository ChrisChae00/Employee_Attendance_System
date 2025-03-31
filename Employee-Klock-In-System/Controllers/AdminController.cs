using Employee_Klock_In_System.Data;
using Employee_Klock_In_System.Models;
using Employee_Klock_In_System.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Employee_Klock_In_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }


        public AdminController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> AdminDashboard()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var today = DateTime.Today;
            var weekStart = today.AddDays(-(int)today.DayOfWeek + 1); // Monday
            var weekEnd = weekStart.AddDays(7);

            var attendancesThisWeek = await _context.Attendances
                .Where(a => a.CheckInTime >= weekStart && a.CheckInTime < weekEnd)
                .ToListAsync();

            var todaysAttendance = await _context.Attendances
                .Where(a => a.CheckInTime.Date == today)
                .Include(a => a.Employee)
                .ToListAsync();

            var totalEmployees = await _userManager.Users.CountAsync();
            var activeToday = todaysAttendance.Count;
            var pendingLeaves = await _context.LeaveRequests.Where(l => l.Status == "Pending").CountAsync();

            var groupedByDay = attendancesThisWeek
                .GroupBy(a => a.CheckInTime.DayOfWeek)
                .ToDictionary(
                    g => CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedDayName(g.Key),
                    g => g.Sum(a => (a.CheckOutTime.HasValue ? (a.CheckOutTime.Value - a.CheckInTime).TotalHours : 0))
                );

            var totalWeeklyHours = groupedByDay.Sum(x => x.Value);
            var avgWeeklyHours = totalEmployees > 0 ? totalWeeklyHours / totalEmployees : 0;

            var viewModel = new AdminDashboardViewModel
            {
                Admin = currentUser,
                TotalEmployees = totalEmployees,
                ActiveToday = activeToday,
                PendingLeaves = pendingLeaves,
                AverageWeeklyHours = Math.Round(avgWeeklyHours, 1),
                TodaysAttendance = todaysAttendance,
                WeeklyGraphData = groupedByDay
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ViewAllEmployees(string? editId, string? deleteId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (!string.IsNullOrEmpty(deleteId))
            {
                var userToDelete = await _userManager.FindByIdAsync(deleteId);
                if (userToDelete != null)
                {
                    await _userManager.DeleteAsync(userToDelete);
                    return RedirectToAction("ViewAllEmployees");
                }
            }

            var employees = await _userManager.Users.ToListAsync();
            foreach (var user in employees)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.Role = roles.FirstOrDefault();
            }

            var viewModel = new ViewAllEmployeesViewModel
            {
                Employees = employees,
                EmployeeBeingEdited = !string.IsNullOrEmpty(editId)
                    ? employees.FirstOrDefault(e => e.Id == editId)
                    : null,
                Admin = currentUser
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(ApplicationUser updated)
        {
            var employee = await _userManager.FindByIdAsync(updated.Id);
            if (employee == null)
                return NotFound();

            employee.FirstName = updated.FirstName;
            employee.LastName = updated.LastName;
            employee.Email = updated.Email;

            // Handle role update
            var currentRoles = await _userManager.GetRolesAsync(employee);
            if (!currentRoles.Contains(updated.Role))
            {
                await _userManager.RemoveFromRolesAsync(employee, currentRoles);
                await _userManager.AddToRoleAsync(employee, updated.Role);
            }

            await _userManager.UpdateAsync(employee);
            return RedirectToAction("ViewAllEmployees");
        }



        public IActionResult AttendanceRecords()
        {
            return View();  // Views/Admin/AttendanceRecords.cshtml
        }

        [HttpGet]
        public async Task<IActionResult> ManageLeaveRequests()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var leaveRequests = await _context.LeaveRequests
                .Include(l => l.Employee) // Ensure employee info is included
                .OrderByDescending(l => l.StartDate)
                .ToListAsync();

            var viewModel = new LeaveRequestViewModel
            {
                Admin = currentUser,
                LeaveRequests = leaveRequests
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessLeaveRequest(int requestId, string action)
        {
            var request = await _context.LeaveRequests.FindAsync(requestId);
            if (request == null)
                return NotFound();

            request.Status = action == "Approve" ? "Approved" : "Rejected";
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageLeaveRequests");
        }





        public IActionResult Reports()
        {
            return View();  // Views/Admin/Reports.cshtml
        }
    }
}
