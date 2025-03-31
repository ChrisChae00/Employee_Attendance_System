namespace Employee_Klock_In_System.Models.ViewModels
{
    public class ViewAllEmployeesViewModel
    {
        public List<ApplicationUser> Employees { get; set; }
        public ApplicationUser EmployeeBeingEdited { get; set; }
        public ApplicationUser Admin { get; set; }
    }


}
