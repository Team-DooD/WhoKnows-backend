using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HtmlAgilityPack;  


namespace WhoKnows_backend.Scripts
{
    // This class contains our magic the logic for fetching and processing data from Wikipedia.
    public static class FetchData
    {
        // fetch combined paragraphs from a Wikipedia
        public static async Task<string> FetchCombinedParagraphs(string keyword)
        {
            string url = $"https://en.wikipedia.org/wiki/{Uri.EscapeDataString(keyword)}";
            using HttpClient client = new HttpClient();

            try
            {
                // Fetch the HTML content from the Wikipedia page
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode(); 

                // Read the HTML content of the page
                string htmlContent = await response.Content.ReadAsStringAsync();
                // Using HtmlAgilityPack's HtmlDocument
                var htmlDoc = new HtmlDocument();
                // Load HTML into HtmlDocument
                htmlDoc.LoadHtml(htmlContent); 

                // Extract the title from the page here
                string title = htmlDoc.DocumentNode.SelectSingleNode("//h1")?.InnerText ?? "Unknown Title";

                // Extract the first 10 paragraphs from the page
                var paragraphs = htmlDoc.DocumentNode.SelectNodes("//p");
                string combinedText = paragraphs?.Take(10)
                                              .Select(p => p.InnerText)
                                              .Aggregate((current, next) => current + " " + next) ?? "";

                // Result object with the combined scraped formatted data
               
                var result = new[]
                {
                    new
                    {
                        Title = title,
                        Url = url,
                        CreatedBy = "",
                        Language = "en",
                        LastUpdated = DateTime.UtcNow.ToString("o"),
                        Content = combinedText,
                        Id = 0
                    }
                };

                // Result as a JSON string
                return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (HttpRequestException e)
            {
                // Handle any errors then give empty result with error content
                var errorResult = new[]
                {
                    new
                    {
                        Title = "",
                        Url = url,
                        CreatedBy = "",
                        Language = "en",
                        LastUpdated = DateTime.UtcNow.ToString("o"),
                        Content = $"Error fetching the page: {e.Message}",
                        Id = 0
                    }
                };

             
                return JsonSerializer.Serialize(errorResult, new JsonSerializerOptions { WriteIndented = true });
            }
        }
    }
}
