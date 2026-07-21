using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(bool Succeeded, IEnumerable<string> Errors)> RegisterUserAsync(
        string firstName,
        string lastName,
        string userName,
        string email,
        string password,
        string role);

    Task<(bool Succeeded, string? Token, IEnumerable<string> Errors)> LoginAsync(
        string email,
        string password);
}