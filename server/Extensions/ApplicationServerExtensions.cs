using AutoMapper;
using e_commerce.Data;
using e_commerce.Entities;
using e_commerce.Helpers;
using e_commerce.Interfaces;
using e_commerce.services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce.Extensions
{
  public static class ApplicationServerExtensions
  {

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
      services.AddScoped<IUnitOfWork, UnitOfWork>();
      services.AddScoped<ITokenService, TokenService>();
      services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

      services.AddDbContext<DataContext>(options =>
      {
        options.UseSqlite(config.GetConnectionString("DefaultConnection"));
      });

      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
      .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
      {
        opt.Cookie.SameSite = SameSiteMode.None;
        opt.Cookie.HttpOnly = true;
        opt.ExpireTimeSpan = TimeSpan.FromMinutes(1);
        opt.Events = new CookieAuthenticationEvents
        {
          OnRedirectToLogin = redirectContext =>
          {
            redirectContext.HttpContext.Response.StatusCode = 401;
            return Task.CompletedTask;
          }
        };
      });

      return services;
    }

  }
}
