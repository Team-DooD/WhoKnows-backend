using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WhoKnows_backend.Models;
using WhoKnowsBackend.Scripts;

namespace WhoKnows_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly WhoknowsContext _context;

        public SearchController(WhoknowsContext context)
        {
            _context = context;
        }

        // HTML endpoint for search.
        [HttpGet("Html")]
        public async Task<IActionResult> Search([FromQuery] string q = null, [FromQuery] string language = "en")
        {
            var searchResults = string.IsNullOrEmpty(q)
                ? new List<Page>()
                : await _context.Pages
                    .Where(p => p.Language == language && p.Content.Contains(q))
                    .ToListAsync();

            return Ok(searchResults); 
        }

      [HttpGet("")]
        public async Task<IActionResult> ApiSearch([FromQuery] string q = null, [FromQuery] string language = "en")
        {

            var searchResults = string.IsNullOrEmpty(q)
              ? new List<Page>()
              : await _context.Pages
                  .Where(p => p.Language == language && p.Content.Contains(q))
                  .ToListAsync();

            if (!searchResults.Any())
            {
                var scarpeResult = PythonScriptExecutor.ExecutePythonScript("scripts/fetchData.py", q);
                var scrapedPages = System.Text.Json.JsonSerializer.Deserialize<List<Page>>(scarpeResult);


                if (scrapedPages[0].Content.Length > 240)
                {
                    _context.Pages.AddRange(scrapedPages);
                    await _context.SaveChangesAsync(); // Save changes to the database

                    return Ok(scrapedPages);
                }
                else
                {
                    return BadRequest("No valid data scraped.");
                }
            }
            else
            {
                return Ok(searchResults);
            }
        }
    }
}
