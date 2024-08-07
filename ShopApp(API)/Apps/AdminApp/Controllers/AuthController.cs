using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShopApp_API_.Apps.AdminApp.Dtos.UserDto;
using ShopApp_API_.Entities;
using ShopApp_API_.Services.Interfaces;
using ShopApp_API_.Settings;

namespace ShopApp_API_.Apps.AdminApp.Controllers
{

    public class AuthController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;

        public AuthController(IOptions<JwtSettings> jwtSettings, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("register")]
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

            await _userManager.AddToRoleAsync(user, "admin");

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


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            //var user = await _userManager.FindByEmailAsync(loginDto.Email);

            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user is null) return BadRequest("Username or Email is wrong");


            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) return BadRequest();

            //jwt
            var roles = await _userManager.GetRolesAsync(user);
            var secretKet = _jwtSettings.SecretKey;
            var audience = _jwtSettings.Audience;
            var issuer = _jwtSettings.Issuer;

            return Ok(new { token = _tokenService.GetToken(secretKet, audience, issuer, user, roles) });
        }


        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null) return BadRequest();

            return Ok(_mapper.Map<UserGetDto>(user));
        }
    }
}
