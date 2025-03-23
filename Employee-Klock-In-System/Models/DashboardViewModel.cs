using System;
using System.Collections.Generic;

namespace Employee_Klock_In_System.Models
{
    public class DashboardViewModel
    {
        public Employee Employee { get; set; }

        public Attendance? TodayAttendance { get; set; }

        public List<Attendance> WeeklyAttendance { get; set; } = new();

        public List<Attendance> RecentHistory { get; set; } = new();
    }
}
