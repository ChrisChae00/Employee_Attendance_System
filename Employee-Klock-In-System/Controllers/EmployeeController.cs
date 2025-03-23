using Employee_Klock_In_System.Data;
using Employee_Klock_In_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Employee_Klock_In_System.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public EmployeeController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        // Show Employee's Dashboard
        public async Task<IActionResult> EmployeeDashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == user.Email);

            if (employee == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var today = DateTime.Today;
            var currentDay = (int)today.DayOfWeek;
            var weekStart = today.AddDays(-((currentDay + 6) % 7));
            var weekEnd = weekStart.AddDays(7);

            var weeklyAttendances = await _context.Attendances
                .Where(a => a.EmployeeId == employee.Id &&
                            a.CheckInTime.Date >= weekStart &&
                            a.CheckInTime.Date < weekEnd)
                .OrderBy(a => a.CheckInTime)
                .ToListAsync();

            var todayAttendance = weeklyAttendances
                .FirstOrDefault(a => a.CheckInTime.Date == today);

            var history = await _context.Attendances
                .Where(a => a.EmployeeId == employee.Id)
                .OrderByDescending(a => a.CheckInTime)
                .Take(7)
                .ToListAsync();  // Get the last 7 days of attendance history

            var viewModel = new DashboardViewModel
            {
                Employee = employee,
                TodayAttendance = todayAttendance,
                WeeklyAttendance = weeklyAttendances,
                RecentHistory = history
            };

            return View(viewModel);
        }

        // GET: Employee/CheckIn
        [HttpPost]
        public async Task<IActionResult> KlockIn()
        {
            var user = await _userManager.GetUserAsync(User);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == user.Email);

            if (employee == null)
                return RedirectToAction("EmployeeDashboard");

            var alreadyCheckedIn = await _context.Attendances.AnyAsync(a =>
                a.EmployeeId == employee.Id && a.CheckInTime.Date == DateTime.Today);

            if (!alreadyCheckedIn)
            {
                var attendance = new Attendance
                {
                    EmployeeId = employee.Id,
                    CheckInTime = DateTime.Now
                };
                _context.Attendances.Add(attendance);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("EmployeeDashboard");
        }

        // GET: Employee/CheckOut
        [HttpPost]
        public async Task<IActionResult> KlockOut()
        {
            var user = await _userManager.GetUserAsync(User);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == user.Email);

            if (employee == null)
                return RedirectToAction("EmployeeDashboard");

            var todayAttendance = await _context.Attendances
                .Where(a => a.EmployeeId == employee.Id && a.CheckInTime.Date == DateTime.Today)
                .OrderByDescending(a => a.CheckInTime)
                .FirstOrDefaultAsync();

            if (todayAttendance != null && todayAttendance.CheckOutTime == null)
            {
                todayAttendance.CheckOutTime = DateTime.Now;
                _context.Attendances.Update(todayAttendance);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("EmployeeDashboard");
        }

        // GET: Employee/AttendanceHistory
        public IActionResult AttendanceHistory()
        {
            return View();  // Views/Employee/AttendanceHistory.cshtml
        }
    }
}
