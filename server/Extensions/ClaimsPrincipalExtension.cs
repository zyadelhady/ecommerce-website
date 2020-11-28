using System;
using System.Security.Claims;

namespace e_commerce.Extensions
{
  public static class ClaimsPrincipalExtension
  {
    public static string GetUsername(this ClaimsPrincipal user)
    {
      return user.FindFirst(ClaimTypes.Name)?.Value;
    }

    public static Guid GetUserId(this ClaimsPrincipal user)
    {
      return new Guid(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    }
  }
}