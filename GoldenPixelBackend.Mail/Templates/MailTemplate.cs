namespace GoldenPixelBackend.Mail.Templates;
/// <summary>
/// Шаблон сообщения
/// </summary>
public class MailTemplate<T> where T : class
{
    /// <summary>
    /// Тема
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Тело
    /// </summary>
    public string BodyPath { get; set; }

    /// <summary>
    /// Модель шаблона
    /// </summary>
    public T TemplateModel { get; set; }
}
