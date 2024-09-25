using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WhoKnows_backend.Entities;

namespace WhoKnows_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly WhoknowsContext _context;

        public SearchController(WhoknowsContext context)
        {
            _context = context;
        }

        // HTML endpoint for search
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

        // API endpoint for search
        [HttpGet("")]
        public async Task<IActionResult> ApiSearch([FromQuery] string q = null, [FromQuery] string language = "en")
        {
            var searchResults = string.IsNullOrEmpty(q)
                ? new List<Page>()
                : await _context.Pages
                    .Where(p => p.Language == language && p.Content.Contains(q))
                    .ToListAsync();

            return Ok(new { searchResults });
        }
    }
}

