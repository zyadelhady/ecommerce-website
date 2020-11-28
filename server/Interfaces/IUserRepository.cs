using System.Collections.Generic;
using System.Threading.Tasks;
using e_commerce.DTOs;

namespace e_commerce.Interfaces
{
  public interface IUserRepository
  {
    Task<IEnumerable<UserDto>> GetUsers();
    Task<UserDto> AddUser(string username, string email, string password);
  }
}