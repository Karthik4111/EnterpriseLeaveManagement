using EnterpriseLeaveManagement.Application.Features.Authentication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EnterpriseLeaveManagement.Application.Common.Interfaces;

public interface IJwtTokenService
{
    Task<TokenResponseDto> GenerateTokenAsync(Guid userId,string userName,string email,IList<string> roles);
}