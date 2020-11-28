using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_commerce.Data;
using e_commerce.DTOs;
using e_commerce.Entities;
using e_commerce.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Controllers
{
  public class UsersController : BaseApiController
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly DataContext _context;
    public UsersController(IUnitOfWork unitOfWork, DataContext context)
    {
      _context = context;
      _unitOfWork = unitOfWork;
    }

    // [HttpPost("add-user")]
    // public async Task<ActionResult<UserDto>> AddUser(RegisterDto RegisterUser)
    // {
    //   var user = _unitOfWork.UserRepository.AddUser(RegisterUser.username, RegisterUser.email, RegisterUser.password);

    //   return Ok(user);
    // }

    // [Authorize(Roles = "Member")]
    // [HttpGet]
    // public async Task<ActionResult> Users()
    // {
    //   var users = await _context.Users
    //   .Include(u => u.Role)
    //   .Select(u => new
    //   {
    //     u.Id,
    //     u.UserName,
    //     Roles = u.Role.Name
    //   })
    //   .ToListAsync();

    //   return Ok(users);
    // }

  }
}