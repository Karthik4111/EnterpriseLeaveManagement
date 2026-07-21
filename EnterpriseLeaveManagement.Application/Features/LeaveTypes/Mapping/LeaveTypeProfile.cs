using AutoMapper;
using EnterpriseLeaveManagement.Application.Features.LeaveTypes.DTOs;
using EnterpriseLeaveManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EnterpriseLeaveManagement.Application.Features.LeaveTypes.Mapping;

public class LeaveTypeProfile : Profile
{
    public LeaveTypeProfile()
    {
        CreateMap<LeaveType, LeaveTypeDto>().ReverseMap();
    }
}