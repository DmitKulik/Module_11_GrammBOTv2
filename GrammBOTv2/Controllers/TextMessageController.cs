using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace GrammBOTv2.Controllers{
    public class TextMessageController{

        private readonly ITelegramBotClient _telegramClient;
        public TextMessageController(ITelegramBotClient telegramBotClient){
            _telegramClient = telegramBotClient;
        }
        public async Task Handle(Message message, CancellationToken ct){

            switch (message.Text){
                case "/start":

                    // Объект, представляющий кноки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]{

                        InlineKeyboardButton.WithCallbackData($"Количество знаков в строке" , $"CountSing"),
                        InlineKeyboardButton.WithCallbackData($"Сумма чисел" , $"SumOfNumbers"),

                    });

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Расчет кол-во чисел в сообщении " +
                        $"и сложить числа.</b> {Environment.NewLine}",
                        cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;
                default:
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Выберите действие.", cancellationToken: ct);

                    break;
            }
        }
    }
}
