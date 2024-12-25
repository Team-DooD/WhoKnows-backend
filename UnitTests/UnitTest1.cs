using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhoKnows_backend.Controllers;
using WhoKnows_backend.Models;
using Xunit;

public class SearchControllerTests
{
    private readonly Mock<WhoknowsContext> _mockContext;
    private readonly SearchController _controller;

    public SearchControllerTests()
    {
        // Using InMemory Database for testing
        var options = new DbContextOptionsBuilder<WhoknowsContext>()
            .UseInMemoryDatabase(databaseName: "WhoknowsTestDb")
            .Options;

        var context = new WhoknowsContext(options);
    
        SeedDatabase(context);
      

        using (var testContext = new WhoknowsContext(options))
        {
            SeedDatabase(testContext);
            GetDatabase(testContext);
        }

        _controller = new SearchController(context);
    }

    private void SeedDatabase(WhoknowsContext context)
    {
        // Clear existing data to avoid conflicts
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Pages.AddRange(
            new Page
            {
                Language = "en",
                Content = "This is a test page",
                Title = "Test1",
                CreatedBy = "System", 
                Url = "/test1"    
            },
            new Page
            {
                Language = "en",
                Content = "Another test page",
                Title = "Test2",
                CreatedBy = "System",
                Url = "/test2"      
            },
            new Page
            {
                Language = "da",
                Content = "En testside",
                Title = "Test3",
                CreatedBy = "System",
                Url = "/test3"       
            }
        );

        context.SaveChanges();
    }


    private void GetDatabase(WhoknowsContext context)
    {
        var pages = context.Pages.OrderBy(a => a.Id).ToList();

        foreach (var page in pages)
        {
            Console.WriteLine($"Id: {page.Id}, Title: {page.Title}, Language: {page.Language}, Content: {page.Content}");
        }
    }




    [Fact]
    public async Task ApiSearch_ReturnsCorrectResults_WhenQueryIsProvided()
    {
        string query = "test";
        string language = "en";

        var result = await _controller.Search(query, language);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var searchResults = Assert.IsAssignableFrom<List<Page>>(okResult.Value);
        Assert.Equal(2, searchResults.Count);
    }


    // This test should be changed to support multiple types since scraping is used now and fix the path to use dynamic path for that python script????...........
    //[Fact]
    //public async Task ApiSearch_ReturnsEmptyList_WhenQueryIsNotProvided()
    //{
    //    //var result = await _controller.Search(null);

    //    // Assert
    //    //var okResult = Assert.IsType<OkObjectResult>(result);
        
    //    //var searchResults = Assert.IsAssignableFrom<List<Page>>(okResult.Value);
    //    //Assert.Empty(searchResults); // Query is null, so result should be empty
    //}

    [Fact]
    public async Task ApiSearch_FiltersByLanguage()
    {
      
        string query = "test";
        string language = "da";

       
        var result = await _controller.Search(query, language);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var searchResults = Assert.IsAssignableFrom<List<Page>>(okResult.Value);
        Assert.Single(searchResults); // Only one Danish page contains "test"
        Assert.Equal("da", searchResults.First().Language);
    }
}
