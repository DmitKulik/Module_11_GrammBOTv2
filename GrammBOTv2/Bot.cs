using Microsoft.Extensions.Hosting;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Polling;
using GrammBOTv2.Services;
using GrammBOTv2.Controllers;
using GrammBOTv2.Models;

namespace GrammBOTv2{
    public class Bot : BackgroundService{
        // Клиент к Telegram Bot API
        private ITelegramBotClient _telegramClient;// создаем экземпляр клиента Telegram Bot API

        // Контроллеры различных видов сообщений
        private TextMessageController _textMessageController;
        private InlineKeyboardController _inlineKeyboardController;
        private DefaultMessageController _defaultMessageController;
        public MessageHandler _messageHandler;
        public Bot(ITelegramBotClient telegramClient, 
            TextMessageController textMessageController, 
            InlineKeyboardController inlineKeyboardController, 
            MessageHandler messageHandler,
            DefaultMessageController defaultMessageController){

            _textMessageController = textMessageController;
            _inlineKeyboardController = inlineKeyboardController;
            _defaultMessageController = defaultMessageController;
            _messageHandler = messageHandler;
            _telegramClient = telegramClient;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken){

            _telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } }, // Здесь выбираем, какие обновления хотим получать. В данном случае разрешены все
                cancellationToken: stoppingToken);

            Console.WriteLine("Бот запущен");

        }
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken){

            //  Обрабатываем нажатия на кнопки  из Telegram Bot API: 
            if (update.Type == UpdateType.CallbackQuery){

                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }

            if (update.Type == UpdateType.Message){

                if (Session.OperationType == null){

                    await _textMessageController.Handle(update.Message, cancellationToken);
                    return;
                }
                else{
                    if (update.Message.Text == "/start"){
                        await _textMessageController.Handle(update.Message, cancellationToken);
                        return;
                    }
                    else{
                        string message = _messageHandler.Handle(Session.OperationType, update.Message.Text);
                        await _telegramClient.SendTextMessageAsync(update.Message.Chat.Id, message, cancellationToken: cancellationToken);
                    }
                    object value = default; await _defaultMessageController.Handle(update.Message, cancellationToken);
                    return;
                }
            }
        }
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken){

            // Задаем сообщение об ошибке в зависимости от того, какая именно ошибка произошла
            var errorMessage = exception switch{

                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            // Выводим в консоль информацию об ошибке
            Console.WriteLine(errorMessage);

            // Задержка перед повторным подключением
            Console.WriteLine("Ожидаем 5 секунд перед повторным подключением.");
            Thread.Sleep(5000);

            return Task.CompletedTask;
        }
    }
}