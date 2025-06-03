using System.Numerics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using RecomendationsApiKursova.BusinessLogic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecomendationsApiKursova.Controllers
{

    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        MoviesAndBooksBL _moviesAndBooksBL = new MoviesAndBooksBL();

        [HttpPost]
        [Route("api/recommendations/{type}")]
        public string FindRecommendations([FromBody] string[] value, string type)
        {
            return _moviesAndBooksBL.GetRecommendations(value.ToList(), type, "default");

        }

        [HttpGet]
        [Route("api/getrecommendationsbook/{telegram_id}")]
        public string GetRecommendationsBooksByTelegramId(long telegram_id)
        {

            return _moviesAndBooksBL.GetRecommendations(_moviesAndBooksBL.GetMoviesOrBooksSortedByRating(telegram_id, "book"), "book", "byRating");
        }

        [HttpGet]
        [Route("api/getrecommendationsmovie/{telegram_id}")]
        public string GetRecommendationsMoviesByTelegramId(long telegram_id)
        {

            return _moviesAndBooksBL.GetRecommendations(_moviesAndBooksBL.GetMoviesOrBooksSortedByRating(telegram_id, "movie"), "movie", "byRating");
        }

        [HttpGet]
        [Route("api/summary/{type}/{bookOrMovie}/{telegram_id}")]
        public string GetSummarry(long telegram_id, string bookOrMovie, string type)
        {

            return _moviesAndBooksBL.GetRecommendationSummary(bookOrMovie, type);
        }

        [HttpGet]
        [Route("api/getrecommendations{type}/together/{telegram_id}")]
        public string GetRecommendationsBooksTogetherByTelegramId(long telegram_id, string type)
        {

            return _moviesAndBooksBL.GetRecommendationsTogether(_moviesAndBooksBL.GetMoviesOrBooksSortedByRating(telegram_id, "movie"), _moviesAndBooksBL.GetMoviesOrBooksSortedByRating(telegram_id, "book"), $"{type}");
        }

        [HttpGet]
        [Route("api/book/get/{telegram_id}")]
        public IEnumerable<string> GetBooks(long telegram_id)
        {
            //return new string[] { "Harry Potter 2", "Harry Poter 3" };
            return _moviesAndBooksBL.GetMoviesOrBooks(telegram_id, "book");
        }

        [HttpGet]
        [Route("api/movie/get/{telegram_id}")]
        public IEnumerable<string> GetMovies(long telegram_id)
        {
            //return new string[] { "The Matrix", "Spider Man" };
            return _moviesAndBooksBL.GetMoviesOrBooks(telegram_id, "movie");
        }

        [HttpGet]
        [Route("api/history/get/{telegram_id}")]
        public IEnumerable<string> GetHistory(long telegram_id)
        {
            //return new string[] { "The Matrix", "Spider Man" };
            return _moviesAndBooksBL.GetRecentBooksAndMovies(telegram_id);
        }

        [HttpGet]
        [Route("api/stats/get/")]
        public string GetStats()
        {
            //return new string[] { "The Matrix", "Spider Man" };
            return _moviesAndBooksBL.GetTopUsers();
        }

        [HttpGet]
        [Route("api/{type}/withrating/get/{telegram_id}")]
        public IEnumerable<string> GetBooksOrMoviesAndRating(long telegram_id, string type)
        {
            //return new string[] { "Harry Potter 2", "Harry Poter 3" };
            return _moviesAndBooksBL.GetMoviesOrBooksSortedByRating(telegram_id, type);
        }

        [HttpGet]
        [Route("api/{type}/isfavorite/get/{telegram_id}")]
        public IEnumerable<string> GetBooksOrMoviesIsFavorite(long telegram_id, string type)
        {
            return _moviesAndBooksBL.GetFavouriteMoviesOrBooks(telegram_id, type);
        }

        [HttpPost]
        [Route("api/book/add/{title}/{rating}/{is_favorite}/{telegram_id}")]
        public void AddBook(long telegram_id, string title, int rating, bool is_favorite)
        {
            _moviesAndBooksBL.AddMovieOrBook(telegram_id, title, "book", rating, is_favorite);
        }

        [HttpPost]
        [Route("api/movie/add/{title}/{rating}/{is_favorite}/{telegram_id}")]
        public void AddMovie(long telegram_id, string title, int rating, bool is_favorite)
        {
            _moviesAndBooksBL.AddMovieOrBook(telegram_id, title, "movie", rating, is_favorite);
        }

        [HttpDelete("{id}")]
        [Route("api/{type}/delete/{title}/{telegram_id}")]
        public void Delete(long telegram_id, string title, string type)
        {
            _moviesAndBooksBL.DeleteMovieOrBook(telegram_id, title, type);
        }

    }
}
