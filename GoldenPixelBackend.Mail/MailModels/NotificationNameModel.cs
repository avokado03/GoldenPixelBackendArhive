using System.ComponentModel.DataAnnotations;

namespace GoldenPixelBackend.Mail.MailModels;

/// <summary>
/// Модель оповещения о новой заявке
/// </summary>
internal class NotificationNameModel : RequestMailModel
{
	[Required]
	public long Id { get; set; }

	[Required]
	public string Email { get; set; }

	public string? Description { get; set; }
}

