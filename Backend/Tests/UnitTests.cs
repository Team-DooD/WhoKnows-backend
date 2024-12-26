//using Moq;  // Add this using directive to work with Moq
//using Microsoft.Extensions.Configuration;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using WhoKnows_backend.Models;
//using WhoKnows_backend.Controllers;
//using Xunit;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.VisualStudio.TestPlatform.TestHost;

//namespace UnitTests
//{
//    public class SearchControllerTests : IClassFixture<WebApplicationFactory<Program>>
//    {
//        private readonly HttpClient _client;
//        private readonly WebApplicationFactory<Program> _factory;

//        public SearchControllerTests(WebApplicationFactory<Program> factory)
//        {
//            _client = factory.CreateClient();
//            _factory = factory;
//        }

//        // Helper method to create an in-memory database with test data
//        private WhoknowsContext CreateInMemoryContext()
//        {
//            // Mocking IConfiguration
//            var mockConfig = new Mock<IConfiguration>();

//            // Setting up DbContextOptions for InMemoryDatabase
//            var options = new DbContextOptionsBuilder<WhoknowsContext>()
//                .UseInMemoryDatabase("TestDatabase")
//                .Options;

//            // Now pass the mock configuration into the context constructor
//            var context = new WhoknowsContext(options, mockConfig.Object);

//            // Adding some test data to the in-memory database
//            context.Pages.AddRange(
//                new Page { Id = 1, Content = "Hot weather in California", Language = "en" },
//                new Page { Id = 2, Content = "Cold weather in Alaska", Language = "en" },
//                new Page { Id = 3, Content = "Hot food recipe", Language = "en" }
//            );
//            context.SaveChanges();

//            return context;
//        }

//        [Fact]
//        public async Task Search_Returns_WeatherForecast()
//        {
//            // Arrange: Create an in-memory context with test data
//            var context = CreateInMemoryContext();

//            // Inject the context into the controller
//            var controller = new SearchController(context);

//            // Act: Call the Search method with the query "Hot"
//            var result = await controller.Search("Hot");

//            // Assert: Check that the result is OK (status code 200) and contains the expected pages
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var pages = Assert.IsAssignableFrom<List<Page>>(okResult.Value);
//            Assert.Equal(2, pages.Count); // Expected 2 pages containing "Hot"
//        }

//        [Fact]
//        public async Task ApiSearch_Returns_WeatherForecast()
//        {
//            // Arrange: Create an in-memory context with test data
//            var context = CreateInMemoryContext();

//            // Inject the context into the controller
//            var controller = new SearchController(context);

//            // Act: Call the ApiSearch method with the query "Cold"
//            var result = await controller.ApiSearch("Cold");

//            // Assert: Check that the result is OK (status code 200) and contains the expected pages
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var pages = Assert.IsAssignableFrom<List<Page>>(okResult.Value);
//            Assert.Equal(1, pages.Count); // Expected 1 page containing "Cold"
//        }

//        [Fact]
//        public async Task ApiSearch_Returns_EmptyList_When_NoQuery()
//        {
//            // Arrange: Create an in-memory context with test data
//            var context = CreateInMemoryContext();

//            // Inject the context into the controller
//            var controller = new SearchController(context);

//            // Act: Call the ApiSearch method with no query (empty)
//            var result = await controller.ApiSearch(null);

//            // Assert: Check that the result is OK (status code 200) and the list is empty
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var pages = Assert.IsAssignableFrom<List<Page>>(okResult.Value);
//            Assert.Empty(pages); // No pages should match if no query
//        }
//    }
//}