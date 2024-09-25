using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhoKnows_backend.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;
using WhoKnows_backend.DTO;

namespace WhoKnows_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly WhoknowsContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        // Inject the DbContext into the controllerssss
        public LoginController(WhoknowsContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpPost("")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid login request.");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            // Initialize PasswordHasher
            var passwordHasher = new PasswordHasher<User>();

            // Verify the hashed password
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid username or password");
            }

            // Generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("123456789123456N123456789123456N"); // Store securely, e.g., in configuration
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

        public class LoginRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        // Example of an authenticated endpoint
        [Authorize]
        [HttpGet("authenticated-endpoint")]
        public IActionResult AuthenticatedEndpoint()
        {
            // Check if the user is logged in
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            //{
            //    return Unauthorized("You are not logged in");
            //}

            return Ok("You are authenticated");
        }


        private bool VerifyPassword(string storedPassword, string inputPassword)
        {
            // Implement your password verification logic here (e.g., hashing)
            return storedPassword == inputPassword; // Simplified for demonstration
        }

        private string HashPassword(string password)
        {
            // Implement your password hashing logic here
            return password; // Simplified for demonstration
        }


    }
}
