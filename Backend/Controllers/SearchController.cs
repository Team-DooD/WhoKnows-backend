using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Security;
using Prometheus;
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

        // Set up the counter for search-api requests 
        private static readonly Counter searchRequestCounter = Metrics.CreateCounter("api_requests_total", "Total number of API requests made.");
        // Set up the trackeing of time for each seacrch request
        private static readonly Histogram searchDurationHistogram = Metrics.CreateHistogram("search_duration_seconds", "Histogram of search durations in seconds");

        public SearchController(WhoknowsContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Search([FromQuery] string q = null, [FromQuery] string language = "en")
        {

            // Starts the timer for meassuring the search duration
            var timer = searchDurationHistogram.NewTimer();

            // Increment the timer for search request
            searchRequestCounter.Inc();

            // The search logic itself
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
                    await _context.SaveChangesAsync();

                    var scrapeResultSearch = string.IsNullOrEmpty(q)
                      ? new List<Page>()
                      : await _context.Pages
                          .Where(p => p.Language == language && p.Content.Contains(q))
                          .ToListAsync();
                    if (!scrapeResultSearch.Any())
                    {
                        return BadRequest("No valid data scraped.");
                    }
                    else
                    {
                        return Ok(scrapeResultSearch);
                    }

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