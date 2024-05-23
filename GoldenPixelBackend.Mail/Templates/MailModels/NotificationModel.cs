using System.ComponentModel.DataAnnotations;

namespace GoldenPixelBackend.Mail.Templates.MailModels;

/// <summary>
/// Модель оповещения о новой заявке
/// </summary>
public class NotificationModel : RequestMailModel
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string Date { get; set; }
}
