using FluentEmail.Core.Models;
using GoldenPixelBackend.Mail.Templates.MailModels;

namespace GoldenPixelBackend.Mail;

/// <summary>
/// Контракт для сервиса отправки почты
/// </summary>
public interface IMailService
{
    /// <summary>
    /// Отправить письмо
    /// </summary>
    Task<SendResponse> SendAsync<T>(MailModel<T> model)
        where T : class;
}
