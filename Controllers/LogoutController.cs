using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace WhoKnows_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogoutController : ControllerBase
    {
        // Logout endpoint
        [HttpPost("")]
        public IActionResult Logout()
        {
            // Check if specific cookies exist and remove them
            if (Request.Cookies.ContainsKey(".AspNetCore.Antiforgery.tib6llArwhY"))
            {
                Response.Cookies.Delete(".AspNetCore.Antiforgery.tib6llArwhY");
            }

            // Clear session
            HttpContext.Session.Clear();

            return Ok("Logged out successfully");
        }
    }
}
