namespace GoldenPixelBackend.Mail.Templates.MailModels;
/// <summary>
/// Модель письма
/// </summary>
public class MailModel<T> where T : class
{
    /// <summary>
    /// От кого
    /// </summary>
    public string? From { get; set; } = null;

    /// <summary>
    /// Кому
    /// </summary>
    public string To { get; set; }

    /// <summary>
    /// Шаблон письма
    /// </summary>
    public MailTemplate<T> Template { get; set; }
}
