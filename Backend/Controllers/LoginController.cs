using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WhoKnows_backend.DTO;
using WhoKnows_backend.Models;

namespace WhoKnows_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly WhoknowsContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        // Inject the DbContext (the database context) into the controllerssss
        public LoginController(WhoknowsContext context, IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
            _configuration = configuration;
        }

        [HttpPost("")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid login request.");
            }

            var secretKey = _configuration["google:SecretKey"];

            if (string.IsNullOrEmpty(secretKey))
            {
                return StatusCode(500, "CAPTCHA secret key not configured.");
            }

            // CAPTCHA part
            using var httpClient = new HttpClient();
            var googleVerificationUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={request.CaptchaResponse}";

            var googleResponse = await httpClient.GetAsync(googleVerificationUrl);
            if (!googleResponse.IsSuccessStatusCode)
            {
                return StatusCode(500, "Failed to verify CAPTCHA.");
            }

            var captchaVerificationResult = await googleResponse.Content.ReadFromJsonAsync<GoogleCaptchaVerificationResponse>();
            if (captchaVerificationResult == null || !captchaVerificationResult.Success)
            {
                return BadRequest("CAPTCHA validation failed.");
            }

            // Authenticating the user ->
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            // Initialize PasswordHasher function here
            var passwordHasher = new PasswordHasher<User>();

            // Verify the hashed password using identity libary function
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid username or password");
            }

            // Generate JWT Token here
            var tokenHandler = new JwtSecurityTokenHandler();

            // PLZ Store securely, e.g., in configuration
            var keyToUse = _configuration["Jwt:SecretKey"];

            var key = Encoding.ASCII.GetBytes(keyToUse); 
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

            public string CaptchaResponse { get; set; } = string.Empty;
        }

        public class GoogleCaptchaVerificationResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("challenge_ts")]
            public DateTime ChallengeTimestamp { get; set; }

            [JsonProperty("hostname")]
            public string Hostname { get; set; }

            [JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }
        }

    }
}