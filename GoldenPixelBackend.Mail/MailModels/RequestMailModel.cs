using System.ComponentModel.DataAnnotations;

namespace GoldenPixelBackend.Mail.MailModels;

/// <summary>
/// Модель для письма клиенту
/// </summary>
internal class RequestMailModel
{
	[Required]
	public string ApplicatorName { get; set; }
}
