using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Threading.Tasks;
using FluentAssertions;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace CRUD_Tests
{
    public class PersonsControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {

        // Field to store an HttpClient instance for sending requests to the test server
        private readonly HttpClient _httpClient;

        // Constructor for the integration test class
        // Takes a CustomWebApplicationFactory instance to set up the test server
        public PersonsControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            // Create an HttpClient from the factory, preconfigured for the test environment
            _httpClient = factory.CreateClient();
        }

        #region Index
        [Fact]
        public async Task Index_ToReturnView()
        {
            // Arrange


            // Act
            HttpResponseMessage response = await _httpClient.GetAsync("/Persons/Index");

            // Assert
            response.Should().BeSuccessful(); // 2xx

            // Read response body
            string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseBody);
            HtmlNode document = htmlDoc.DocumentNode;

            document.QuerySelectorAll("table.persons").Should().NotBeNull();

        }
        #endregion

    }
}
