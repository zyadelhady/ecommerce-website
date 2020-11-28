using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using e_commerce.Entities;
using e_commerce.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace e_commerce.services
{
  public class TokenService : ITokenService
  {
    private readonly UserManager<User> _userManager;
    public TokenService(IConfiguration config, UserManager<User> userManager)
    {
      _userManager = userManager;
    }

    public async Task<ClaimsIdentity> CreateToken(User user)
    {
      var claims = new List<Claim>
      {
          new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
          new Claim(ClaimTypes.Name,user.UserName.ToString()),
      };

      var roles = await _userManager.GetRolesAsync(user);

      claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

      return new ClaimsIdentity(
        claims, CookieAuthenticationDefaults.AuthenticationScheme);

    }
  }
}