using AutoMapper;
using EnterpriseLeaveManagement.Application.Features.LeaveRequests.DTOs;
using EnterpriseLeaveManagement.Domain.Entities;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Mapping;

public class LeaveRequestProfile : Profile
{
    public LeaveRequestProfile()
    {
        CreateMap<LeaveRequest, LeaveRequestDto>().ReverseMap();
    }
}