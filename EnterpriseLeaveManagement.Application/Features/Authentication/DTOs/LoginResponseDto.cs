using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.Application.Features.Authentication.DTOs;

public class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
}