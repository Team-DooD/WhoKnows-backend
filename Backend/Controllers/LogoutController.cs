using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace WhoKnows_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogoutController : ControllerBase
    {
        // Logout endpoint
        [HttpPost("")]
        public IActionResult Logout()
        {
            // Check if specific cookies exist and remove them
            foreach (var cookie in Request.Cookies.Keys)
            {
                // Check if the cookie name contains the antiforgery token prefix
                if (cookie.StartsWith(".AspNetCore.Antiforgery"))
                {
                    // Remove the antiforgery cookie
                    Response.Cookies.Delete(cookie);
                }
            }
            // Clear session
            HttpContext.Session.Clear();

            return Ok("Logged out successfully");
        }
    }
}
