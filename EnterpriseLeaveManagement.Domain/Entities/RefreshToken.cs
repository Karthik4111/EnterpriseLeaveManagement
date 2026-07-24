using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.Domain.Entities;


public class RefreshToken
{
    public Guid Id { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime Expires { get; set; }

    public DateTime Created { get; set; }

    public bool IsRevoked { get; set; }

    public Guid UserId { get; set; }

    public DateTime? Revoked { get; set; }

    public bool IsExpired => DateTime.UtcNow >= Expires;

    public bool IsActive => !IsRevoked && !IsExpired;
}