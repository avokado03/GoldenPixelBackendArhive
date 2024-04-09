namespace GoldenPixel.Core;

public enum CoreErrorCode
{
	UnknownError = 1
}

public readonly record struct CoreError(CoreErrorCode ErrorCode, string ErrorMessage);

public static class Errors
{
	private static readonly string UnknownCoreMessage = "Неизвестная ошибка";

	private static readonly CoreError UnknownError = new(CoreErrorCode.UnknownError, UnknownCoreMessage);
}