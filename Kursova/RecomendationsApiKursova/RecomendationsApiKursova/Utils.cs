using System.Net.Http.Headers;
using System.Text;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Numerics;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;



namespace RecomendationsApiKursova
{
    public class DbSecretModel
    {
        public string host { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string port { get; set; }
        public string database { get; set; }
    }

    public class BookRecommendation
    {
        public string title { get; set; }
        public string description { get; set; }
        public string YearOfProduction { get; set; }
    }

    public class DataBaseSecret
    {
        public static async Task<string> GetSecret()
        {
            string secretName = "DataBaseSecretForRecommendationsApp";
            string region = "eu-north-1";

            IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

            GetSecretValueRequest request = new GetSecretValueRequest
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified.
            };

            GetSecretValueResponse response;

            try
            {
                response = await client.GetSecretValueAsync(request);
            }
            catch (Exception e)
            {
                // For a list of the exceptions thrown, see
                // https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
                throw e;
            }

            string secret = response.SecretString;

            // Your code goes here
            return secret;
        }
        public static string GetConnectionString()
        {
            //Пробуємо взяти данні з SecretManager якщо не виходить беремо ConnectionString з файлу на диску (для дебагу)
            string ConnectionString = string.Empty;
            try 
            {
                string secretManagerResponse = DataBaseSecret.GetSecret().GetAwaiter().GetResult();
                DbSecretModel secretModel = JsonConvert.DeserializeObject<DbSecretModel>(secretManagerResponse);
                ConnectionString = $"Host={secretModel.host};Port={secretModel.port};Username={secretModel.username};Password={secretModel.password};Database={secretModel.database}";
            }
            catch (Exception e)
            {
                ConnectionString = File.ReadAllText(@"C:\Users\wwhhpp\Documents\DatabaseKursova\ConectionString.txt");
                
            }
            return ConnectionString;
        }   

    }

    public class MoviesAndBooks
    {

        public static string GetRecommendationSummary(string books)
        {
            var apiKey = "WnisWZdQpLlt1xcNftFfo4Y8UktxfeHGPF1Qgg9D";
            var client = new CohereClient(apiKey);
            string DelimitedList = string.Join(", ", books.Select(v => $"\"{v}\""));


            string input = $"give me a summary 350 words of the book: {DelimitedList}.";
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































        public static string GetRecommendations(List<string> moviesOrBooks, string type)
        {
            var apiKey = "WnisWZdQpLlt1xcNftFfo4Y8UktxfeHGPF1Qgg9D";
            var client = new CohereClient(apiKey);
            string DelimitedList = string.Join(", ", moviesOrBooks.Select(v => $"\"{v}\""));


            string input = $"I like {type}s {DelimitedList}.Please recommend me something similar, 1 {type}s.Give me answer in form of structured json with \"title\", \"description\"(description is short ,clear and max 25 words), \"YearOfProduction\" without aditional text DO NOT WRITE Here are some movies similar to..., DO NOT WRITE ANYTHING ELSE BUT JSON";
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

        public static string GetRecommendationsTogether(List<string> movies, List<string>  books,string type)
        {
            var apiKey = "WnisWZdQpLlt1xcNftFfo4Y8UktxfeHGPF1Qgg9D";
            var client = new CohereClient(apiKey);
            string DelimitedListMovies = string.Join(", ", movies.Select(v => $"\"{v}\""));
            string DelimitedListBooks = string.Join(", ", movies.Select(v => $"\"{v}\""));

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
        //SELECT title, rating FROM books_and_movies WHERE telegram_id = @telegram_id AND type = @type ORDER BY title
        public static List<string> GetMoviesOrBooks(long telegram_id, string type)
        {
            var result = new List<string>();

            using (var connection = new NpgsqlConnection(DataBaseSecret.GetConnectionString()))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("SELECT title FROM books_and_movies WHERE telegram_id = @telegram_id AND type = @type ORDER BY type", connection))
                {
                    command.Parameters.AddWithValue("telegram_id", telegram_id);
                    command.Parameters.AddWithValue("type", type);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return result;
        }
        public static List<string> GetMoviesOrBooksSortedByRating(long telegram_id, string type)
        {
            var result = new List<string>();

            using (var connection = new NpgsqlConnection(DataBaseSecret.GetConnectionString()))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(
                    "SELECT title, rating FROM books_and_movies WHERE telegram_id = @telegram_id AND type = @type ORDER BY rating DESC NULLS LAST",
                    connection))
                {
                    command.Parameters.AddWithValue("telegram_id", telegram_id);
                    command.Parameters.AddWithValue("type", type);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string title = reader.GetString(0);
                            string ratingText = reader.IsDBNull(1) ? "Рейтинг: немає" : $"Рейтинг: {reader.GetInt32(1)}";

                            result.Add($"{title} — {ratingText}");
                        }
                    }
                }
            }

            return result;
        }

