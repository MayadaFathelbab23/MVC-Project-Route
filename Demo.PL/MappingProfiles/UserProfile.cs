using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Models;

namespace Demo.PL.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
            CreateMap<ApplicationUser, UserEditViewModel>().ReverseMap();
        }
    }
}
