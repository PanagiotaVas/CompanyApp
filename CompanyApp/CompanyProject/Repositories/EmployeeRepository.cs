using Azure.Core;
using CompanyProject.Data;
using CompanyProject.DTO;
using CompanyProject.Models;
using Microsoft.EntityFrameworkCore;
using EmplTask = CompanyProject.Data.Task;
using Task = System.Threading.Tasks.Task;

namespace CompanyProject.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(CompanyProjectDbContext context)
            : base(context)
        {

        }

        public async Task<Employee?> GetEmployeeByPhonenumber(string phonenumber)
        {
            var employeeToReturn = await _context.Employees.Where(e => e.PhoneNumber == phonenumber).FirstOrDefaultAsync();
            return employeeToReturn;
        }


        public async Task<List<EmplTask>> GetEmployeeTasks(int id)
        {
            List<EmplTask> tasks = new();
            tasks = await _context.Employees.Where(em => em.Id == id)
                .SelectMany(t => t.Tasks).ToListAsync();
            return tasks;
            
        }

        public Task<User?> GetEmployeeByUsernameAsync(string username)
        {
            var userToReturn = _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
            return userToReturn;
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee is null) return false;

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee?> GetEmployeeByEmailAsync(string email)
        {
            return await _context.Employees.Where(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Employee?> UpdateEmployeeAsync(int id, EmployeeUpdateDTO updateDTO)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return null;

            employee.Email = updateDTO.Email;
            employee.Firstname = updateDTO.Firstname;
            employee.Lastname = updateDTO.Lastname;
            employee.PhoneNumber = updateDTO.Phonenumber;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<ICollection<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .ToArrayAsync();
        }

        public async Task<bool> InsertEmployeeAsync(EmployeeDTO employeeDTO)
        {
            try
            {
                var existingEmployee = await _context.Employees.Where(x => x.Email == employeeDTO.Email).FirstOrDefaultAsync();
                if (existingEmployee is not null) return false;

                var employee = new Employee()
                {
                    UserId = employeeDTO.UserId,
                    Firstname = employeeDTO.Firstname,
                    Lastname = employeeDTO.Lastname,
                    Email = employeeDTO.Email,
                    PhoneNumber = employeeDTO.Phonenumber,
                };

                await _context.Employees.AddAsync(employee);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<Employee?> GetEmployeeByUserIdAsync(int userId)
        {
            var employee = await _context.Employees.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            return employee;
        }
    }
}
