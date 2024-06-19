using System.ComponentModel.DataAnnotations;

namespace CompanyProject.DTO
{
    public class EmployeePresenterDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters")]
        public string Firstname { get; set; } = null!;

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters")]
        public string Lastname { get; set; } = null!;

        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [StringLength(10, ErrorMessage = "Phonenumber must be 10 characters.")]
        public string? PhoneNumber { get; set; }

        public string? Username { get; set; }
    }
}
