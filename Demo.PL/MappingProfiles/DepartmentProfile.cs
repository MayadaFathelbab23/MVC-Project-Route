using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Models;

namespace Demo.PL.MappingProfiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile() { 
            CreateMap<Department , DepartmentViewModel>().ReverseMap();
        }
    }
}
