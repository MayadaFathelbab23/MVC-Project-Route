using AutoMapper;
using Demo.PL.Controllers;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;

namespace Demo.PL.MappingProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleViewModel , IdentityRole>()
                .ForMember(d=>d.Name , o => o.MapFrom(s=>s.RoleName)).ReverseMap();
        }
    }
}
