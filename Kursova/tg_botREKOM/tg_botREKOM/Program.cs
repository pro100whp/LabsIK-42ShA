using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;


using static Program;

class Program
{
    static readonly HttpClient httpClient = new HttpClient();
    static readonly ConcurrentDictionary<long, Draft> userTempData = new();
    static readonly ConcurrentDictionary<long, string> userStates = new();


    static void Main(string[] args)
    {
        var client = new TelegramBotClient("7776464703:AAFbqKJEOEbR63Z2krGY5krY171jjNYBkEA");
        client.StartReceiving(Update, Error);
        Console.ReadLine();
    }

    static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        var message = update.Message;
        if (message?.Text == null)

            return;

        long chatId = message.Chat.Id;
        string text = message.Text.ToLower();

        Console.WriteLine($"{message.Chat.Username}    {message.Text}");
        #region states
        //                                        СОСТОЯНИЯ
        #region waiting_for_similar_book
        if (userStates.TryGetValue(chatId, out var state) && state == "waiting_for_similar_book")
        {
            if (text == "відмінити")
            {

                userStates.TryRemove(chatId, out _);
                userTempData.TryRemove(chatId, out _);


                var mainMenu = new ReplyKeyboardMarkup(new[]
                {
                    new[] { new KeyboardButton("🔎 Порекомендувати"), new KeyboardButton("📚 Моє переглянуте") },
                    new[] { new KeyboardButton("⭐ Рейтинг"),         new KeyboardButton("💖 Улюблене")      },
                        })
                { ResizeKeyboard = true };



                return;
            }
            else
            {
                userStates[chatId] = "";
                var mainMenu = new ReplyKeyboardMarkup(new[]
                {
                    new[] { new KeyboardButton("🔎 Порекомендувати"), new KeyboardButton("📚 Моє переглянуте") },
                    new[] { new KeyboardButton("⭐ Рейтинг"),         new KeyboardButton("💖 Улюблене")      },
                        })
                { ResizeKeyboard = true };
                var thinkingMessage = await botClient.SendMessage(
                    chatId,
                    "💭 Думаю...",
                        cancellationToken: token);

                var json = System.Text.Json.JsonSerializer.Serialize(new[] { message.Text });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    var response = await httpClient.PostAsync("http://13.53.190.164:5000/api/recommendations/book", content);
                    response.EnsureSuccessStatusCode();

                    var responseText = await response.Content.ReadAsStringAsync();

                    await botClient.DeleteMessage(chatId, thinkingMessage.MessageId, cancellationToken: token);

                    await botClient.SendMessage(chatId, $"📚 Рекомендації:\n{responseText}",
                    replyMarkup: mainMenu,
                    cancellationToken: token
                );
                }
                catch (Exception ex)
                {
                    await botClient.SendMessage(chatId, $"⚠️ Помилка: {ex.Message}");
                }
            }
        }
        #endregion

        #region waiting_for_similar_movie
        if (userStates.TryGetValue(chatId, out var state1) && state1 == "waiting_for_similar_movie")
        {
            if (text == "відмінити")
            {

                userStates.TryRemove(chatId, out _);
                userTempData.TryRemove(chatId, out _);


                var mainMenu = new ReplyKeyboardMarkup(new[]
                {
                    new[] { new KeyboardButton("🔎 Порекомендувати"), new KeyboardButton("📚 Моє переглянуте") },
                    new[] { new KeyboardButton("⭐ Рейтинг"),         new KeyboardButton("💖 Улюблене")      },
                        })
                { ResizeKeyboard = true };

                await botClient.SendMessage(
                    chatId,
                    "❌ Скасовано. Повертаємось у головне меню.",
                    replyMarkup: mainMenu,
                    cancellationToken: token
                );
                return;
            }
            else
            {
                userStates[chatId] = "";

                var json = System.Text.Json.JsonSerializer.Serialize(new[] { message.Text });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var mainMenu = new ReplyKeyboardMarkup(new[]
                {
                    new[] { new KeyboardButton("🔎 Порекомендувати"), new KeyboardButton("📚 Моє переглянуте") },
                    new[] { new KeyboardButton("⭐ Рейтинг"),         new KeyboardButton("💖 Улюблене")      },
                        })
                { ResizeKeyboard = true };
                var thinkingMessage = await botClient.SendMessage(
    chatId,
    "💭 Думаю...",
        cancellationToken: token);

                try
                {
                    var response = await httpClient.PostAsync("http://13.53.190.164:5000/api/recommendations/movie", content);
                    response.EnsureSuccessStatusCode();

                    var responseText = await response.Content.ReadAsStringAsync();


                    await botClient.DeleteMessage(chatId, thinkingMessage.MessageId, cancellationToken: token);

                    await botClient.SendMessage(chatId, $"📚 Рекомендації:\n{responseText}",
                    replyMarkup: mainMenu,
                    cancellationToken: token
                );
                }
                catch (Exception ex)
                {
                    await botClient.SendMessage(chatId, $"⚠️ Помилка: {ex.Message}");
                }
            }
        }
        #endregion

        #region waiting_for_title
        if (userStates.TryGetValue(chatId, out var state2) && state2 == "waiting_for_title")
        {
            if (text == "відмінити")
            {

                userStates.TryRemove(chatId, out _);
                userTempData.TryRemove(chatId, out _);


                var mainMenu = new ReplyKeyboardMarkup(new[]
                {
                    new[] { new KeyboardButton("🔎 Порекомендувати"), new KeyboardButton("📚 Моє переглянуте") },
                    new[] { new KeyboardButton("⭐ Рейтинг"),         new KeyboardButton("💖 Улюблене")      },
                        })
                { ResizeKeyboard = true };

                await botClient.SendMessage(
                    chatId,
                    "❌ Скасовано. Повертаємось у головне меню.",
                    replyMarkup: mainMenu,
                    cancellationToken: token
                );
                return;
            }
            else
            {

                userStates[chatId] = "waiting_for_rating";
                if (userTempData.TryGetValue(chatId, out var draft))
                {
                    draft.title = message.Text;

                }

                var keyboard = new ReplyKeyboardMarkup(new[]
                {
                new[] { new KeyboardButton("1"), new KeyboardButton("2"), new KeyboardButton("3") },
                new[] { new KeyboardButton("4"), new KeyboardButton("5") }
                })
                { ResizeKeyboard = true };
                if (draft.Type == "book")
                {
                    await botClient.SendMessage(chatId, "📊 Вкажи рейтинг книжки (1–5):", replyMarkup: keyboard);
                }
                else if (draft.Type == "movie")
                {
                    await botClient.SendMessage(chatId, "📊 Вкажи рейтинг фільма (1–5):", replyMarkup: keyboard);

                }

                return;
            }
        }
        #endregion

        #region waiting_for_rating
        if (userStates.TryGetValue(chatId, out state) && state == "waiting_for_rating")
        {
            if (text == "відмінити")
            {

                userStates.TryRemove(chatId, out _);
                userTempData.TryRemove(chatId, out _);


                var mainMenu = new ReplyKeyboardMarkup(new[]
                {
                    new[] { new KeyboardButton("🔎 Порекомендувати"), new KeyboardButton("📚 Моє переглянуте") },
                    new[] { new KeyboardButton("⭐ Рейтинг"),         new KeyboardButton("💖 Улюблене")      },
                        })
                { ResizeKeyboard = true };

                await botClient.SendMessage(
                    chatId,
                    "❌ Скасовано. Повертаємось у головне меню.",
                    replyMarkup: mainMenu,
                    cancellationToken: token
                );
                return;
            }
            else
            {
                if (int.TryParse(message.Text, out int rating) && rating >= 1 && rating <= 5)
                {
                    userStates[chatId] = "waiting_for_favorite";

                    if (userTempData.TryGetValue(chatId, out var Draft))
                    {
                        Draft.rating = rating;
                        var keyboard = new ReplyKeyboardMarkup(new[]
                        {new[]
                    {
                        new KeyboardButton("Так"),
                        new KeyboardButton("Ні") }
                    })

                        { ResizeKeyboard = true };
                        if (Draft.Type == "book")
                        {
                            await botClient.SendMessage(chatId, "💖 Чи є ця книжка улюбленою?", replyMarkup: keyboard);
                        }
                        else if (Draft.Type == "movie")
                        {
                            await botClient.SendMessage(chatId, "💖 Чи є цей фільм улюбленим?", replyMarkup: keyboard);

                        }

                    }
                }
                else
                {
                    await botClient.SendMessage(chatId, "⚠️ Введи число від 1 до 5.");
                }
            }
        }
        #endregion

        #region waiting_for_favorite
        if (userStates.TryGetValue(chatId, out state) && state == "waiting_for_favorite")
        {
            if (text == "відмінити")
            {

                userStates.TryRemove(chatId, out _);
                userTempData.TryRemove(chatId, out _);


                var mainMenu = new ReplyKeyboardMarkup(new[]
                {
                    new[] { new KeyboardButton("🔎 Порекомендувати"), new KeyboardButton("📚 Моє переглянуте") },
                    new[] { new KeyboardButton("⭐ Рейтинг"),         new KeyboardButton("💖 Улюблене")      },
                        })
                { ResizeKeyboard = true };

                await botClient.SendMessage(
                    chatId,
                    "❌ Скасовано. Повертаємось у головне меню.",
                    replyMarkup: mainMenu,
                    cancellationToken: token
                );
                return;
            }
            else
            {
                if (userTempData.TryGetValue(chatId, out var Draft))
                {
                    if (text == "так")
                    {
                        Draft.is_favorite = true;
                    }
                    else if (text == "ні")
                    {
                        Draft.is_favorite = false;
                    }
                    else
                    {
                        await botClient.SendMessage(chatId, "⚠️ Напиши 'Так' або 'Ні'.");
                        return;
                    }

                    userStates[chatId] = "";

                    var keyboard = new ReplyKeyboardMarkup(new[]
                    {

                    new[] { new KeyboardButton("🔎 Порекомендувати") },
                    new[] { new KeyboardButton("📚 Моє переглянуте") },
                    new[] { new KeyboardButton("⭐ Рейтинг") },
                    new[] { new KeyboardButton("💖 Улюблене") }
                })
                    {
                        ResizeKeyboard = true
                    };

                    var json = JsonSerializer.Serialize(new { title = Draft.title });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var userId = message.From.Id;
                    var url = "";
                    if (Draft.Type == "book")
                    {
                        url = $"http://13.53.190.164:5000/api/book/add/{Draft.title}/{Draft.rating}/{Draft.is_favorite}/{userId}";
                    }
                    else if (Draft.Type == "movie")
                    {
                        url = $"http://13.53.190.164:5000/api/movie/add/{Draft.title}/{Draft.rating}/{Draft.is_favorite}/{userId}";

                    }


                    try
                    {
                        var response = await httpClient.PostAsync(url, content);
                        response.EnsureSuccessStatusCode();



                        if (Draft.Type == "book")
                        {
                            await botClient.SendMessage(chatId, "✅ Книжку успішно додано!", replyMarkup: keyboard,
                cancellationToken: token);
                        }
                        else if (Draft.Type == "movie")
                        {
                            await botClient.SendMessage(chatId, "✅ Фільм успішно додано!", replyMarkup: keyboard,
                cancellationToken: token);

                        }
                    }
                    catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
                    {
                        await botClient.SendMessage(chatId, $"⚠️ Такої книжки не існує", replyMarkup: keyboard,
                cancellationToken: token);
                    }
                    catch (Exception ex)
                    {
                        await botClient.SendMessage(chatId, $"⚠️ Помилка під час додавання книжки: {ex.Message}", replyMarkup: keyboard,
                cancellationToken: token);
                    }


                    userTempData.TryRemove(chatId, out _);
                }
            }
        }
        #endregion

        #region waiting_for_delete
        if (userStates.TryGetValue(chatId, out string state5) && state5 == "waiting_for_delete")
        {
            if (text == "відмінити")
            {

                userStates.TryRemove(chatId, out _);
                userTempData.TryRemove(chatId, out _);


                var mainMenu = new ReplyKeyboardMarkup(new[]
                {
                    new[] { new KeyboardButton("🔎 Порекомендувати"), new KeyboardButton("📚 Моє переглянуте") },
                    new[] { new KeyboardButton("⭐ Рейтинг"),         new KeyboardButton("💖 Улюблене")      },
                        })
                { ResizeKeyboard = true };

                await botClient.SendMessage(
                    chatId,
                    "❌ Скасовано. Повертаємось у головне меню.",
                    replyMarkup: mainMenu,
                    cancellationToken: token
                );
                return;
            }
            else
            {
                userStates[chatId] = "";
                if (int.TryParse(message.Text, out int number)
                    && userTempData.TryGetValue(chatId, out Draft draft)
                    && draft.booksOrMooviesList != null
                    && number >= 1 && number <= draft.booksOrMooviesList.Count)


                {
                    var keyboard = new ReplyKeyboardMarkup(new[]
                   {

                    new[] { new KeyboardButton("🔎 Порекомендувати") },
                    new[] { new KeyboardButton("📚 Моє переглянуте") },
                    new[] { new KeyboardButton("⭐ Рейтинг") },
                    new[] { new KeyboardButton("💖 Улюблене") }
                })
                    {
                        ResizeKeyboard = true
                    };

                    var selectedLine = draft.booksOrMooviesList[number - 1];
                    var title = selectedLine.Substring(selectedLine.IndexOf('.') + 1).Trim();
                    title = title.Trim('"');
                    var userId = message.From.Id;
                    Console.WriteLine(title);

                    try
                    {
                        var url = $"http://13.53.190.164:5000/api/{draft.Type}/delete/{title}/{userId}";
                        var response = await httpClient.DeleteAsync(url);
                        response.EnsureSuccessStatusCode();
                        if (draft.Type == "book")
                        {
                            await botClient.SendMessage(chatId, $"✅ Книжку \"{title}\" успішно видалено.", replyMarkup: keyboard,
                cancellationToken: token);
                        }
                        else if (draft.Type == "movie")
                        {
                            await botClient.SendMessage(chatId, $"✅ Фільм \"{title}\" успішно видалено.", replyMarkup: keyboard,
                cancellationToken: token);

                        }

                    }
                    catch (Exception ex)
                    {
                        await botClient.SendMessage(chatId, $"⚠️ Помилка під час видалення книжки: {ex.Message}", replyMarkup: keyboard,
                cancellationToken: token);
                    }


                }
                else
                {
                    await botClient.SendMessage(chatId, "❗ Введи коректний номер книжки зі списку.");
                }
            }
        }
        #endregion

        #region waiting_for_summary
        if (userStates.TryGetValue(chatId, out string state6) && state6 == "waiting_for_summary")
        {
            if (text == "відмінити")
            {

                userStates.TryRemove(chatId, out _);
                userTempData.TryRemove(chatId, out _);


                var mainMenu = new ReplyKeyboardMarkup(new[]
                {
                    new[] { new KeyboardButton("🔎 Порекомендувати"), new KeyboardButton("📚 Моє переглянуте") },
                    new[] { new KeyboardButton("⭐ Рейтинг"),         new KeyboardButton("💖 Улюблене")      },
                        })
                { ResizeKeyboard = true };

                await botClient.SendMessage(
                    chatId,
                    "❌ Скасовано. Повертаємось у головне меню.",
                    replyMarkup: mainMenu,
                    cancellationToken: token
                );
                return;
            }
            else
            {
                userStates[chatId] = "";
                if (int.TryParse(message.Text, out int number)
                    && userTempData.TryGetValue(chatId, out Draft draft)
                    && draft.booksOrMooviesList != null
                    && number >= 1 && number <= draft.booksOrMooviesList.Count)


                {
                    var keyboard = new ReplyKeyboardMarkup(new[]
                   {

                    new[] { new KeyboardButton("🔎 Порекомендувати") },
                    new[] { new KeyboardButton("📚 Моє переглянуте") },
                    new[] { new KeyboardButton("⭐ Рейтинг") },
                    new[] { new KeyboardButton("💖 Улюблене") }
                })
                    {
                        ResizeKeyboard = true
                    };
                    await botClient.SendMessage(
                         chatId,
                        "💭 Переказую...",
                        cancellationToken: token
                         );
                    var selectedLine = draft.booksOrMooviesList[number - 1];
                    var title = selectedLine.Substring(selectedLine.IndexOf('.') + 1).Trim();
                    title = title.Trim('"');
                    var userId = message.From.Id;
                    Console.WriteLine(title);

                    try
                    {
                        var url = $"http://13.53.190.164:5000/api/summary/{draft.Type}/{title}/{userId}";
                        var response = await httpClient.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        var summaryText = await response.Content.ReadAsStringAsync();


                        if (draft.Type == "book")
                        {
                            await botClient.SendMessage(chatId, $"📖 Переказ книги:\n\n{summaryText}", replyMarkup: keyboard, cancellationToken: token);
                        }
                        else if (draft.Type == "movie")
                        {
                            await botClient.SendMessage(chatId, $"📖 Переказ фільму:\n\n{summaryText}", replyMarkup: keyboard, cancellationToken: token);

                        }
                    }
                    catch (Exception ex)
                    {
                        await botClient.SendMessage(chatId, $"⚠️ Помилка : {ex.Message}", replyMarkup: keyboard,
                cancellationToken: token);
                    }


                }
                else
                {
                    await botClient.SendMessage(chatId, "❗ Введи коректний номер книжки зі списку.");
                }
            }
        }
        #endregion
        #endregion states

        if (text == "/start")
        {
            userStates[chatId] = ""; // очищаємо стан
            userTempData.TryRemove(chatId, out _); // очищення тимчасових даних
            var keyboard = new ReplyKeyboardMarkup(new[]
                {

                    new[] { new KeyboardButton("🔎 Порекомендувати") },
                    new[] { new KeyboardButton("📚 Моє переглянуте") },
                    new[] { new KeyboardButton("⭐ Рейтинг") },
                    new[] { new KeyboardButton("💖 Улюблене") }

                })
            {
                ResizeKeyboard = true
            };

            await botClient.SendMessage(chatId, "Привіт!Я бот з рекомендації книжок та фільмів", replyMarkup: keyboard,
            cancellationToken: token);

            return;
        }

        if (text == "📌 головне меню")
        {

            var keyboard = new ReplyKeyboardMarkup(new[]
                {

                    new[] { new KeyboardButton("🔎 Порекомендувати") },
                    new[] { new KeyboardButton("📚 Моє переглянуте") },
                    new[] { new KeyboardButton("⭐ Рейтинг") },
                    new[] { new KeyboardButton("💖 Улюблене") }
                })
            {
                ResizeKeyboard = true
            };

            await botClient.SendMessage(chatId, "📌 Головне меню", replyMarkup: keyboard,
            cancellationToken: token);

            return;
        }

        if (text == "🔎 порекомендувати")
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
                {
                    new[] { new KeyboardButton("📌 Головне меню") },
                    new[] { new KeyboardButton("🔎 Порекомендувати книжку на основі мого преглянутого") },
                    new[] { new KeyboardButton("🔎 Порекомендувати фільм на основі мого преглянутого") },
                    new[] { new KeyboardButton("🧠 Знайти схоже") },

                })
            {
                ResizeKeyboard = true
            };
            await botClient.SendMessage(chatId, "Оберіть тип рекомендації", replyMarkup: keyboard,
            cancellationToken: token);
        }






        #region findLike
        if (text == "🧠 знайти схоже")
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
                {
                    new[] { new KeyboardButton("📌 Головне меню") },
                    new[] { new KeyboardButton("📖 Книжка") },
                    new[] { new KeyboardButton("🎥 Фільм") },


                })
            {
                ResizeKeyboard = true
            };
            await botClient.SendMessage(chatId, "🔎🔎🔎", replyMarkup: keyboard,
            cancellationToken: token);
        }

        else if (text == "📖 книжка")
        {
            userStates[chatId] = "waiting_for_similar_book";


            var keyboard = new ReplyKeyboardMarkup(new[]
                {
                    new[] { new KeyboardButton("Відмінити") }


                })
            {
                ResizeKeyboard = true
            };
            await botClient.SendMessage(chatId, "✍️ Введи назву книжки, яка тобі сподобалась:", replyMarkup: keyboard,
            cancellationToken: token);


        }

        else if (text == "🎥 фільм")
        {
            userStates[chatId] = "waiting_for_similar_movie";

            var keyboard = new ReplyKeyboardMarkup(new[]
               {
                    new[] { new KeyboardButton("Відмінити") }


                })
            {
                ResizeKeyboard = true
            };
            await botClient.SendMessage(chatId, "✍️ Введи назву фільма, який тобі сподобався:", replyMarkup: keyboard,
            cancellationToken: token);

        }
        #endregion

        #region recommend
        if (text == "🔎 порекомендувати книжку на основі мого преглянутого")
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
                {
                    new[] { new KeyboardButton("📌 Головне меню") },
                    new[] { new KeyboardButton("📖 Тільки на основі книжок") },

                    new[] { new KeyboardButton("📖🎥 На основі книжок та фільмів") },

                })
            {
                ResizeKeyboard = true
            };
            await botClient.SendMessage(chatId, "🔎🔎🔎", replyMarkup: keyboard,
            cancellationToken: token);
        }

        if (text == "🔎 порекомендувати фільм на основі мого преглянутого")
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
                {
                    new[] { new KeyboardButton("📌 Головне меню") },
                    new[] { new KeyboardButton("🎥 Тільки на основі фільмів") },
                    new[] { new KeyboardButton("🎥📖 На основі фільмів та книжок") },

                })
            {
                ResizeKeyboard = true
            };
            await botClient.SendMessage(chatId, "🔎🔎🔎", replyMarkup: keyboard,
            cancellationToken: token);
        }

        else if (text == "📖 тільки на основі книжок")
        {
            var userId = message.From.Id;
            var keyboard = new ReplyKeyboardMarkup(new[]
                {
                    new[] { new KeyboardButton("📌 Головне меню") },


                })
            {
                ResizeKeyboard = true
            };
            var thinkingMessage = await botClient.SendMessage(
            chatId,
            "💭 Думаю...",
            cancellationToken: token);

            try
            {
                var response = await httpClient.GetAsync($"http://13.53.190.164:5000/api/getrecommendationsbook/{userId}");
                response.EnsureSuccessStatusCode();


                var responseText = await response.Content.ReadAsStringAsync();

                await botClient.DeleteMessage(chatId, thinkingMessage.MessageId, cancellationToken: token);

                await botClient.SendMessage(chatId, $"📚 Рекомендації на основі книжок:\n{responseText}", replyMarkup: keyboard,
            cancellationToken: token);
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(chatId, $"⚠️ Помилка: {ex.Message}");
            }
        }

        else if (text == "🎥 тільки на основі фільмів")
        {
            var userId = message.From.Id;
            var keyboard = new ReplyKeyboardMarkup(new[]
               {
                    new[] { new KeyboardButton("📌 Головне меню") },


                })
            {
                ResizeKeyboard = true
            };
            var thinkingMessage = await botClient.SendMessage(
            chatId,
            "💭 Думаю...",
         cancellationToken: token);




            try
            {
                var response = await httpClient.GetAsync($"http://13.53.190.164:5000/api/getrecommendationsmovie/{userId}");
                response.EnsureSuccessStatusCode();

                var responseText = await response.Content.ReadAsStringAsync();

                await botClient.DeleteMessage(chatId, thinkingMessage.MessageId, cancellationToken: token);

                await botClient.SendMessage(chatId, $"📚 Рекомендації на основі фільмів:\n{responseText}", replyMarkup: keyboard,
            cancellationToken: token);
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(chatId, $"⚠️ Помилка: {ex.Message}", replyMarkup: keyboard, cancellationToken: token);
            }
        }
        else if (text == "📖🎥 на основі книжок та фільмів")
        {
            var userId = message.From.Id;
            var keyboard = new ReplyKeyboardMarkup(new[]
               {
                    new[] { new KeyboardButton("📌 Головне меню") },


                })
            {
                ResizeKeyboard = true
            };
            var thinkingMessage = await botClient.SendMessage(
            chatId,
            "💭 Думаю...",
        cancellationToken: token);


            try
            {
                var response = await httpClient.GetAsync($"http://13.53.190.164:5000/api/getrecommendationsbook/together/{userId}");
                response.EnsureSuccessStatusCode();

                var responseText = await response.Content.ReadAsStringAsync();

                await botClient.DeleteMessage(chatId, thinkingMessage.MessageId, cancellationToken: token);

                await botClient.SendMessage(chatId, $"📚 Рекомендації на основі книжок та фільмів:\n{responseText}");
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(chatId, $"⚠️ Помилка: {ex.Message}", replyMarkup: keyboard, cancellationToken: token);
            }
        }

        else if (text == "🎥📖 на основі фільмів та книжок")
        {
            var userId = message.From.Id;
            var keyboard = new ReplyKeyboardMarkup(new[]
               {
                    new[] { new KeyboardButton("📌 Головне меню") },


                })
            {
                ResizeKeyboard = true
            };
            var thinkingMessage = await botClient.SendMessage(
            chatId,
            "💭 Думаю...",
          cancellationToken: token);


            try
            {
                var response = await httpClient.GetAsync($"http://13.53.190.164:5000/api/getrecommendationsmovie/together/{userId}");
                response.EnsureSuccessStatusCode();

                var responseText = await response.Content.ReadAsStringAsync();

                await botClient.DeleteMessage(chatId, thinkingMessage.MessageId, cancellationToken: token);

                await botClient.SendMessage(chatId, $"📚 Рекомендації на основі фільмів та книжок:\n{responseText}", replyMarkup: keyboard, cancellationToken: token);
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(chatId, $"⚠️ Помилка: {ex.Message}", replyMarkup: keyboard, cancellationToken: token);
            }
        }
        #endregion
        if (text == "📚 моє переглянуте")
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
               {
                    new[] { new KeyboardButton("📌 Головне меню") },
                    new[] { new KeyboardButton("📖 Книжки") },
                    new[] { new KeyboardButton("🎥 Фільми") },


                })
            {
                ResizeKeyboard = true
            };
            await botClient.SendMessage(chatId, "Оберіть тип ", replyMarkup: keyboard,
            cancellationToken: token);
        }
        #region action with a book
        if (text == "📖 книжки")
        {

            var keyboard = new ReplyKeyboardMarkup(new[]
              {
                    new[] { new KeyboardButton("📌 Головне меню") },
                    new[] { new KeyboardButton("➕ Додати книжку") },
                    new[] { new KeyboardButton("🗑️ Видалити книжку") },
                    new[] { new KeyboardButton("🧠 Отримати пересказ книги") },

                })
            {
                ResizeKeyboard = true
            };

            var userId = message.From.Id;



            try
            {
                var response = await httpClient.GetAsync($"http://13.53.190.164:5000/api/book/get/{userId}");
                response.EnsureSuccessStatusCode();

                var responseText = await response.Content.ReadAsStringAsync();




                List<string> UserBooks = JsonSerializer.Deserialize<List<string>>(responseText);


                var messages = string.Join("\n", UserBooks.Select((books, index) => $"{index + 1}. {books}"));

                await botClient.SendMessage(chatId, $"Ось список ваших переглянутих книжок\n{messages}", replyMarkup: keyboard,
            cancellationToken: token);
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(chatId, $"⚠️ Помилка: {ex.Message}");
            }
        }
        if (text == "➕ додати книжку")
        {

            userStates[chatId] = "waiting_for_title";
            userTempData[chatId] = new Draft { Type = "book" };


            var keyboard = new ReplyKeyboardMarkup(new[]
              {
                    new[] { new KeyboardButton("Відмінити") }


                })
            {
                ResizeKeyboard = true
            };
            await botClient.SendMessage(chatId, "✍️ Введи назву книжки:", replyMarkup: keyboard,
            cancellationToken: token);

            return;
        }

        if (text == "🗑️ видалити книжку")
        {
            userStates[chatId] = "waiting_for_delete";

            var userId = message.From.Id;
            var keyboard = new ReplyKeyboardMarkup(new[]
               {
                    new[] { new KeyboardButton("Відмінити") }


                })
            {
                ResizeKeyboard = true
            };


            try
            {
                var response = await httpClient.GetAsync($"http://13.53.190.164:5000/api/book/get/{userId}");
                response.EnsureSuccessStatusCode();

                var responseText = await response.Content.ReadAsStringAsync();




                List<string> UserBooks = JsonSerializer.Deserialize<List<string>>(responseText);


                List<string> formattedBookList = UserBooks.Select((book, index) => $"{index + 1}. {book}").ToList();

                userTempData[chatId] = new Draft
                {
                    Type = "book",
                    booksOrMooviesList = formattedBookList
                };

                await botClient.SendMessage(chatId, $"Напишіть під яким номером знаходиться книжка яку ви хочете видалити", replyMarkup: keyboard,
            cancellationToken: token);
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(chatId, $"⚠️ Помилка: {ex.Message}");
            }
            return;
        }

        if (text == "🧠 отримати пересказ книги")
        {
            userStates[chatId] = "waiting_for_summary";
            var userId = message.From.Id;
            var keyboard = new ReplyKeyboardMarkup(new[]
              {
                    new[] { new KeyboardButton("Відмінити") }


                })
            {
                ResizeKeyboard = true
            };

            try
            {
                var response = await httpClient.GetAsync($"http://13.53.190.164:5000/api/book/get/{userId}");
                response.EnsureSuccessStatusCode();

                var responseText = await response.Content.ReadAsStringAsync();




                List<string> UserBooks = JsonSerializer.Deserialize<List<string>>(responseText);


                List<string> formattedBookList = UserBooks.Select((book, index) => $"{index + 1}. {book}").ToList();

                userTempData[chatId] = new Draft
                {
                    Type = "book",
                    booksOrMooviesList = formattedBookList
                };

                await botClient.SendMessage(chatId, $"Напишіть під яким номером знаходиться книжка переказ якої ви хочете побачити", replyMarkup: keyboard,
            cancellationToken: token);
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(chatId, $"⚠️ Помилка: {ex.Message}");
            }
            return;
        }
        #endregion

        #region action with a film
        if (text == "🎥 фільми")
        {

            var keyboard = new ReplyKeyboardMarkup(new[]
              {
                    new[] { new KeyboardButton("📌 Головне меню") },
                    new[] { new KeyboardButton("➕ Додати фільм") },
                    new[] { new KeyboardButton("🗑️ Видалити фільм") },
                    new[] { new KeyboardButton("🧠 Отримати пересказ фільма") },
                })
            {
                ResizeKeyboard = true
            };

            var userId = message.From.Id;

            try
            {
                var response = await httpClient.GetAsync($"http://13.53.190.164:5000/api/movie/get/{userId}");
                response.EnsureSuccessStatusCode();

                var responseText = await response.Content.ReadAsStringAsync();

                // Перетворюємо JSON у список
                List<string> UserFilms = JsonSerializer.Deserialize<List<string>>(responseText);

                // Виводимо красиво з нумерацією
                var messages = string.Join("\n", UserFilms.Select((films, index) => $"{index + 1}. {films}"));

                await botClient.SendMessage(chatId, $"Ось список ваших переглянутих фільмів", replyMarkup: keyboard,
            cancellationToken: token);
                //    await botClient.SendMessage(chatId, messages, replyMarkup: keyboard,
                //cancellationToken: token);
                await botClient.SendMessage(
                chatId,
                $"<b>{messages}</b>",
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                replyMarkup: keyboard,
                cancellationToken: token
    );
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(chatId, $"⚠️ Помилка: {ex.Message}");
            }
        }
        if (text == "➕ додати фільм")
        {
            userStates[chatId] = "waiting_for_title";


            userTempData[chatId] = new Draft { Type = "movie" };
            var keyboard = new ReplyKeyboardMarkup(new[]
              {
                    new[] { new KeyboardButton("Відмінити") }


                })
            {
                ResizeKeyboard = true
            };


            await botClient.SendMessage(chatId, "✍️ Введи назву фільму:", replyMarkup: keyboard,
            cancellationToken: token);
            return;
        }

        if (text == "🗑️ видалити фільм")
        {
            userStates[chatId] = "waiting_for_delete";

            var userId = message.From.Id;
            var keyboard = new ReplyKeyboardMarkup(new[]
             {
                    new[] { new KeyboardButton("Відмінити") }


                })
            {
                ResizeKeyboard = true
            };



            try
            {
                var response = await httpClient.GetAsync($"http://13.53.190.164:5000/api/movie/get/{userId}");
                response.EnsureSuccessStatusCode();

                var responseText = await response.Content.ReadAsStringAsync();




                List<string> UserBooks = JsonSerializer.Deserialize<List<string>>(responseText);


                List<string> formattedBookList = UserBooks.Select((book, index) => $"{index + 1}. {book}").ToList();

                userTempData[chatId] = new Draft
                {
                    Type = "movie",
                    booksOrMooviesList = formattedBookList
                };

                await botClient.SendMessage(chatId, $"Напишіть під яким номером знаходиться книжка яку ви хочете видалити", replyMarkup: keyboard,
            cancellationToken: token);
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(chatId, $"⚠️ Помилка: {ex.Message}");
            }
            return;
        }

        if (text == "🧠 отримати пересказ фільма")
        {
            userStates[chatId] = "waiting_for_summary";
            var userId = message.From.Id;
            var keyboard = new ReplyKeyboardMarkup(new[]
             {
                    new[] { new KeyboardButton("Відмінити") }


                })
            {
                ResizeKeyboard = true
            };



            try
            {
                var response = await httpClient.GetAsync($"http://13.53.190.164:5000/api/movie/get/{userId}");
                response.EnsureSuccessStatusCode();

                var responseText = await response.Content.ReadAsStringAsync();




                List<string> UserBooks = JsonSerializer.Deserialize<List<string>>(responseText);


                List<string> formattedBookList = UserBooks.Select((book, index) => $"{index + 1}. {book}").ToList();

                userTempData[chatId] = new Draft
                {
                    Type = "movie",
                    booksOrMooviesList = formattedBookList
                };

                await botClient.SendMessage(chatId, $"Напишіть під яким номером знаходиться книжка переказ якої ви хочете побачити", replyMarkup: keyboard,
            cancellationToken: token);
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(chatId, $"⚠️ Помилка: {ex.Message}");
            }
            return;
        }
        #endregion

        #region rating
        if (text == "⭐ рейтинг")
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
              {
                    new[] { new KeyboardButton("📌 Головне меню") },
                    new[] { new KeyboardButton("⭐📖 Рейтинг книжок") },
                    new[] { new KeyboardButton("⭐🎥 Рейтинг фільмів") },


                })
            {
                ResizeKeyboard = true
            };

            await botClient.SendMessage(chatId, $"Оберіть тип", replyMarkup: keyboard,
            cancellationToken: token);
        }
        if (text == "⭐📖 рейтинг книжок")
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
              {
                    new[] { new KeyboardButton("🔎 Порекомендувати") },
                    new[] { new KeyboardButton("📚 Моє переглянуте") },
                    new[] { new KeyboardButton("⭐ Рейтинг") },
                    new[] { new KeyboardButton("💖 Улюблене") }

                })
            {
                ResizeKeyboard = true
            };

            var userId = message.From.Id;



            try
            {
                var response = await httpClient.GetAsync($"http://13.53.190.164:5000/api/book/withrating/get/{userId}");
                response.EnsureSuccessStatusCode();

                var responseText = await response.Content.ReadAsStringAsync();




                List<string> UserBooks = JsonSerializer.Deserialize<List<string>>(responseText);


                var messages = string.Join("\n", UserBooks.Select((books, index) => $"{index + 1}. {books}"));

                await botClient.SendMessage(chatId, $"Ось Рейтинг ваших переглянутих книжок\n{messages}", replyMarkup: keyboard,
            cancellationToken: token);
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(chatId, $"⚠️ Помилка: {ex.Message}");
            }
        }
        if (text == "⭐🎥 рейтинг фільмів")
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
             {
                    new[] { new KeyboardButton("🔎 Порекомендувати") },
                    new[] { new KeyboardButton("📚 Моє переглянуте") },
                    new[] { new KeyboardButton("⭐ Рейтинг") },
                    new[] { new KeyboardButton("💖 Улюблене") }

                })
            {
                ResizeKeyboard = true
            };

            var userId = message.From.Id;



            try
            {
                var response = await httpClient.GetAsync($"http://13.53.190.164:5000/api/movie/withrating/get/{userId}");
                response.EnsureSuccessStatusCode();

                var responseText = await response.Content.ReadAsStringAsync();




                List<string> UserBooks = JsonSerializer.Deserialize<List<string>>(responseText);


                var messages = string.Join("\n", UserBooks.Select((books, index) => $"{index + 1}. {books} "));

                await botClient.SendMessage(chatId, $"Ось Рейтинг ваших переглянутих фільмів\n{messages}", replyMarkup: keyboard,
            cancellationToken: token);
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(chatId, $"⚠️ Помилка: {ex.Message}");
            }
        }
        #endregion

        #region isFavourite
        if (text == "💖 улюблене")
        {

            var keyboard = new ReplyKeyboardMarkup(new[]
              {
                    new[] { new KeyboardButton("📌 Головне меню") },
                  new[] { new KeyboardButton("📖💖 Улюблені книжки") },
                  new[] { new KeyboardButton("🎥💖 Улюблені фільми") },
                })
            {
                ResizeKeyboard = true
            };

            var userId = message.From.Id;
            await botClient.SendMessage(chatId, $"Виберіть тип", replyMarkup: keyboard,
            cancellationToken: token);
        }
        if (text == "📖💖 улюблені книжки")
        {

            var keyboard = new ReplyKeyboardMarkup(new[]
             {
                    new[] { new KeyboardButton("📌 Головне меню") },

                })
            {
                ResizeKeyboard = true
            };

            var userId = message.From.Id;
            try
            {
                var response = await httpClient.GetAsync($"http://13.53.190.164:5000/api/book/isfavorite/get/{userId}");
                response.EnsureSuccessStatusCode();

                var responseText = await response.Content.ReadAsStringAsync();




                List<string> UserBooks = JsonSerializer.Deserialize<List<string>>(responseText);


                var messages = string.Join("\n", UserBooks.Select((books, index) => $"{index + 1}. {books} 💖"));

                await botClient.SendMessage(chatId, $"Ось список ваших улюблених книжок\n{messages}", replyMarkup: keyboard,
            cancellationToken: token);
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(chatId, $"⚠️ Помилка: {ex.Message}");
            }
        }

        if (text == "🎥💖 улюблені фільми")
        {

            var keyboard = new ReplyKeyboardMarkup(new[]
             {
                    new[] { new KeyboardButton("📌 Головне меню") },

                })
            {
                ResizeKeyboard = true
            };

            var userId = message.From.Id;
            try
            {
                var response = await httpClient.GetAsync($"http://13.53.190.164:5000/api/movie/isfavorite/get/{userId}");
                response.EnsureSuccessStatusCode();

                var responseText = await response.Content.ReadAsStringAsync();




                List<string> UserBooks = JsonSerializer.Deserialize<List<string>>(responseText);


                var messages = string.Join("\n", UserBooks.Select((books, index) => $"{index + 1}. {books} 💖"));

                await botClient.SendMessage(chatId, $"Ось список ваших улюблених фільмів\n{messages}", replyMarkup: keyboard,
            cancellationToken: token);
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(chatId, $"⚠️ Помилка: {ex.Message}");
            }
        }
        #endregion isFavourite

    }



    public class Draft
    {
        public List<string> booksOrMooviesList { get; set; }
        public string Type { get; set; }
        public string title { get; set; }
        public int rating { get; set; }
        public bool is_favorite { get; set; }
    }
    private static async Task Error(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}