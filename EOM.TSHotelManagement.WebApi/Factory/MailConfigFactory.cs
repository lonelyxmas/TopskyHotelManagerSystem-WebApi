using EOM.TSHotelManagement.Shared;
using Microsoft.Extensions.Configuration;

namespace EOM.TSHotelManagement.WebApi
{
    public class MailConfigFactory : IMailConfigFactory
    {
        private readonly IConfiguration _configuration;

        public MailConfigFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MailConfig GetMailConfig()
        {
            var mailConfig = new MailConfig
            {
                Host = _configuration.GetSection("Mail").GetValue<string>("Host"),
                Port = _configuration.GetSection("Mail").GetValue<int>("Port"),
                UserName = _configuration.GetSection("Mail").GetValue<string>("UserName"),
                Password = _configuration.GetSection("Mail").GetValue<string>("Password"),
                EnableSsl = _configuration.GetSection("Mail").GetValue<bool>("EnableSsl"),
                DisplayName = _configuration.GetSection("Mail").GetValue<string>("DisplayName")
            };
            return mailConfig;
        }
    }
}
