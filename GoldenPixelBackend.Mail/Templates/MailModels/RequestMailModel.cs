using System.ComponentModel.DataAnnotations;

namespace GoldenPixelBackend.Mail.Templates.MailModels;

/// <summary>
/// Модель для письма клиенту
/// </summary>
public class RequestMailModel
{
    [Required]
    public string RequesterName { get; set; }
}
