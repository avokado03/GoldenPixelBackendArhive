namespace GoldenPixelBackend.Mail;
/// <summary>
/// Параметры конфигурации SmtpClientOptions
/// </summary>
public class MailConfiguration
{
    /// <summary>
    /// С какой почты идет отправка
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// Имя отправителя
    /// </summary>
    public string FromName { get; set; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public string User { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// smtp-сервер
    /// </summary>
    public string Server { get; set; }

    /// <summary>
    /// Порт
    /// </summary>
    public string Port { get; set; }
}
