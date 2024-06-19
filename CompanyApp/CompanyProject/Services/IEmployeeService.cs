using CompanyProject.Data;
using CompanyProject.DTO;
using EmplTask = CompanyProject.Data.Task;
using Task = System.Threading.Tasks.Task;

namespace CompanyProject.Services
{
    public interface IEmployeeService
    {
        Task InsertEmployeeAsync(EmployeeDTO employeeDTO);
        Task<Employee> UpdateEmployeeAsync(EmployeeUpdateDTO updateDTO, int id);
        Task<bool> DeleteEmployee(int id);
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee?> GetEmployeeByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUserEmployeesAsync();
        Task<ICollection<Employee>> GetAllEmployeesAsync();
    }
}
