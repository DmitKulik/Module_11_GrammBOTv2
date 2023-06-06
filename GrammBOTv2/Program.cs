using System;
using System.Text;
using System.Threading.Tasks;
using GrammBOTv2.Controllers;
using GrammBOTv2.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace GrammBOTv2{
    public class Program{

        static void ConfigureServices(IServiceCollection services){

            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("6142007089:AAFZt5OVHvN1NSV6r6bgHIOJUOcKbw40Tlo"));
            services.AddHostedService<Bot>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();
            services.AddTransient<MessageHandler>();
            services.AddTransient<DefaultMessageController>();
        }

        public static async Task Main(){

            Console.OutputEncoding = Encoding.Unicode;

            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.WriteLine("Сервис запущен");
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }
    }
}