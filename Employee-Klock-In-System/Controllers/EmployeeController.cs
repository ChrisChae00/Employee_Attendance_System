using Microsoft.AspNetCore.Mvc;

namespace Employee_Klock_In_System.Controllers
{
    public class EmployeeController : Controller
    {
        // Show list of attendance
        public IActionResult Index()
        {
            return View();  // Views/Employee/Index.cshtml
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
