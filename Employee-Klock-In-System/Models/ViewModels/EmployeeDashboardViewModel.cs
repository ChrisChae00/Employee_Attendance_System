using System;
using System.Collections.Generic;
using Employee_Klock_In_System.Models;

namespace Employee_Klock_In_System.Models.ViewModels
{
    public class EmployeeDashboardViewModel
    {
        public ApplicationUser Employee { get; set; }

        public Attendance? TodayAttendance { get; set; }

        public List<Attendance> WeeklyAttendance { get; set; } = new();

        public List<Attendance> RecentHistory { get; set; } = new();
    }
}
