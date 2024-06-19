
using AutoMapper;
using CompanyProject.Data;

namespace CompanyProject.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyProjectDbContext _context;
        private readonly IMapper _mapper;

        public UnitOfWork(CompanyProjectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IUserRepository UserRepository => new UserRepository(_context, _mapper);

        public IEmployeeRepository EmployeeRepository => new EmployeeRepository(_context);

        public ITaskRepository TaskRepository => new TaskRepository(_context);

        public IEmployeesXTasksRepository EmployeesXTasksRepository => new EmployeesXTasksRepository(_context);
        
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
