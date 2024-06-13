using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace HelpDesk.Data
{
    public class TelegramService
    {
        private readonly string _botToken;
        private readonly string _chatId;
        private readonly TelegramBotClient _botClient;
        private readonly ILogger<TelegramService> _logger;

        public TelegramService(string botToken, string chatId, ILogger<TelegramService> logger)
        {
            _botToken = botToken;
            _chatId = chatId;
            _botClient = new TelegramBotClient(_botToken);
            _logger = logger;
        }

        public async Task SendMessageAsync(string message)
        {
            try
            {
                _logger.LogInformation("Attempting to send message to Telegram...");
                await _botClient.SendTextMessageAsync(_chatId, message);
                _logger.LogInformation("Message sent to Telegram successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send message to Telegram: {ex.Message}");
            }
        }
    }
}
