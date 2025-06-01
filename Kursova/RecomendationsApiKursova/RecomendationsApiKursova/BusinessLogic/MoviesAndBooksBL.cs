using Newtonsoft.Json;
using Npgsql;
using RecomendationsApiKursova.DataAccessLayer;

namespace RecomendationsApiKursova.BusinessLogic
{
    public class MoviesAndBooksBL
    {
        MoviesAndBooksDAL _moviesAndBooksDAL = new MoviesAndBooksDAL();
        
        public string GetRecommendations(List<string> moviesOrBooks, string type)
        {
            var apiKey = "WnisWZdQpLlt1xcNftFfo4Y8UktxfeHGPF1Qgg9D";
            var client = new CohereClient(apiKey);
            string DelimitedList = string.Join(", ", moviesOrBooks.Select(v => $"\"{v}\""));


            string input = $"I like {type}s {DelimitedList}.Please recommend me something similar, 3 {type}s.Give me answer in form of structured json with \"title\", \"description\"(description is short ,clear and max 25 words), \"YearOfProduction\" without aditional text DO NOT WRITE Here are some movies similar to..., DO NOT WRITE ANYTHING ELSE BUT JSON";
            //string input = $"ДАЙ ВІДПОВІДЬ УКРАЇНСЬКОЮ МОВОЮ!Я люблю {type} {DelimitedList}. Порекомендуй мені 3 схожих {type}. Вивід тільки українською. Для кожного пункту: назва жирним (**), рік у дужках, короткий опис (до 25 слів). Без вступів і завершень. Без пояснень. Просто список у заданому форматі. Між пунктами — порожній рядок.";


            string response = client.GetResponseAsync(input).GetAwaiter().GetResult();

            int start = response.IndexOf('[');
            int end = response.LastIndexOf(']');

            if (start >= 0 && end > start)
            {

                string jsonOnly = response.Substring(start, end - start + 1);

                try
                {


                    var books = JsonConvert.DeserializeObject<List<BookRecommendation>>(jsonOnly);

                    response = string.Join("\n\n", books.Select(b =>
                            $"{b.title} ({b.YearOfProduction}): {b.description}"
                            ));

                }
                catch (JsonException ex)
                {
                    Console.WriteLine("Не вдалося розпарсити JSON: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("JSON не знайдений у відповіді API.");
            }


            return response;
        }

        public string GetRecommendationsTogether(List<string> movie, List<string> book, string type)
        {
            var apiKey = "WnisWZdQpLlt1xcNftFfo4Y8UktxfeHGPF1Qgg9D";
            var client = new CohereClient(apiKey);
            string DelimitedListMovies = string.Join(", ", movie.Select(v => $"\"{v}\""));
            string DelimitedListBooks = string.Join(", ", book.Select(v => $"\"{v}\""));

            string input = $"I like such books{DelimitedListBooks} and such films {DelimitedListMovies}.Please recommend me something similar, 3 {type}s.Give me answer in form of structured json with \"title\", \"description\"(description is short ,clear and max 25 words), \"YearOfProduction\" without aditional text DO NOT WRITE Here are some movies similar to..., DO NOT WRITE ANYTHING ELSE BUT JSON";
            //string input = $"ДАЙ ВІДПОВІДЬ УКРАЇНСЬКОЮ МОВОЮ!Я люблю {type} {DelimitedList}. Порекомендуй мені 3 схожих {type}. Вивід тільки українською. Для кожного пункту: назва жирним (**), рік у дужках, короткий опис (до 25 слів). Без вступів і завершень. Без пояснень. Просто список у заданому форматі. Між пунктами — порожній рядок.";


            string response = client.GetResponseAsync(input).GetAwaiter().GetResult();

            int start = response.IndexOf('[');
            int end = response.LastIndexOf(']');

            if (start >= 0 && end > start)
            {

                string jsonOnly = response.Substring(start, end - start + 1);

                try
                {


                    var booksOrFilms = JsonConvert.DeserializeObject<List<BookRecommendation>>(jsonOnly);

                    response = string.Join("\n\n", booksOrFilms.Select(b =>
                            $"{b.title} ({b.YearOfProduction}): {b.description}"
                            ));

                }
                catch (JsonException ex)
                {
                    Console.WriteLine("Не вдалося розпарсити JSON: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("JSON не знайдений у відповіді API.");
            }


            return response;
        }

        public string GetRecommendationSummary(string bookOrMovie, string type)
        {
            var apiKey = "WnisWZdQpLlt1xcNftFfo4Y8UktxfeHGPF1Qgg9D";
            var client = new CohereClient(apiKey);
            string DelimitedList = string.Join(", ", bookOrMovie.Select(v => $"\"{v}\""));


            string input = $"give me a summary 350 words of the   {type}: {DelimitedList}.";
            //string input = $"ДАЙ ВІДПОВІДЬ УКРАЇНСЬКОЮ МОВОЮ!Я люблю {type} {DelimitedList}. Порекомендуй мені 3 схожих {type}. Вивід тільки українською. Для кожного пункту: назва жирним (**), рік у дужках, короткий опис (до 25 слів). Без вступів і завершень. Без пояснень. Просто список у заданому форматі. Між пунктами — порожній рядок.";


            string response = client.GetResponseAsync(input).GetAwaiter().GetResult();

            int start = response.IndexOf('[');
            int end = response.LastIndexOf(']');

            if (start >= 0 && end > start)
            {

                string jsonOnly = response.Substring(start, end - start + 1);

                try
                {


                    var booksі = JsonConvert.DeserializeObject<List<BookRecommendation>>(jsonOnly);

                    response = string.Join("\n\n", booksі.Select(b =>
                            $"{b.title} ({b.YearOfProduction}): {b.description}"
                            ));

                }
                catch (JsonException ex)
                {
                    Console.WriteLine("Не вдалося розпарсити JSON: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("JSON не знайдений у відповіді API.");
            }


            return response;
        }


        public List<string> GetMoviesOrBooks(long telegram_id, string type)
        {

            return _moviesAndBooksDAL.GetMoviesOrBooks( telegram_id,  type);
        }

        public List<string> GetMoviesOrBooksSortedByRating(long telegram_id, string type)
        {           
            return _moviesAndBooksDAL.GetMoviesOrBooksSortedByRating(telegram_id, type);
        }

        public List<string> GetFavouriteMoviesOrBooks(long telegram_id, string type)
        {            
            return _moviesAndBooksDAL.GetFavouriteMoviesOrBooks(telegram_id, type);
        }

        public void AddMovieOrBook(long telegram_id, string title, string type, int rating, bool is_favorite)
        {
           _moviesAndBooksDAL.AddMovieOrBook(telegram_id, title, type, rating, is_favorite);
        }

        public void DeleteMovieOrBook(long telegram_id, string title, string type)
        {
            _moviesAndBooksDAL.DeleteMovieOrBook(telegram_id, title, type);
        }

        public List<string> GetRecentBooksAndMovies(long telegram_id, int limit = 5)
        {
            return _moviesAndBooksDAL.GetRecentBooksAndMovies(telegram_id, limit);
        }

        public string GetTopUsers()
        {
            return _moviesAndBooksDAL.GetTopUsers();
        }
    }
}
