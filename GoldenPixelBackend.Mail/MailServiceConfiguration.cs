using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentEmail.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentEmail.MailKitSmtp;

namespace GoldenPixelBackend.Mail;
/// <summary>
/// Добавление почтового сервисв в DI-контейнер
/// и конфигурация
/// </summary>
public static class MailServiceConfiguration
{
    public static void ConfigureMailKitService(this IServiceCollection services, IConfiguration configuration)
    {
        var mailServiceConfig = configuration.GetSection("Mail").Get<MailConfiguration>();
        var options = new SmtpClientOptions
        {
            Server = mailServiceConfig.Server,
            Port = Convert.ToInt32(mailServiceConfig.Port),
            Password = mailServiceConfig.Password,
            UseSsl = true,
            User = mailServiceConfig.User,
            RequiresAuthentication = true,
            SocketOptions = MailKit.Security.SecureSocketOptions.Auto
        };
        services.AddSingleton<ISender>(x => new MailKitSender(options));

        services
            .AddFluentEmail(mailServiceConfig.From, mailServiceConfig.FromName ?? "")
            .AddMailKitSender(options)
            .AddLiquidRenderer();
        services.AddSingleton<MailService>();
    }
}
