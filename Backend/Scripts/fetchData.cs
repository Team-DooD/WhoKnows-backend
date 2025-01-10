using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HtmlAgilityPack;  


namespace WhoKnows_backend.Scripts  // You can modify the namespace as needed
{
    // This class contains the logic for fetching and processing data from Wikipedia.
    public static class FetchData
    {
        // Method to fetch combined paragraphs from a Wikipedia page
        public static async Task<string> FetchCombinedParagraphs(string keyword)
        {
            string url = $"https://en.wikipedia.org/wiki/{Uri.EscapeDataString(keyword)}";
            using HttpClient client = new HttpClient();

            try
            {
                // Fetch the HTML content from the Wikipedia page
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Ensure the HTTP request is successful

                // Read the HTML content of the page
                string htmlContent = await response.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();  // Using HtmlAgilityPack's HtmlDocument
                htmlDoc.LoadHtml(htmlContent);    // Load HTML into HtmlDocument

                // Extract the title from the page
                string title = htmlDoc.DocumentNode.SelectSingleNode("//h1")?.InnerText ?? "Unknown Title";

                // Extract the first 10 paragraphs from the page
                var paragraphs = htmlDoc.DocumentNode.SelectNodes("//p");
                string combinedText = paragraphs?.Take(10)
                                              .Select(p => p.InnerText)
                                              .Aggregate((current, next) => current + " " + next) ?? "";

                // Create a result object with the combined data
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

                // Return the result as a JSON string
                return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (HttpRequestException e)
            {
                // Handle any errors that occur during the HTTP request
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

                // Return the error result as a JSON string
                return JsonSerializer.Serialize(errorResult, new JsonSerializerOptions { WriteIndented = true });
            }
        }
    }
}
