
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using GrammBOTv2.Models;

namespace GrammBOTv2.Controllers{
    public class InlineKeyboardController{

        private readonly ITelegramBotClient _telegramClient;
        public InlineKeyboardController(ITelegramBotClient telegramBotClient){

            _telegramClient = telegramBotClient;

        }
        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct){

            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            Session.OperationType = callbackQuery.Data;

            // Генерим информационное сообщение
            string OperationText = callbackQuery.Data switch{

                "CountSing" => "Количество знаков в строке",
                "SumOfNumbers" => "Сумма чисел",
                _ => string.Empty
            };

            // Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Твой выбор - {OperationText}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Ты можешь всегда изменить свой выбор в главном Меню.", cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}
