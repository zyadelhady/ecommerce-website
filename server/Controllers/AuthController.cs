using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using e_commerce.DTOs;
using e_commerce.Entities;
using e_commerce.Extensions;
using e_commerce.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Controllers
{
  public class AuthController : BaseApiController
  {
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<Role> _roleManager;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, ITokenService tokenService, IMapper mapper)
    {
      _roleManager = roleManager;
      _userManager = userManager;
      _mapper = mapper;
      _tokenService = tokenService;
      _signInManager = signInManager;
    }



    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {

      var user = _mapper.Map<User>(registerDto);

      user.UserName = registerDto.username.ToLower();

      var result = await _userManager.CreateAsync(user, registerDto.password);

      if (!result.Succeeded) return BadRequest(result.Errors);

      var MemberRole = await _roleManager.FindByNameAsync("Member");

      user.Role = MemberRole;

      var roleResult = await _userManager.AddToRoleAsync(user, "Member");

      if (!roleResult.Succeeded) return BadRequest(result.Errors);


      await HttpContext.SignInAsync(
          CookieAuthenticationDefaults.AuthenticationScheme,
          new ClaimsPrincipal(await _tokenService.CreateToken(user)));

      return new UserDto
      {
        Id = user.Id,
        UserName = user.UserName,
        Email = user.Email
      };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
      var user = await _userManager.Users
      .SingleOrDefaultAsync(x => x.Email == loginDto.email);

      if (user == null) return Unauthorized("Invalid email");

      var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.password, false);

      if (!result.Succeeded) return Unauthorized();

      await HttpContext.SignInAsync(
          CookieAuthenticationDefaults.AuthenticationScheme,
          new ClaimsPrincipal(await _tokenService.CreateToken(user)));

      return new UserDto
      {
        Id = user.Id,
        UserName = user.UserName,
        Email = user.Email
      };
    }


    [Authorize]
    [HttpGet("get-me")]
    public async Task<ActionResult<UserDto>> getMe()
    {

      var userId = User.GetUserId();

      var user = await _userManager.FindByIdAsync(userId.ToString());

      if (user == null) return Unauthorized("Invalid email");

      return new UserDto
      {
        Id = user.Id,
        UserName = user.UserName,
        Email = user.Email
      };
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
      await HttpContext.SignOutAsync(
        CookieAuthenticationDefaults.AuthenticationScheme);
      return NoContent();
    }

  }


}