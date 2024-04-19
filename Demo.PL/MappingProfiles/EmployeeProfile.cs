using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Models;

namespace Demo.PL.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            // Mapp from EmployeeViewModel to Employee [Map by default using propert name]
            // EmployeeViewModel.Name -> Employee.Name , .....
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();

        }
    }
}
