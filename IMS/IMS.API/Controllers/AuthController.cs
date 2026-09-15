using IMS.API.Data;
using IMS.API.DTOs;
using IMS.API.Models;
using IMS.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(
            ApplicationDbContext context,
            IJwtService jwtService,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _jwtService = jwtService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Email already exists."
                });
            }

            var role = await _roleManager.FindByNameAsync(request.UserRole);

            if (role == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid user role."
                });
            }

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                StoreId = request.StoreId,

                UserRoleId = role.Id,

                CreatedDate = DateTime.Now,
                IsActive = true
            };

            await _userManager.SetUserNameAsync(
                user,
                request.Email);

            await _userManager.SetEmailAsync(
                user,
                request.Email);

            var result = await _userManager.CreateAsync(
                user,
                request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Registration failed.",
                    errors = result.Errors.Select(x => x.Description)
                });
            }

            var roleResult = await _userManager.AddToRoleAsync(
                user,
                role.Name);

            if (!roleResult.Succeeded)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "User created but role assignment failed.",
                    errors = roleResult.Errors.Select(x => x.Description)
                });
            }

            return Ok(new
            {
                success = true,
                message = "Registration successful.",
                userId = user.Id
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Invalid email or password."
                });
            }

            if (!user.IsActive)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Your account is inactive."
                });
            }

            var passwordValid = await _userManager.CheckPasswordAsync(
                user,
                request.Password);

            if (!passwordValid)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Invalid email or password."
                });
            }

            // Get user's roles
            var roles = await _userManager.GetRolesAsync(user);

            var role = roles.FirstOrDefault() ?? string.Empty;

            // JWT will be generated here
            var token = _jwtService.GenerateToken(
                user.Id,
                user.Email,
                role);

            return Ok(new
            {
                success = true,
                message = "Login successful.",
                token = token,

                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    firstName = user.FirstName,
                    middleName = user.MiddleName,
                    lastName = user.LastName,
                    phoneNumber = user.PhoneNumber,
                    storeId = user.StoreId,
                    role = role,
                    profileUrl = user.ProfileUrl
                }
            });
        }
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles
                .Select(x => x.Name)
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = roles
            });
        }
    }
}
