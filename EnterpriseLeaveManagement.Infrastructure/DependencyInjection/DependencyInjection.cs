using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Common.Services;
using EnterpriseLeaveManagement.Infrastructure.Authentication;
using EnterpriseLeaveManagement.Infrastructure.Identity;
using EnterpriseLeaveManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EnterpriseLeaveManagement.Infrastructure.Email;

namespace EnterpriseLeaveManagement.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));
        });

        // Register ASP.NET Core Identity
        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // Configure JWT Settings
        services.Configure<JwtSettings>(
            configuration.GetSection("JwtSettings"));

        var jwtSettings =
            configuration.GetSection("JwtSettings").Get<JwtSettings>()!;

        // Configure JWT Authentication
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

                        ClockSkew = TimeSpan.Zero
                    };
            });

        

        // Register Application DbContext Interface
        services.AddScoped<IApplicationDbContext>(
            provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.AddScoped<IIdentityService, IdentityService>();

        services.AddScoped<IAuditService, AuditService>();

        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        services.AddScoped<IEmailService, EmailService>();

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}