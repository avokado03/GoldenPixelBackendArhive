using FluentEmail.Core.Interfaces;
using FluentEmail.MailKitSmtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        services.Configure<EmailOptions>(x => x.DefaultTo = mailServiceConfig.From);
        services.AddSingleton<ISender>(x => new MailKitSender(options));

        services
            .AddFluentEmail(mailServiceConfig.From, mailServiceConfig.FromName ?? "")
            .AddMailKitSender(options)
            .AddLiquidRenderer();
        services.AddSingleton<MailService>();
    }
}

public class EmailOptions
{
    public string DefaultTo { get; set; }
}
