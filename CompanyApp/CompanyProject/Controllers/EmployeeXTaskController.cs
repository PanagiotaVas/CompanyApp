using AutoMapper;
using CompanyProject.Data;
using CompanyProject.Services;
using CompanyProject.Services.Exceptions;
using EmplTask = CompanyProject.Data.Task;
using Microsoft.AspNetCore.Mvc;
using CompanyProject.DTO;

namespace CompanyProject.Controllers
{
    [ApiController]
    [Route("/api/employeesxtasks")]
    public class EmployeeXTaskController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public EmployeeXTaskController(IApplicationService applicationService, IConfiguration configuration, IMapper mapper)
            : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("/api/employeesxtasks/insertAssignment")]
        public async Task<ActionResult<bool>> InsertAssignment(EmployeeXTaskInsertDTO insertDto)
        {
            if (!ModelState.IsValid)
            {
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

            string username = insertDto.Username!;
            int tId = insertDto.TaskId!;

            bool inserted = await _applicationService.EmployeesXTasksService.AssignTaskAsync(username, tId);

            return Ok(inserted);
        }

        [HttpGet]
        [Route("/api/employeesxtasks/getAllAssignmentsFormatted")]
        public async Task<ActionResult<List<EmployeeTask>>> GetAllAssignments()
        {
            ICollection<EmployeeTask> allAssignments;
            ICollection<EmployeeXTaskFormattedDTO> allAssignmentsToReturnFormatted = new List<EmployeeXTaskFormattedDTO>();

            allAssignments = await _applicationService.EmployeesXTasksService.GetAllAssignmentsAsync();

            if (allAssignments is null)
                return NoContent();

            foreach (var assignment in allAssignments)
            {
                EmployeeXTaskFormattedDTO tmp = new EmployeeXTaskFormattedDTO();
                var user = await _applicationService.UserService.GetUserByIdAsync(assignment.UserId);
                var task = await _applicationService.TaskService.GetTaskAsync(assignment.TaskId);
                tmp.Username = user!.Username;
                tmp.Title = task!.Title;
                allAssignmentsToReturnFormatted.Add(tmp);
            }

            return Ok(allAssignmentsToReturnFormatted);
        }

        [HttpGet]
        [Route("/api/employeesxtasks/getAllAssignmentsOfUser/{username}")]
        public async Task<ActionResult<List<EmployeeTask>>> GetAllAssignmentsOfUser(string username)
        {
            ICollection<EmployeeTask> allAssignments;

            User? user = await _applicationService.UserService.GetUserByUsernameAsync(username);

            allAssignments = await _applicationService.EmployeesXTasksService.GetAllAssignmentsOfUserAsync(user!.Id);

            if (allAssignments is null)
                return NoContent();

            return Ok(allAssignments);

        }

        [HttpDelete]
        [Route("/api/employeesxtasks/removeAnAssignment/{username}/{taskId}")]
        public async Task<IActionResult> DeleteAssignment(string username, int taskId)
        {
            User? user = await _applicationService.UserService.GetUserByUsernameAsync(username);
            EmplTask? task = await _applicationService.TaskService.GetTaskAsync(taskId);

            if (user is null) throw new UserNotFoundException("User with username " + username + " was not found.");

            if (task is null) throw new TaskNotFoundException("Task with id " + taskId + " was not found.");

            bool removed = await _applicationService.EmployeesXTasksService.DeleteAssignmentAsync(username, taskId);

            return Ok(removed);
        }

        [HttpDelete]
        [Route("/api/employeesxtasks/removeAssignment/{username}/{title}")]
        public async Task<IActionResult> RemoveAssignment(string username, string title)
        {
            User? user = await _applicationService.UserService.GetUserByUsernameAsync(username);
            var taskToFind = await _applicationService.TaskService.GetTaskByTitle(title);
            EmplTask? task = await _applicationService.TaskService.GetTaskAsync(taskToFind!.Id);

            if (user is null) throw new UserNotFoundException("User with username " + username + " was not found.");

            if (task is null) throw new TaskNotFoundException("Task with id " + taskToFind!.Id + " was not found.");

            bool removed = await _applicationService.EmployeesXTasksService.DeleteAssignmentAsync(username, taskToFind!.Id);

            return Ok(removed);
        }

        [HttpDelete]
        [Route("/api/employeesxtasks/removeAllAssignmentsOfAUser/{username}")]
        public async Task<IActionResult> DeleteAllAssignmentsOfAUser(string username)
        {
            ICollection<EmployeeTask> allAssignments;

            User? user = await _applicationService.UserService.GetUserByUsernameAsync(username);

            allAssignments = await _applicationService.EmployeesXTasksService.GetAllAssignmentsOfUserAsync(user!.Id);

            foreach (var assignment in allAssignments)
            {
                await _applicationService.EmployeesXTasksService.DeleteAssignmentAsync(username, assignment.TaskId);
            }

            return Ok();
        }

        [HttpGet]
        [Route("/api/employeesxtasks/getAllAssignedTasksOfUser/{userId}")]
        public async Task<ActionResult<List<EmployeeTask>>> GetAllAssignedTasksOfUser(int userId)
        {
            ICollection<EmployeeTask> allAssignments;
            ICollection<TaskReadOnlyDTO> allUsersTasks = new HashSet<TaskReadOnlyDTO>();

            User? user = await _applicationService.UserService.GetUserByIdAsync(userId);

            allAssignments = await _applicationService.EmployeesXTasksService.GetAllAssignmentsOfUserAsync(user!.Id);

            foreach (var assignment in allAssignments)
            {
                TaskReadOnlyDTO tmpDto = new TaskReadOnlyDTO();
                var taskToFind = await _applicationService.TaskService.GetTaskAsync(assignment.TaskId);

                tmpDto.Title = taskToFind!.Title;
                tmpDto.Description = taskToFind.Description;
                tmpDto.Deadline = taskToFind.Deadline;

                allUsersTasks.Add(tmpDto);
            }

            if (allUsersTasks is null)
                return NoContent();

            return Ok(allUsersTasks);
        }
    }
}
