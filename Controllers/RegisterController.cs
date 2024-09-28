using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhoKnows_backend.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

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

        // Register new user endpoint
        [HttpPost("")]
        public async Task<IActionResult> Register([FromBody] DTO.RegisterRequest registerRequest)
        {
            if (registerRequest == null || string.IsNullOrEmpty(registerRequest.Username) || string.IsNullOrEmpty(registerRequest.Password))
            {
                return BadRequest("Invalid registration request.");
            }

            // Check if user already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == registerRequest.Username);
            if (existingUser != null)
            {
                return BadRequest("Username already taken.");
            }

            // Create new user and hash the password
            var newUser = new User
            {
                Username = registerRequest.Username,
                Email = registerRequest.Email
            };

            // Hash the password and store the hash
            newUser.Password = _passwordHasher.HashPassword(newUser, registerRequest.Password);

            // Add the new user to the database
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }
    }
}
