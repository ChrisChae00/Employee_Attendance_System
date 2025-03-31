using Microsoft.AspNetCore.Identity;

namespace Employee_Klock_In_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }
            
        public string FullName => $"{FirstName} {LastName}";

    }
}
