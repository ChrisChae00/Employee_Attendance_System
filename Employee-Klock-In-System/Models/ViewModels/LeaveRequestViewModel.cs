namespace Employee_Klock_In_System.Models.ViewModels
{
    public class LeaveRequestViewModel
    {
        public ApplicationUser Employee { get; set; } // Employee who is requesting the leave
        public List<LeaveRequest> LeaveRequests { get; set; } // Leave requests of the employee
        public ApplicationUser Admin { get; set; } // Admin who is viewing the leave requests
    }

}
