using FluentEmail.Core;
using FluentEmail.Core.Models;
using GoldenPixelBackend.Mail.Templates;
using GoldenPixelBackend.Mail.Templates.MailModels;
using Microsoft.Extensions.Options;
using System.Net;

namespace GoldenPixelBackend.Mail;

/// <summary>
/// E-mail сервис
/// </summary>
public class MailService
{
    private readonly IFluentEmailFactory _fluentEmailFactory;

    public string DefaultTo { get; private set; }

    public MailService(IFluentEmailFactory fluentEmailFactory, IOptions<EmailOptions> options)
    {
        _fluentEmailFactory = fluentEmailFactory;
        DefaultTo = options.Value.DefaultTo ?? throw new ArgumentNullException(nameof(options.Value));
        ServicePointManager.ServerCertificateValidationCallback =
            (sender, certificate, chain, sslPolicyErrors) => true;
    }

    /// <summary>
    /// Отправить письмо
    /// </summary>
    public async Task<SendResponse> SendAsync<T>(MailModel<T> model)
        where T : class
    {
        var sendResponse = await BuildMail(model).SendAsync();
        return sendResponse;
    }

    private IFluentEmail BuildMail<T>(MailModel<T> model)
     where T : class => _fluentEmailFactory
                            .Create()
                            .To(model.To)
                            .Subject(model.Template.Subject)
                            .Header("List-unsubscribe", $"<{string.Empty}>")
                            .Header("Date", $"<{DateTime.Now}>")
                            .UsingTemplateFromFile(PathHelpers.GetTemplatePath(model.Template.BodyPath),
                                model.Template.TemplateModel);
}
