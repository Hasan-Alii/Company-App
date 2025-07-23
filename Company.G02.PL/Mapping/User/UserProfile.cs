using AutoMapper;
using Company.G02.DAL.Models;
using Company.G02.PL.ViewModels;

namespace Company.G02.PL.Mapping.User
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
        }
    }
}
