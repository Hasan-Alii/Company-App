using AutoMapper;
using Company.G02.DAL.Models;
using Company.G02.PL.ViewModels.Employee;

namespace Company.G02.PL.Mapping.Employee
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Company.G02.DAL.Models.Employee, EmployeeViewModel>().ReverseMap();
        }
    }
}
