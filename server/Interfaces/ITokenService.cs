using System.Security.Claims;
using System.Threading.Tasks;
using e_commerce.Entities;

namespace e_commerce.Interfaces
{
  public interface ITokenService
  {
    Task<ClaimsIdentity> CreateToken(User user);
  }
}