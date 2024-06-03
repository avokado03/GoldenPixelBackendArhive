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
        var noReplyMailUserConfig = configuration.GetSection("Mail : NoReplyMailUser").Get<MailUserConfiguration>();
        var requestMailUserConfig = configuration.GetSection("Mail : RequestMailUser").Get<MailUserConfiguration>();
        var options = new SmtpClientOptions
        {
            Server = mailServiceConfig.Server,
            Port = Convert.ToInt32(mailServiceConfig.Port),
            Password = noReplyMailUserConfig.Password,
            UseSsl = true,
            User = noReplyMailUserConfig.User,
            RequiresAuthentication = true,
            SocketOptions = MailKit.Security.SecureSocketOptions.Auto
        };

        services.Configure<EmailOptions>(
            x =>
            {
                x.NoReplyMail = noReplyMailUserConfig.From;
                x.RequestMail = requestMailUserConfig.From;
            }
        );
        services.AddSingleton<ISender>(x => new MailKitSender(options));

        services
            .AddFluentEmail(noReplyMailUserConfig.From, mailServiceConfig.FromName ?? "")
            .AddMailKitSender(options)
            .AddLiquidRenderer();
        services.AddSingleton<MailService>();
    }
}

public class EmailOptions
{
    public string NoReplyMail { get; set; }

    public string RequestMail { get; set; }
}
