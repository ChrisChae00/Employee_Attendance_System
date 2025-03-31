using System;
using System.Collections.Generic;

namespace Employee_Klock_In_System.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public ApplicationUser Admin { get; set; }
        public int TotalEmployees { get; set; }
        public int ActiveToday { get; set; }
        public int PendingLeaves { get; set; }
        public double AverageWeeklyHours { get; set; }


        public List<Attendance> TodaysAttendance { get; set; } = new();

        // Use this dictionary directly in the view for the weekly graph
        public Dictionary<string, double> WeeklyGraphData { get; set; } = new();

        // Shortcut for today's attendance
        public List<Attendance> TodayActivity => TodaysAttendance;
    }
}