        public static void AddMovieOrBook(long telegram_id, string title, string type, int rating, bool is_favorite)
        {
            
            string insertQuery = @"
            INSERT INTO public.books_and_movies (telegram_id, title, type, rating, is_favorite)
            VALUES (@telegram_id, @title, @type, @rating, @is_favorite);
        ";

            try
            {
                using var connection = new NpgsqlConnection(DataBaseSecret.GetConnectionString());
                connection.Open();

                using var command = new NpgsqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("telegram_id", telegram_id);
                command.Parameters.AddWithValue("title", title);
                command.Parameters.AddWithValue("type", type);
                command.Parameters.AddWithValue("rating", rating);
                command.Parameters.AddWithValue("is_favorite", is_favorite);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($" Успішно додано записів: {rowsAffected}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Помилка: {ex.Message}");
            }
        }

        public static void DeleteMovieOrBook(long telegram_id, string title, string type)
        {
            string deleteQuery = @"
        DELETE FROM public.books_and_movies 
        WHERE telegram_id = @telegram_id AND title = @title AND type = @type;
    ";

            try
            {
                using var connection = new NpgsqlConnection(DataBaseSecret.GetConnectionString());
                connection.Open();

                using var command = new NpgsqlCommand(deleteQuery, connection);
                command.Parameters.AddWithValue("telegram_id", telegram_id);
                command.Parameters.AddWithValue("title", title);
                command.Parameters.AddWithValue("type", type);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"🗑️ Успішно видалено записів: {rowsAffected}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Помилка при видаленні: {ex.Message}");
            }
        }

        public static List<string> GetRecentBooksAndMovies(long telegram_id, int limit = 5)
        {
            var result = new List<string>();

            using (var connection = new NpgsqlConnection(DataBaseSecret.GetConnectionString()))
            {
                connection.Open();

                string query = @"
            SELECT title, type, createdat 
            FROM books_and_movies 
            WHERE telegram_id = @telegram_id 
            ORDER BY createdat DESC 
            LIMIT @limit;
        ";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("telegram_id", telegram_id);
                    command.Parameters.AddWithValue("limit", limit);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string title = reader.GetString(0);
                            string type = reader.GetString(1);
                            DateTime createdAt = reader.GetDateTime(2);

                            result.Add($"{createdAt:yyyy-MM-dd HH:mm} — {type.ToUpper()}: {title}");
                        }
                    }
                }
            }

            return result;
        }

        public static string GetTopUsers()
        {
            var result = "Топ-3 користувачі за кількістю прочитаних книжок:\n";

            using (var connection = new NpgsqlConnection(DataBaseSecret.GetConnectionString()))
            {
                connection.Open();

                // Топ-3 по книжках
                using (var bookCommand = new NpgsqlCommand(@"
            SELECT telegram_id, COUNT(*) AS count
            FROM books_and_movies
            WHERE type = 'book'
            GROUP BY telegram_id
            ORDER BY count DESC
            LIMIT 3;
        ", connection))
                using (var reader = bookCommand.ExecuteReader())
                {
                    int rank = 1;
                    while (reader.Read())
                    {
                        long telegramId = reader.GetInt64(0);
                        int count = reader.GetInt32(1);
                        result += $"{rank}.  {telegramId} —  {count} книжок\n";
                        rank++;
                    }
                }

                result += "\n Топ-3 користувачі за кількістю переглянутих фільмів:\n";

                // Топ-3 по фільмах
                using (var movieCommand = new NpgsqlCommand(@"
            SELECT telegram_id, COUNT(*) AS count
            FROM books_and_movies
            WHERE type = 'movie'
            GROUP BY telegram_id
            ORDER BY count DESC
            LIMIT 3;
        ", connection))
                using (var reader = movieCommand.ExecuteReader())
                {
                    int rank = 1;
                    while (reader.Read())
                    {
                        long telegramId = reader.GetInt64(0);
                        int count = reader.GetInt32(1);
                        result += $"{rank}.  {telegramId} —  {count} фільмів\n";
                        rank++;
                    }
                }
            }

            return result;
        }
    }
    public class CohereClient
    {
        private readonly HttpClient _httpClient;

        public CohereClient(string apiKey)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.cohere.ai/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            _httpClient.DefaultRequestHeaders.Add("Cohere-Version", "2022-12-06");
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            var requestBody = new
            {
                //ДОБАВИТЬ АПИ ПЕРЕВОДЧИКА
                //model = "command",                 
                model = "command",
                prompt = prompt,
                max_tokens = 10000,
                temperature = 0.8,
                k = 50,
                p = 0.75,
                stop_sequences = new string[] { "--" },
                return_likelihoods = "NONE"
            };

            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("generate", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Помилка від API: " + response.StatusCode);
                Console.WriteLine("Деталі: " + responseString);
                return "[Помилка отримання відповіді]";
            }

            try
            {
                var json = JObject.Parse(responseString);
                return json["generations"]?[0]?["text"]?.ToString()?.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка парсингу JSON: " + ex.Message);
                Console.WriteLine("Відповідь API: " + responseString);
                return "[Помилка обробки відповіді]";
            }
        }
    }




}
