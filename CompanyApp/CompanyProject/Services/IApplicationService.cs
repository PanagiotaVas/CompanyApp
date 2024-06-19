namespace CompanyProject.Services
{
    public interface IApplicationService
    {
        IUserService UserService { get; }
        IEmployeeService EmployeeService { get; }
        ITaskService TaskService { get; }
        IEmployeesXTasksService EmployeesXTasksService { get; }
    }
}
