using Employee_Klock_In_System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        // Show list of attendance
        public async Task<IActionResult> EmployeeDashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == user.Email);

            return View(employee);
        }


        // GET: Employee/CheckIn
        public IActionResult CheckIn()
        {
            return View();  // Views/Employee/CheckIn.cshtml
        }

        // GET: Employee/CheckOut
        public IActionResult CheckOut()
        {
            return View();  // Views/Employee/CheckOut.cshtml
        }

        // GET: Employee/AttendanceHistory
        public IActionResult AttendanceHistory()
        {
            return View();  // Views/Employee/AttendanceHistory.cshtml
        }
    }
}
