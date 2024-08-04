using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp_API_.Apps.AdminApp.Dtos.UserDto;
using ShopApp_API_.Entities;

namespace ShopApp_API_.Apps.AdminApp.Controllers
{

    public class AuthController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;



        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var existUser = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existUser != null) return Conflict();

            AppUser user = new()
            {
                FullName = registerDto.FullName,
                UserName = registerDto.UserName,
                Email = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "member");

            return StatusCode(201);
        }
        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            if (!await _roleManager.RoleExistsAsync("member"))
                await _roleManager.CreateAsync(new IdentityRole() { Name = "member" });

            if (!await _roleManager.RoleExistsAsync("admin"))
                await _roleManager.CreateAsync(new IdentityRole() { Name = "admin" });

            return StatusCode(201);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
            {
                user = await _userManager.FindByNameAsync(loginDto.UserName);

                if (user is null) return BadRequest("Username or Email is wrong");
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) return BadRequest();


        }
    }
}
