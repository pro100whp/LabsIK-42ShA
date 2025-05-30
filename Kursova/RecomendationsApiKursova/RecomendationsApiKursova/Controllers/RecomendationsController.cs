using System.Numerics;
using System.Text;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecomendationsApiKursova.Controllers
{
    
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        [HttpPost]
        [Route("api/recommendations/{type}")]
        public string FindRecommendations([FromBody] string[] value, string type)
        {
            return MoviesAndBooks.GetRecommendations(value.ToList(), type);

        }
        //http://13.53.190.164:5000/api/book/recommendations
        [HttpGet]
        [Route("api/getrecommendationsbook/{telegram_id}")]
        public string GetRecommendationsBooksByTelegramId(long telegram_id)
        {

            return MoviesAndBooks.GetRecommendations(MoviesAndBooks.GetMoviesOrBooks(telegram_id, "book"), "book");
        }

        [HttpGet]
        [Route("api/summary/{type}/{bookOrMovie}/{telegram_id}")]
        public string GetSummarry(long telegram_id, string bookOrMovie, string type)
        {

            return MoviesAndBooks.GetRecommendationSummary(bookOrMovie, type);
        }

        [HttpGet]
        [Route("api/getrecommendationsmovie/{telegram_id}")]
        public string GetRecommendationsMoviesByTelegramId(long telegram_id)
        {

            return MoviesAndBooks.GetRecommendations(MoviesAndBooks.GetMoviesOrBooks(telegram_id, "movie"), "movie");
        }

        //http://13.53.190.164:5000/api/getrecommendationsbook/together/123456789
        [HttpGet]
        [Route("api/getrecommendations{type}/together/{telegram_id}")]
        public string GetRecommendationsBooksTogetherByTelegramId(long telegram_id, string type)
        {

            return MoviesAndBooks.GetRecommendationsTogether(MoviesAndBooks.GetMoviesOrBooks(telegram_id, "movie"), MoviesAndBooks.GetMoviesOrBooks(telegram_id, "book"), $"{type}");
        }


        // GET: api/<RecomendationsController>
        [HttpGet]
        [Route("api/book/get/{telegram_id}")]
        public IEnumerable<string> GetBooks(long telegram_id)
        {
            //return new string[] { "Harry Potter 2", "Harry Poter 3" };
            return MoviesAndBooks.GetMoviesOrBooks(telegram_id, "book");
        }

        // GET: api/<RecomendationsController>
        [HttpGet]
        [Route("api/movie/get/{telegram_id}")]
        public IEnumerable<string> GetMovies(long telegram_id)
        {
            //return new string[] { "The Matrix", "Spider Man" };
            return MoviesAndBooks.GetMoviesOrBooks(telegram_id, "movie");
        }

        [HttpGet]
        [Route("api/history/get/{telegram_id}")]
        public IEnumerable<string> GetHistory(long telegram_id)
        {
            //return new string[] { "The Matrix", "Spider Man" };
            return MoviesAndBooks.GetRecentBooksAndMovies(telegram_id);
        }

        [HttpGet]
        [Route("api/stats/get/")]
        public string GetStats()
        {
            //return new string[] { "The Matrix", "Spider Man" };
            return MoviesAndBooks.GetTopUsers();
        }

        // GET: api/<RecomendationsController>
        [HttpGet]
        [Route("api/{type}/withrating/get/{telegram_id}")]
        public IEnumerable<string> GetBooksOrMoviesAndRating(long telegram_id, string type)
        {
            //return new string[] { "Harry Potter 2", "Harry Poter 3" };
            return MoviesAndBooks.GetMoviesOrBooksSortedByRating(telegram_id, type);
        }

        [HttpGet]
        [Route("api/{type}/isfavorite/get/{telegram_id}")]
        public IEnumerable<string> GetBooksOrMoviesIsFavorite(long telegram_id, string type)
        {

            return MoviesAndBooks.GetFavouriteMoviesOrBooks(telegram_id, type);
        }





        // POST: http://localhost:5000/api/books/add/123456789
        [HttpPost]
        [Route("api/book/add/{title}/{rating}/{is_favorite}/{telegram_id}")]
        public void AddBook(long telegram_id, string title, int rating, bool is_favorite)
        {
            MoviesAndBooks.AddMovieOrBook(telegram_id, title, "book", rating, is_favorite);
        }


        // POST: http://localhost:5000/api/books/add/123456789
        [HttpPost]
        [Route("api/movie/add/{title}/{rating}/{is_favorite}/{telegram_id}")]
        public void AddMovie(long telegram_id, string title, int rating, bool is_favorite)
        {
            MoviesAndBooks.AddMovieOrBook(telegram_id, title, "movie", rating, is_favorite);
        }

        //// GET api/<RecomendationsController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}




        //// PUT api/<RecomendationsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}



        // DELETE api/<RecomendationsController>/5
        [HttpDelete("{id}")]
        [Route("api/{type}/delete/{title}/{telegram_id}")]
        public void Delete(long telegram_id, string title,string type )
        {
            MoviesAndBooks.DeleteMovieOrBook(telegram_id, title, type);
        }

    }
}
