using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace TestRecommendationApi
{
    public class RecommendationsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public RecommendationsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task PostRecommendations_ReturnsNonEmptyResponse()
        {
            // Arrange
            var input = new[] { "Inception", "Interstellar" };
            string type = "movie";

            // Act
            var response = await _client.PostAsJsonAsync($"/api/recommendations/{type}", input);

            // Assert
            response.EnsureSuccessStatusCode(); // HTTP 200
            var content = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrWhiteSpace(content)); // Ответ не пустой
        }
    }
}
