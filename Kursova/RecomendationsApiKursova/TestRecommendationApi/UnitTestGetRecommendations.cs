using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using RecomendationsApiKursova.BusinessLogic;

namespace TestRecommendationApi
{
    public class MoviesAndBooksBLTests
    {
        [Fact]
        public void GetRecommendations_ReturnsNonEmptyString_ForMovies()
        {
            // Arrange
            var bl = new MoviesAndBooksBL();
            var input = new List<string> { "Inception", "Interstellar" };
            var type = "movie";

            // Act
            var result = bl.GetRecommendations(input, type);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        
        [Fact]
        public void GetRecommendationsTogether_ReturnsNonEmptyRecommendation()
        {
            // Arrange
            var bl = new MoviesAndBooksBL(); // или через моку — см. ниже
            var movies = new List<string> { "Inception", "Matrix" };
            var books = new List<string> { "Dune", "1984" };

            // Act
            var result = bl.GetRecommendationsTogether(movies, books, "movie");

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(result));
        }


        [Fact]
        public void GetRecommendationSummary_ReturnsSummaryForKnownTitle()
        {
            // Arrange
            var bl = new MoviesAndBooksBL();

            var title = "Dune";
            var type = "book";

            // Act
            var summary = bl.GetRecommendationSummary(title, type);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(summary));
            Assert.Contains("Dune", summary); // зависит от реализации
        }

    }
}
