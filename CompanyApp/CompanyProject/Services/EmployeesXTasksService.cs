using AutoMapper;
using CompanyProject.Data;
using CompanyProject.Repositories;
using CompanyProject.Services.Exceptions;
using System.Threading.Tasks;
using System.Transactions;
using EmplTask = CompanyProject.Data.Task;
using Task = System.Threading.Tasks.Task;
namespace CompanyProject.Services
{
    public class EmployeesXTasksService : IEmployeesXTasksService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService>? _logger;

        public EmployeesXTasksService(IUnitOfWork unitOfWork, ILogger<UserService>? logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        


        public async Task<bool> AssignTaskAsync(string username, int taskId)
        {
            User? userEmployee = await _unitOfWork.EmployeeRepository.GetEmployeeByUsernameAsync(username);
            EmplTask? taskToFind = await _unitOfWork.TaskRepository.GetTaskAsync(taskId);

            if(userEmployee is null)
            {
                _logger!.LogError("{Message}{Exception}", "User with username " + username + " could not be found.", "");
                throw new UserNotFoundException("User with username " + username + " could not be found.");
            }


            if (taskToFind is null)
            {
                _logger!.LogError("{Message}{Exception}", "Task with id " + taskId + " could not be found.", "");
                throw new TaskNotFoundException("Task with id " + taskId + " could not be found.");
            }

            // now we have to find emplId
            Employee? employee = await _unitOfWork.EmployeeRepository.GetEmployeeByUserIdAsync(userEmployee.Id);

            int emplId = employee!.Id;
            int userId = userEmployee.Id;

            bool inserted = await _unitOfWork.EmployeesXTasksRepository.AssignTaskToEmployee(emplId, taskId, userId);

            await _unitOfWork.SaveAsync();

            _logger!.LogInformation("{Message}", "Assignment: to " + username + " with task id " + taskId + " was inserted successfully");

            return inserted;
        }


        public async Task<bool> DeleteAssignmentAsync(string username, int taskId)
        {

            User? userEmployee = await _unitOfWork.EmployeeRepository.GetEmployeeByUsernameAsync(username);
            Employee? emplToFind = await _unitOfWork.EmployeeRepository.GetEmployeeByUserIdAsync(userEmployee.Id);
            EmplTask? taskToFind = await _unitOfWork.TaskRepository.GetTaskAsync(taskId);

            if (userEmployee is null) throw new UserNotFoundException("User with username " + username + " could not be found.");

            if (emplToFind is null) throw new EmployeeNotFoundException("Employee with username " + username + " could not be found.");

            if (taskToFind is null) throw new TaskNotFoundException("Task with id " + taskId + " could not be found.");

            bool removed = await _unitOfWork.EmployeesXTasksRepository.RemoveAssignment(emplToFind!.Id, taskToFind.Id, userEmployee.Id);

            _unitOfWork.SaveAsync();

            _logger!.LogInformation("{Message}", "Assignment " + username + " - " + taskId  + " was deleted successfully");

            return removed;
        }

        public async Task<bool> DeleteEmployeeAndAssignments(int id, string username)
        {
            try
            {
                var employeeToDelete = await _unitOfWork.EmployeeRepository.GetEmployeeById(id);
                if (employeeToDelete is null)
                {
                    throw new EmployeeNotFoundException("Employee was not found to be deleted.");
                }

                var userEmployee = await _unitOfWork.EmployeeRepository.GetEmployeeByUsernameAsync(username);
                if (userEmployee is null)
                {
                    throw new UserNotFoundException("User with username " + username + " could not be found.");
                }

                var allAssignments = await _unitOfWork.EmployeesXTasksRepository.GetAllAssignmentsOfUserAsync(username);

                foreach (var assignment in allAssignments)
                {
                    
                    int taskId = (int)assignment.TaskId;
                    await _unitOfWork.EmployeesXTasksRepository.RemoveAssignment(userEmployee.Id, taskId, userEmployee.Id);
                }
                _unitOfWork.SaveAsync();     

                int usId = (int)employeeToDelete.UserId;

                await _unitOfWork.EmployeeRepository.DeleteAsync(id);

                _unitOfWork.SaveAsync();

                await _unitOfWork.UserRepository.DeleteUserAsync(usId);

                await _unitOfWork.SaveAsync();

                _logger!.LogInformation("{Message}", "Employee with " + username + " and their tasks was deleted successfully");

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAllAssignmentsOfTask(int taskId)
        {
            try
            {
                var taskToDelete = await _unitOfWork.TaskRepository.GetTaskAsync(taskId);

                if (taskToDelete is null)
                {
                    throw new TaskNotFoundException("Task with id " + taskId + " was not found to be deleted.");
                }

                var allAssignments = await _unitOfWork.EmployeesXTasksRepository.GetAllAssignmentsOfTaskAsync(taskId);

                foreach (var assignment in allAssignments)
                {
                    await _unitOfWork.EmployeesXTasksRepository.RemoveAssignmentByTask(taskId);
                }

                await _unitOfWork.SaveAsync();

                _logger!.LogInformation("{Message}", "All assignments related to the task with id " + taskId + " were deleted successfully");

                return true;
            } catch(Exception ex)
            {
                return false;
            }
        }


        public async Task<ICollection<EmployeeTask>> GetAllAssignmentsAsync()
        {
            ICollection<EmployeeTask> allAssignments = await _unitOfWork.EmployeesXTasksRepository.GetAllAssignmentsAsync();

            _logger!.LogInformation("{Message}", "All assignments were returned successfully");

            return allAssignments;
        }

        public async Task<ICollection<EmployeeTask>> GetAllAssignmentsOfUserAsync(int userId)
        {
            User? userToFind = await _unitOfWork.UserRepository.GetAsync(userId);
            if(userToFind is null) throw new UserNotFoundException("User with id " +  userId + " could not be found.");

            ICollection<EmployeeTask> allAssignments = await _unitOfWork.EmployeesXTasksRepository.GetAllAssignmentsOfUserAsync(userId);

            _logger!.LogInformation("{Message}", "All assignments of user " + userToFind.Username + " were returned successfully");

            return allAssignments;
        }

        public async Task<ICollection<EmployeeTask>> GetAllAssignmentsOfTaskAsync(int taskId)
        {
            EmplTask? taskToFind = await _unitOfWork.TaskRepository.GetTaskAsync(taskId);
            if (taskToFind is null) throw new UserNotFoundException("Task with id " + taskId + " could not be found.");

            ICollection<EmployeeTask> allAssignments = await _unitOfWork.EmployeesXTasksRepository.GetAllAssignmentsOfTaskAsync(taskId);

            _logger!.LogInformation("{Message}", "All assignments related to the task " + taskId + " were returned successfully");

            return allAssignments;
        }

        public async Task<ICollection<EmployeeTask>> GetAllAssignmentsOfUserAsync(string username)
        {
            User? userToFind = await _unitOfWork.UserRepository.GetByUsernameAsync(username);
            if (userToFind is null) throw new UserNotFoundException("User with id " + username + " could not be found.");

            ICollection<EmployeeTask> allAssignments = await _unitOfWork.EmployeesXTasksRepository.GetAllAssignmentsOfUserAsync(username);

            _logger!.LogInformation("{Message}", "All assignments of user " + userToFind.Username + " were returned successfully");

            return allAssignments;
        }

        public async Task<EmplTask> GetUserTaskAsync(string username, int taskId)
        {
            // find user by username
            User? userToFind = await _unitOfWork.UserRepository.GetByUsernameAsync(username);

            if (userToFind is null) throw new UserNotFoundException("User with username " + username + " could not be found.");

            // find the corresponding employee
            Employee? employeeFromUser = await _unitOfWork.EmployeeRepository.GetEmployeeByUserIdAsync(userToFind.Id);

            // find the ask based on its id
            EmplTask? taskToReturn = await _unitOfWork.TaskRepository.GetTaskAsync(taskId);
            if (taskToReturn is null) throw new TaskNotFoundException("Task with id " + taskId + " does not exist.");

            // find all tasks of that user and see if this task is assigned to them
            ICollection<EmployeeTask> allTasksOfUser = await _unitOfWork.EmployeesXTasksRepository.GetAllAssignmentsOfUserAsync(userToFind.Id);
            bool found = false;
            foreach(EmployeeTask task in allTasksOfUser)
            {
                if(task.TaskId == taskId)
                {
                    found = true;
                    break;
                }
            }

            if (!found) throw new AssignmentNotFoundException("User with username " + username + " is not assigned with task with id " + taskId + ".");
            _logger!.LogInformation("{Message}", "Assignment " + username + " - " + taskId + " was returned successfully");

            // if it is assigned to them we send it back
            return taskToReturn;

        }
    }
}
