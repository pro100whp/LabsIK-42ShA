using System.Numerics;
using System.Text;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecomendationsApiKursova.Controllers
{
    
    [ApiController]
    public class RecomendationsController : ControllerBase
    {
        [HttpPost]
        [Route("api/[controller]")]
        public string Post([FromBody] string[] value)
        {
           
            var apiKey = "WnisWZdQpLlt1xcNftFfo4Y8UktxfeHGPF1Qgg9D";
            var client = new CohereClient(apiKey);
            string DelimitedList = string.Join(", ", value.Select(v => $"\"{v}\""));


            string input = $"I like movies {DelimitedList}.Please recommend me something similar, 3 movies.Give me answer in form of structured json with \"title\", \"description\"(description is short ,clear and max 25 words), \"YearOfProduction\" without aditional text DO NOT WRITE Here are some movies similar to..., DO NOT WRITE ANYTHING ELSE BUT JSON";
            
            string response = client.GetResponseAsync(input).GetAwaiter().GetResult();
            
           
            return response;
        }

        // GET: api/<RecomendationsController>
        [HttpGet]
        [Route("api/books/get/{telegram_id}")]
        public IEnumerable<string> GetBooks(long telegram_id)
        {
            //return new string[] { "Harry Potter 2", "Harry Poter 3" };
            return MoviesAndBooks.GetMoviesOrBooks(telegram_id, "book");
        }

        // GET: api/<RecomendationsController>
        [HttpGet]
        [Route("api/movies/get/{telegram_id}")]
        public IEnumerable<string> GetMovies(long telegram_id)
        {
            //return new string[] { "The Matrix", "Spider Man" };
            return MoviesAndBooks.GetMoviesOrBooks(telegram_id, "movie");
        }

        // POST: http://localhost:5000/api/books/add/123456789
        [HttpPost]
        [Route("api/books/add/{telegram_id}")]
        public void AddBook(long telegram_id, [FromBody] string title)
        {
            MoviesAndBooks.AddMovieOrBook(telegram_id, title, "book", 0, false);
        }


        // POST: http://localhost:5000/api/books/add/123456789
        [HttpPost]
        [Route("api/movies/add/{telegram_id}")]
        public void AddMovie(long telegram_id, [FromBody] string title)
        {
            MoviesAndBooks.AddMovieOrBook(telegram_id, title, "movie", 0, false);
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



        //// DELETE api/<RecomendationsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

    }
}
