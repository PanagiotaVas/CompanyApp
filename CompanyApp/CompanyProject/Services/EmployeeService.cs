using AutoMapper;
using Azure.Core;
using CompanyProject.Data;
using CompanyProject.DTO;
using CompanyProject.Repositories;
using CompanyProject.Services.Exceptions;
using EmplTask = CompanyProject.Data.Task;
using Task = System.Threading.Tasks.Task;

namespace CompanyProject.Services
{
    public class EmployeeService : IEmployeeService
    {

        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;
        private readonly ILogger<UserService>? _logger;

        public EmployeeService(IUnitOfWork? unitOfWork, ILogger<UserService>? logger, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            bool employeeDeleted = false;
            try
            {
                Employee? employee = await _unitOfWork!.EmployeeRepository.GetEmployeeById(id);
                if (employee is null) throw new EmployeeNotFoundException("Employee with id " + id + " was not found");

                var deleted = await _unitOfWork.EmployeeRepository.DeleteEmployee(id);

                await _unitOfWork.SaveAsync();

                int userId = (int)employee.UserId;

                var daletedu = await _unitOfWork.UserRepository.DeleteAsync(userId);

                await _unitOfWork.SaveAsync();

                employeeDeleted = (deleted && daletedu);

                if(employeeDeleted)
                {
                    _logger!.LogInformation("{Message}", "Employee with id: " + id + " was deleted successfully");

                }

                return employeeDeleted;
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
            }
            return employeeDeleted;
        }
    

        public async Task<ICollection<Employee>> GetAllEmployeesAsync()
        {

            ICollection<Employee> allEmployees = null;
            try
            {
                 allEmployees = await _unitOfWork!.EmployeeRepository.GetAllEmployeesAsync();
                _logger!.LogInformation("{Message}", "All Employees were returned successfully");
                return allEmployees;

            }
                catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
            }
                return allEmployees;
        }

        public Task<IEnumerable<User>> GetAllUserEmployeesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Employee?> GetEmployeeByEmailAsync(string email)
        {
            Employee? employee = null;
            try
            {
                employee = await _unitOfWork!.EmployeeRepository.GetEmployeeByEmailAsync(email); ;
                _logger!.LogInformation("{Message}", "Employee with email: " + email + " was returned successfully");
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
            }
            return employee;
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            Employee? employee = null;
            try
            {
                employee = await _unitOfWork!.EmployeeRepository.GetEmployeeById(id);
                _logger!.LogInformation("{Message}", "Employee with id: " + id + " was returned successfully");
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
            }
            return employee;
        }

        public async Task InsertEmployeeAsync(EmployeeDTO employeeDTO)
        {
            try
            {
                if (await _unitOfWork!.EmployeeRepository.GetEmployeeByEmailAsync(employeeDTO.Email!) is not null)
                    throw new ApplicationException("Employee already exists");
                await _unitOfWork.SaveAsync();
                _logger!.LogInformation("{Message}", "Employee with email: " + employeeDTO.Email + " was inserted successfully");
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
            }
        }

        public async Task<Employee?> UpdateEmployeeAsync(EmployeeUpdateDTO updateDTO, int id)
        {
            Employee? employee = null;
            try
            {
                employee = await _unitOfWork!.EmployeeRepository.UpdateEmployeeAsync(id, updateDTO);
                await _unitOfWork.SaveAsync();
                _logger!.LogInformation("{Message}", "Employee with id: " + id + " was updated successfully");
                return employee;
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
            }
            return employee;
        }

    }

}

