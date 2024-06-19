using CompanyProject.Models;
using EmpTasks = CompanyProject.Data.Task;


namespace CompanyProject.Data
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
