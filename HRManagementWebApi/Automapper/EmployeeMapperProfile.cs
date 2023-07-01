using AutoMapper;
using CompanyManagement.Common.Dto;
using HRManagementWebApi.Database.Entities;

namespace HRManagementWebApi.Automapper
{
    public class EmployeeMapperProfile : Profile
    {

        public EmployeeMapperProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeLightDto, Employee>()
                .ForMember(e => e.StartDate, o => o.MapFrom(s => DateTime.Now));
        }
    }
}
