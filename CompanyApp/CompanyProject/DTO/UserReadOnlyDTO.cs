using CompanyProject.Models;

namespace CompanyProject.DTO
{
    public class UserReadOnlyDTO
    {
        public int Id { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string? Phonenumber { get; set; }
    }
}
