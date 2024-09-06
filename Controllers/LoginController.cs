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

        // Inject the DbContext into the controller
        public LoginController(WhoknowsContext context)
        {
            _context = context;
        }

        //[HttpPost]
        //public async Task<IActionResult> Login([FromBody] LoginRequest2 loginRequest)
        //{
        //    if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
        //    {
        //        return BadRequest("Invalid login request.");
        //    }

        //    // Find user by username
        //    var user = await _context.Users
        //        .FirstOrDefaultAsync(u => u.Username == loginRequest.Username);

        //    // Check if user exists and password matches
        //    if (user == null || user.Password != loginRequest.Password)
        //    {
        //        return Unauthorized("Invalid username or password.");
        //    }

        //    // If successful, return a success message or token (for demonstration purposes, just return OK)
        //    return Ok("Login successful");
        //}
    }
}
