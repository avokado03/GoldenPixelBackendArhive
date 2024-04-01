using System.ComponentModel.DataAnnotations;

namespace GoldenPixelBackend.Mail;

/// <summary>
/// Метаданные для SmtpClientOptions
/// </summary>
internal class MailMetadata
{
	/// <summary>
	/// С какой почты идет отправка
	/// </summary>
	[Required]
	public string From { get; set; }

	/// <summary>
	/// Имя отправителя
	/// </summary>
	[Required]
	public string FromName { get; set; }

	/// <summary>
	/// Пользователь
	/// </summary>
	[Required]
	public string User { get; set; }

	/// <summary>
	/// Пароль
	/// </summary>
	[Required]
	public string Password { get; set; }

	/// <summary>
	/// smtp-сервер
	/// </summary>
	[Required]
	public string Server { get; set; }

	/// <summary>
	/// Порт
	/// </summary>
	[Required]
	public string Port { get; set; }
}

