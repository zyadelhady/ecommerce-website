using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using e_commerce.DTOs;
using e_commerce.Entities;
using e_commerce.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Data
{
  public class UserRepository : IUserRepository
  {
    private readonly DataContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UserRepository(DataContext context, IMapper mapper)
    {
      _mapper = mapper;
      _context = context;
    }

    public async Task<UserDto> AddUser(string username, string email, string password)
    {
      var user = new User
      {
        UserName = username,
        Email = email,
      };

      _context.Users.Add(user);


      if (await _context.SaveChangesAsync() < 0) return null;

      return new UserDto
      {
        Id = user.Id,
        UserName = user.UserName,
        Email = user.Email,
      };
    }

    public Task<IEnumerable<UserDto>> GetUsers()
    {
      //   var query = await _context.Users.ToListAsync();

      //   foreach (var item in query)
      //   {

      //   }
      //   return new UserDto
      //   {
      //     UserName = query.UserName
      //   };

      throw new System.NotImplementedException();

    }
  }
}