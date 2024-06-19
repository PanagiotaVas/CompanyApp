using AutoMapper;
using CompanyProject.Data;
using CompanyProject.DTO;
using CompanyProject.Services;
using CompanyProject.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyProject.Controllers
{
    [ApiController]
    [Route("/api/employees")]
    public class EmployeeController :  BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public EmployeeController(IApplicationService applicationService, IConfiguration configuration, IMapper mapper)
            : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("/api/employees/signupEmployee")]
        public async Task<ActionResult<UserSignUpDTO>> SignUpUser(UserSignUpDTO? userSignUpDTO)
        {
            if (!ModelState.IsValid)
            {
                // building a response if the model is not valid
                var errors = ModelState.Where(e => e.Value!.Errors.Any())
                    .Select(er => new
                    {
                        Field = er.Key,
                        Errors = er.Value!.Errors.Select(err => err.ErrorMessage.ToArray())
                    });
                throw new InvalidRegistrationException("ErrorsInRegistration: " + errors);
            }

            if (_applicationService is null)
            {
                throw new ServerGenericException("ApplicationServiceNull");
            }

            User? user = await _applicationService.UserService.GetUserByUsernameAsync(userSignUpDTO!.Username!);

            if (user is not null)
            {
                throw new UserAlreadyExistsException("UserAlreadyExists, " + user.Username);
            }

            await _applicationService.UserService.SignUpUserAsync(userSignUpDTO);

            User? userToReturn = await _applicationService.UserService.GetUserByUsernameAsync(userSignUpDTO!.Username!);
            Console.WriteLine("We retrieved " + userSignUpDTO!.Username!);
            if (userToReturn is null)
            {
                throw new InvalidRegistrationException("InvalidRegistration");
            }

            return CreatedAtAction(nameof(GetEmployeeById), new { id = userToReturn.Id }, userToReturn);
        }

        [HttpGet]
        [Route("/api/employees/getEmployeeById/{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var employeeToReturn = await _applicationService.EmployeeService.GetEmployeeByIdAsync(id);
            if (employeeToReturn is null)
            {
                throw new UserNotFoundException("UserNotFound");
            }

            return Ok(employeeToReturn);
        }

        [HttpGet]
        [Route("/api/employees/getEmployeeByEmail/{email}")]
        public async Task<ActionResult<Employee>> GetEmployeeByEmail(string email)
        {
            var employeeToReturn = await _applicationService.EmployeeService.GetEmployeeByEmailAsync(email);
            if (employeeToReturn is null)
            {
                throw new UserNotFoundException("UserNotFound");
            }


            return Ok(employeeToReturn);
        }

        [HttpPut]
        [Route("/api/employees/updateEmployee")]
        public async Task<ActionResult<EmployeeReadOnlyDTO>> UpdateEmployee(EmployeeUpdateDTO dto)
        {
            Employee? employeeToUpdate = await _applicationService.EmployeeService.GetEmployeeByIdAsync(dto.Id);

            if (employeeToUpdate is null)
            {
                throw new EmployeeNotFoundException("Employee with id " + dto.Id + " could not be found to be updated.");
            }
            
            if(employeeToUpdate.UserId != dto.UserId)
            {
                throw new UserChangeException("An employee cannot change user id.");
            }

            User? userNeeded = await _applicationService.UserService.GetUserByIdAsync(dto.UserId);

            if (userNeeded is null) throw new UserNotFoundException("User with id " + dto.UserId + " cannot be found.");

            Employee? employeeUpdated = await _applicationService.EmployeeService.UpdateEmployeeAsync(dto, dto.Id);

            EmployeeReadOnlyDTO readOnlyEmployee = _mapper.Map<EmployeeReadOnlyDTO>(employeeUpdated);

            return Ok(readOnlyEmployee);
        }

        [HttpGet]
        [Route("/api/employees/getAllEmployees")]
        public async Task<ActionResult<List<Employee>>> GetAllEmployees()
        {
            ICollection<Employee> allEmployees;

            allEmployees = await _applicationService.EmployeeService.GetAllEmployeesAsync();

            if (allEmployees is null)
                return NoContent();

            return Ok(allEmployees);
        }

        [HttpGet]
        [Route("/api/employees/getAllEmployeesPresenter")]
        public async Task<ActionResult<List<Employee>>> GetAllEmployeesPresenter()
        {
            ICollection<Employee> allEmployees;
            IList<EmployeePresenterDTO> employeesToPresent = new List<EmployeePresenterDTO>();

            allEmployees = await _applicationService.EmployeeService.GetAllEmployeesAsync();

            foreach (var employee in allEmployees)
            {
                EmployeePresenterDTO tmpdto = new EmployeePresenterDTO();
                tmpdto.Id = employee.Id;
                tmpdto.Firstname = employee.Firstname!;
                tmpdto.Lastname = employee.Lastname!;
                tmpdto.PhoneNumber = employee.PhoneNumber;
                tmpdto.Email = employee.Email;

                int userId = (int)employee.UserId!;
                User? user = await _applicationService.UserService.GetUserByIdAsync(userId);
                tmpdto.UserId = userId;
                tmpdto.Username = user!.Username;

                employeesToPresent.Add(tmpdto);
            }

            if (allEmployees is null)
                return NoContent();

            return Ok(employeesToPresent);
        }

        [HttpDelete]
        [Route("/api/employees/deleteEmployee/{id}/{username}")]
        public async Task<IActionResult> DeleteEmployee(int id, string username)
        {
            ICollection<EmployeeTask> allAssignments;

            var employeeToDelete = await _applicationService.EmployeeService.GetEmployeeByIdAsync(id);
            if (employeeToDelete is null)
            {
                throw new EmployeeNotFoundException("Employee was not found to be deleted.");
            }

            // need to delete the assignments first
            allAssignments = await _applicationService.EmployeesXTasksService.GetAllAssignmentsOfUserAsync(username);

            if(allAssignments is not null)
            {
                foreach (var assignment in allAssignments)
                {
                    await _applicationService.EmployeesXTasksService.DeleteAssignmentAsync(username!, assignment.TaskId);

                }
            }

            int usId = (int)employeeToDelete.UserId!;

            var deletedEmployee = await _applicationService.EmployeeService.DeleteEmployee(id);

            return Ok();
        }
    }
}
