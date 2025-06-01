using Npgsql;

namespace RecomendationsApiKursova.DataAccessLayer
{
    public class MoviesAndBooksDAL
    {
        string _connectionString; 
        public MoviesAndBooksDAL() 
        {

            _connectionString = DataBaseSecret.GetConnectionString();


        }
        public List<string> GetMoviesOrBooks(long telegram_id, string type)
        {
            var result = new List<string>();

            using (var connection = new NpgsqlConnection(this._connectionString))
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

        public List<string> GetMoviesOrBooksSortedByRating(long telegram_id, string type)
        {
            var result = new List<string>();

            using (var connection = new NpgsqlConnection(this._connectionString))
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

        public List<string> GetFavouriteMoviesOrBooks(long telegram_id, string type)
        {
            var result = new List<string>();

            using (var connection = new NpgsqlConnection(this._connectionString))
            {
                connection.Open();

                var query = @"
            SELECT title, rating 
            FROM books_and_movies 
            WHERE telegram_id = @telegram_id AND type = @type AND is_favorite = true";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("telegram_id", telegram_id);
                    command.Parameters.AddWithValue("type", type);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var title = reader.GetString(0);

                            result.Add($"{title}");
                        }
                    }
                }
            }

            return result;
        }

        public void AddMovieOrBook(long telegram_id, string title, string type, int rating, bool is_favorite)
        {

            string insertQuery = @"
            INSERT INTO public.books_and_movies (telegram_id, title, type, rating, is_favorite)
            VALUES (@telegram_id, @title, @type, @rating, @is_favorite);
        ";

            try
            {
                using var connection = new NpgsqlConnection(this._connectionString);
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

        public void DeleteMovieOrBook(long telegram_id, string title, string type)
        {
            string deleteQuery = @"
        DELETE FROM public.books_and_movies 
        WHERE telegram_id = @telegram_id AND title = @title AND type = @type;
    ";

            try
            {
                using var connection = new NpgsqlConnection(this._connectionString);
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

        public List<string> GetRecentBooksAndMovies(long telegram_id, int limit = 5)
        {
            var result = new List<string>();

            using (var connection = new NpgsqlConnection(this._connectionString))
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

        public string GetTopUsers()
        {
            var result = "Топ-3 користувачі за кількістю прочитаних книжок:\n";

            using (var connection = new NpgsqlConnection(this._connectionString))
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
}
