using EnterpriseLeaveManagement.Application.Features.Authentication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(bool Succeeded, Guid? UserId, IEnumerable<string> Errors)> RegisterUserAsync(
        string firstName,
        string lastName,
        string userName,
        string email,
        string password,
        string role);

    Task<(bool Succeeded, TokenResponseDto? Token, IEnumerable<string> Errors)> LoginAsync(
    string email,
    string password);

    Task<bool> UserExistsAsync(Guid userId);

    Task<IList<string>> GetUserRolesAsync(Guid userId);

    Task<(Guid Id, string UserName, string Email)?> GetUserAsync(Guid userId);
}