using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhoKnows_backend.Entities;
using WhoKnows_backend.DTO;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace WhoKnows_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly WhoknowsContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public LoginController(WhoknowsContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest2 loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Invalid login request.");
            }

            // Find user by username
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginRequest.Username);

            // Check if user exists and password matches
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, loginRequest.Password) != PasswordVerificationResult.Success)
            {
                return Unauthorized("Invalid username or password.");
            }
            System.Console.WriteLine($"Username: {loginRequest.Username}");
            System.Console.WriteLine($"Password: {loginRequest.Password}");


            return Ok("Login successful");
        }


        // Register new user endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
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
