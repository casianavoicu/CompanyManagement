using AutoMapper;
using CompanyManagement.Common.Dto;
using HRManagementWebApi.Database.Entities;

namespace HRManagementWebApi.Automapper
{
    public class EmployeeMapperProfile : Profile
    {
        public EmployeeMapperProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(e => e.EmployeeId, o => o.MapFrom(s => s.Id));
            CreateMap<EmployeeLightDto, Employee>();
        }
    }
}