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
        
        
        //public static string ConnectionString = "Host=database-2.c5ug4y6mymme.eu-north-1.rds.amazonaws.com;Port=5432;Username=postgres;Password=4BewwwCNk#KVH_t;Database=postgres";
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
                model = "command",  
                prompt = prompt,
                max_tokens = 500,
                temperature = 0.8,
                k = 0,
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
