namespace CompanyProject.Repositories
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }
        public ITaskRepository TaskRepository { get; }
        public IEmployeesXTasksRepository EmployeesXTasksRepository { get; }
        Task<bool> SaveAsync();
    }
}
