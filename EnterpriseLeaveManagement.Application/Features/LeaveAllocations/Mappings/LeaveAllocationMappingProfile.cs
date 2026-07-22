using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EnterpriseLeaveManagement.Application.Features.LeaveAllocations.DTOs;
using EnterpriseLeaveManagement.Domain.Entities;

namespace EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Mappings;

public class LeaveAllocationMappingProfile : Profile
{
    public LeaveAllocationMappingProfile()
    {
        CreateMap<LeaveAllocation, LeaveAllocationDto>()
            .ForMember(dest => dest.EmployeeName,
                opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName))
            .ForMember(dest => dest.LeaveTypeName,
                opt => opt.MapFrom(src => src.LeaveType.Name));

        CreateMap<LeaveAllocationDto, LeaveAllocation>();
    }
}