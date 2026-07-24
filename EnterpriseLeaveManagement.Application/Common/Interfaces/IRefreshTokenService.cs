using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Domain.Entities;

namespace EnterpriseLeaveManagement.Application.Interfaces;

public interface IRefreshTokenService
{
    RefreshToken GenerateRefreshToken(Guid userId);

    Task<RefreshToken?> GetRefreshTokenAsync(string token);

    Task SaveRefreshTokenAsync(RefreshToken refreshToken);

    Task RevokeRefreshTokenAsync(string token);

    Task RevokeAllUserRefreshTokensAsync(Guid userId);

}