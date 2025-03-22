namespace Employee_Klock_In_System.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }  // Admin or Employee

        public string Email { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
