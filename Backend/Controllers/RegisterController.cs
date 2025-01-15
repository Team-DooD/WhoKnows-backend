using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WhoKnows_backend.Models;

namespace WhoKnows_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly WhoknowsContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public RegisterController(WhoknowsContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpPost("")]
        public async Task<IActionResult> Register([FromBody] DTO.RegisterRequest registerRequest)
        {
            if (registerRequest == null || string.IsNullOrEmpty(registerRequest.Username) || string.IsNullOrEmpty(registerRequest.Password))
            {
                return BadRequest("Invalid registration request.");
            }

            // Check user already exists in db
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == registerRequest.Username);
            if (existingUser != null)
            {
                return BadRequest("Username already taken.");
            }

            // Create new user and hash the password using identity
            var newUser = new User
            {
                Username = registerRequest.Username,
                Email = registerRequest.Email
            };

            // Hash the password and store the hash again
            newUser.Password = _passwordHasher.HashPassword(newUser, registerRequest.Password);

            // new user to the database here
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }
    }
}