using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HelpDesk.Data
{
    public class EmailSender
    {
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly TelegramService telegramService;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration Configuration, IHttpContextAccessor HttpContextAccessor, TelegramService telegramService, ILogger<EmailSender> logger)
        {
            configuration = Configuration;
            httpContextAccessor = HttpContextAccessor;
            this.telegramService = telegramService;
            _logger = logger;
        }

        public async Task SendEmail(string EmailType, string EmailAddress, string TicketGuid)
        {
            try
            {
                // Отправка уведомления в Telegram
                string telegramMessage = $"New {EmailType}:\n{GetHelpDeskTicketUrl(TicketGuid)}";
                _logger.LogInformation("Sending message to Telegram...");
                await telegramService.SendMessageAsync(telegramMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send Telegram message: {ex.Message}");
            }
        }

        public string GetHelpDeskTicketUrl(string TicketGuid)
        {
            var request = httpContextAccessor.HttpContext.Request;
            var host = request.Host.ToUriComponent();
            var pathBase = request.PathBase.ToUriComponent();
            return $@"{request.Scheme}://{host}{pathBase}/emailticketedit/{TicketGuid}";
        }
    }
}
