﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Security;
using Prometheus;
using System.Linq;
using System.Threading.Tasks;
using WhoKnows_backend.Models;
using WhoKnows_backend.Scripts;


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
            // Starts the timer for measuring the search duration
            using (var timer = searchDurationHistogram.NewTimer()) 
            {
                // Increment the counter for search requests for benchmarking
                searchRequestCounter.Inc();

                // The mighty search logic itself --->>
                var searchResults = string.IsNullOrEmpty(q)
                    ? new List<Page>()
                    : await _context.Pages
                        .Where(p => p.Language == language && p.Content.ToLower().Contains(q.ToLower()))
                        .ToListAsync();

                if (!searchResults.Any())
                {
                    string scrapeResult = await FetchData.FetchCombinedParagraphs(q);

                    var scrapedPages = System.Text.Json.JsonSerializer.Deserialize<List<Page>>(scrapeResult);

                    if (scrapedPages[0].Content.Length > 240)
                    {
                        _context.Pages.AddRange(scrapedPages);
                        await _context.SaveChangesAsync();

                        var scrapeResultSearch = string.IsNullOrEmpty(q)
                          ? new List<Page>()
                          : await _context.Pages
                                .Where(p => p.Language == language && p.Content.ToLower().Contains(q.ToLower()))
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
            } // Now we log the duration to Prometheus
        }

    }
}