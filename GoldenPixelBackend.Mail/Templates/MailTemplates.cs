using GoldenPixelBackend.Mail.Templates.MailModels;

namespace GoldenPixelBackend.Mail.Templates;

/// <summary>
/// Шаблоны для конкретных писем
/// </summary>
public static class MailTemplates
{
    /// <summary>
    /// Уведомление заявителя о принятии заявки
    /// </summary>
    public static Func<string, MailTemplate<RequestMailModel>> GetRequestTemplate = requesterName =>
        new MailTemplate<RequestMailModel>()
        {
            BodyPath = "RequestEmail.liquid",
            Subject = "Подтверждение заявки - Golden Pixel",
            TemplateModel = new RequestMailModel { RequesterName = requesterName }
        };

    /// <summary>
    /// Уведомление компании о новой заявке
    /// </summary>
    public static Func<Guid, string, string, string, MailTemplate<NotificationModel>> GetNotificationTemplate =
        (id, email, description, requesterName) =>
            new MailTemplate<NotificationModel>()
            {
                BodyPath = "NotificationEmail.liquid",
                Subject = $"Поступление новой заявки от {DateTime.Today}.",
                TemplateModel = new NotificationModel
                {
                    RequesterName = requesterName,
                    Description = description,
                    Email = email,
                    Id = id.ToString()
                }
            };
}
