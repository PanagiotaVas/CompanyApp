using AutoMapper;
using CompanyProject.Repositories;

namespace CompanyProject.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService>? _logger;

        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService>? logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public IUserService UserService => new UserService(_unitOfWork, _logger, _mapper);
        public IEmployeeService EmployeeService => new EmployeeService(_unitOfWork, _logger, _mapper);
        public ITaskService TaskService => new TaskService(_unitOfWork, _logger, _mapper);
        public IEmployeesXTasksService EmployeesXTasksService => new EmployeesXTasksService(_unitOfWork, _logger, _mapper);

    }
}
