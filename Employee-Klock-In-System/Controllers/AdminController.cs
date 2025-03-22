using Microsoft.AspNetCore.Mvc;

namespace Employee_Klock_In_System.Controllers
{
    public class AdminController : Controller
    {
        // Dashboard home
        public IActionResult AdminDashboard()
        {
            return View();  // Views/Admin/AdminDashboard.cshtml
        }

        // View all attendance records
        public IActionResult AttendanceRecords()
        {
            return View();  // Views/Admin/AttendanceRecords.cshtml
        }

        // Approve or reject leave requests
        public IActionResult LeaveRequests()
        {
            return View();  // Views/Admin/LeaveRequests.cshtml
        }

        // Generate reports
        public IActionResult Reports()
        {
            return View();  // Views/Admin/Reports.cshtml
        }
    }
}
