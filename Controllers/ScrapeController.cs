using Microsoft.AspNetCore.Mvc;
using WhoKnowsBackend.Scripts; // Ensure the correct namespace is imported for PythonScriptExecutor

namespace WhoKnows_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScrapeController : ControllerBase
    {

        // API endpoint for scraping data
        [HttpGet("")]
        public IActionResult ScrapeApi([FromQuery] string quary = null)
        {
            if (string.IsNullOrEmpty(quary))
            {
                return BadRequest("quary must be provided for scraping.");
            }

            try
            {
                // Call ExecutePythonScript from the PythonScriptExecutor class in the Scripts folder
                string result = PythonScriptExecutor.ExecutePythonScript("scripts/fetchData.py", quary);

                return Ok(new { ScrapedContent = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
