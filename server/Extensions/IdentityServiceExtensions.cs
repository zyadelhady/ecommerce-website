using System.Text;
using System.Threading.Tasks;
using e_commerce.Data;
using e_commerce.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace e_commerce.Extensions
{
  public static class IdentityServiceExtensions
  {
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {

      services.AddIdentityCore<User>(opt =>
      {
        opt.Password.RequireNonAlphanumeric = false;
        opt.User.RequireUniqueEmail = true;
      })
      .AddRoles<Role>()
      .AddRoleManager<RoleManager<Role>>()
      .AddSignInManager<SignInManager<User>>()
      .AddRoleValidator<RoleValidator<Role>>()
      .AddEntityFrameworkStores<DataContext>();



      // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
      //      {
      //        options.TokenValidationParameters = new TokenValidationParameters
      //        {
      //          ValidateIssuerSigningKey = true,
      //          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
      //          ValidateIssuer = false,
      //          ValidateAudience = false
      //        };

      //        options.Events = new JwtBearerEvents
      //        {
      //          OnMessageReceived = context =>
      //          {
      //            var accessToken = context.Request.Query["access_token"];

      //            var path = context.HttpContext.Request.Path;

      //            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
      //            {
      //              context.Token = accessToken;
      //            }
      //            return Task.CompletedTask;
      //          }
      //        };
      //      });

      // services.AddAuthorization(opt =>
      // {
      //   opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
      // });

      return services;
    }
  }
}