using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;

namespace TestRecommendationApi
{
    public class RecommendationsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly long _testTelegramId = 555555555;
        private readonly string _testTitle = "Test Book Integration";

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

        [Fact]
        public async Task FullBookLifecycle_WorksCorrectly()
        {
            // 1. Add book
            var addResponse = await _client.PostAsync(
                $"/api/book/add/{_testTitle}/5/true/{_testTelegramId}",
                null
            );
            addResponse.EnsureSuccessStatusCode();

            // 2. Get all books and check if the test book exists
            var allBooks = await _client.GetFromJsonAsync<string[]>($"/api/book/get/{_testTelegramId}");
            Assert.Contains(_testTitle, allBooks);

            // 3. Get favorite books and check
            var favorites = await _client.GetFromJsonAsync<string[]>($"/api/book/isfavorite/get/{_testTelegramId}");
            Assert.Contains(_testTitle, favorites);

            // 4. Get books sorted by rating
            var ratedBooks = await _client.GetFromJsonAsync<string[]>($"/api/book/withrating/get/{_testTelegramId}");
            Assert.Contains(ratedBooks, b => b.Contains($"{_testTitle} — Рейтинг: 5"));

            // 5. Delete the book
            var deleteResponse = await _client.DeleteAsync($"/api/book/delete/{_testTitle}/{_testTelegramId}");
            deleteResponse.EnsureSuccessStatusCode();

            // 6. Ensure book is deleted
            var updatedBooks = await _client.GetFromJsonAsync<string[]>($"/api/book/get/{_testTelegramId}");
            Assert.DoesNotContain(_testTitle, updatedBooks);
        }
    }
}
