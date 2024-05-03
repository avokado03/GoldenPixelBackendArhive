using System.Text;

namespace GoldenPixel.CQRS.Handlers.Validation
{
	internal class ValidationResult
	{
		public int ErrorsCount { get; private set; } = 0;
        public List<string> ErrorDescriptions { get; private set; }

		public override string ToString()
		{
			if (ErrorsCount == 0)
			{
				return "Модель валидна.";
			}
			var strBuilder = new StringBuilder();
			foreach (var decr in ErrorDescriptions)
			{
				strBuilder.AppendLine(decr);
			}
			var result = $"Обнаружено {ErrorsCount} ошибок валидации. {strBuilder}";
			return result;
		}

		public void AddError(string message)
		{
			ErrorsCount++;
			ErrorDescriptions.Add(message);
		}

		public void AddErrors(List<string> errorMessages)
		{
			ErrorsCount = errorMessages.Count;
			ErrorDescriptions.AddRange(errorMessages);
		}
    }
}
