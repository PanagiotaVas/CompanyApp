using CompanyProject.Data;
using CompanyProject.DTO;
using EmpTasks = CompanyProject.Data.Task;
using Task = System.Threading.Tasks.Task;
namespace CompanyProject.Repositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<bool> InsertEmployeeAsync(EmployeeDTO employeeDTO);
        Task<Employee?> GetEmployeeById(int id);
        Task<Employee> GetEmployeeByEmailAsync(string email);
        Task<Employee> UpdateEmployeeAsync(int id, EmployeeUpdateDTO updateDTO);
        Task<bool> DeleteEmployee(int id);
        Task<ICollection<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByPhonenumber(string phonenumber);
        Task<User?> GetEmployeeByUsernameAsync(string username);
        Task<Employee?> GetEmployeeByUserIdAsync(int userId);
    }
}
