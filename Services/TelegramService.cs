using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Portafolio.Services
{
    public class TelegramService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public TelegramService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendNotificationAsync(string clientName, string clientEmail, string messageBody)
        {
            var botToken = _configuration["TelegramSettings:BotToken"];
            var chatId = _configuration["TelegramSettings:ChatId"];

            var url = $"https://api.telegram.org/bot{botToken}/sendMessage";

            // Formateamos el mensaje para que te llegue con los datos completos del contacto
            var text = $"📩 *Nuevo mensaje del Portafolio*\n\n" +
                       $"👤 *Nombre:* {clientName}\n" +
                       $"📧 *Email:* {clientEmail}\n" +
                       $"💬 *Mensaje:* {messageBody}";

            var payload = new
            {
                chat_id = chatId,
                text = text,
                parse_mode = "Markdown" // Permite usar negritas y formato de texto
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                var response = await _httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                // Registra el error en consola/logs para no detener el flujo principal de la API
                Console.WriteLine($"Error al enviar notificación de Telegram: {ex.Message}");
            }
        }
    }
}