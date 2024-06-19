using AutoMapper;
using CompanyProject.Data;
using CompanyProject.DTO;
using EmplTask = CompanyProject.Data.Task;

namespace CompanyProject.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig() 
        { 
            CreateMap<User,UserPatchDTO>().ReverseMap();
            CreateMap<User, UserSignUpDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserReadOnlyDTO>().ReverseMap();
            CreateMap<TaskDTO, TaskReadOnlyDTO>().ReverseMap();
            CreateMap<TaskDTO, EmplTask>().ReverseMap();
            CreateMap<TaskDTO, TaskUpdateDTO>().ReverseMap();
            CreateMap<Employee, EmployeeReadOnlyDTO>().ReverseMap();
        }
    }
}
