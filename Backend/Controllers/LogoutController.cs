using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WhoKnows_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogoutController : ControllerBase
    {
       
        [HttpPost("")]
        public IActionResult Logout()
        {
            // Check if cookies exist and remove them here ----->
            foreach (var cookie in Request.Cookies.Keys)
            {
                // Checking if the cookie name contains the antiforgery token prefix from frontend
                if (cookie.StartsWith(".AspNetCore.Antiforgery"))
                {
                    // Remove antiforgery cookie from the server
                    Response.Cookies.Delete(cookie);
                }
            }
            // Clear session (deprecated but just to be sure)
            HttpContext.Session.Clear();

            return Ok("Logged out successfully");
        }
    }
}