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
using System.Security.Claims;
using System.Threading.Tasks;

namespace e_commerce.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, ITokenService tokenService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }



        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            var user = _mapper.Map<User>(registerDto);

            user.UserName = registerDto.username.ToLower();

            var result = await _userManager.CreateAsync(user, registerDto.password);

            if (!result.Succeeded)
            {
                var message = "";

                foreach (var item in result.Errors)
                {
                    message += $"{item.Code} ";
                }

                return BadRequest(new
                {
                    message
                });
            }



            var UserRole = await _roleManager.FindByNameAsync("User");

            user.Role = UserRole;

            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
            {
                var message = "";

                foreach (var item in roleResult.Errors)
                {
                    message += $"{item.Code} ";
                }

                return BadRequest(new
                {
                    message
                });
            }

            var cart = new Cart
            {
                UserId = user.Id
            };

            _unitOfWork.CartRepo.AddCart(cart);

            if (await _unitOfWork.Complete())
            {
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
            return BadRequest("Can not create the user");

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
            .SingleOrDefaultAsync(x => x.Email == loginDto.email);

            if (user == null)
            {
                return Unauthorized(new
                {
                    messages = "the email is incorrect"
                });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new
                {
                    messages = "the email or password is incorrect"
                });
            }

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

            if (user == null)
            {
                return Unauthorized("Invalid email");
            }

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