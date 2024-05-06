using GoldenPixel.Core.Orders;
using System.Text.RegularExpressions;

namespace GoldenPixel.CQRS.Handlers.Validation
{
	public static class OrdersValidator
	{
		public static ValidationResult VadidateGetOrderByIdRequest(GetOrderByIdQuery request)
		{
			var result = new ValidationResult();
			bool isIdValid = request.Id != Guid.Empty;
			if (!isIdValid)
			{
				result.AddError("Идентификатор заявки невалиден.");
			}
			return result;
		}

		//TODO: реализовать после фильтров
		public static ValidationResult ValidateGetOrdersRequest(GetOrdersQuery request)
		{
			throw new NotImplementedException();
		}

		public static ValidationResult ValidateCreateOrderCommand(CreateOrderCommand command)
		{
			// see https://emailregex.com/
			var emailRegex = new Regex("(?:[a-z0-9!#$%&'*+\\=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+\\=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])");

			var result = new ValidationResult();
			var errorsList = new List<string>();

			if (string.IsNullOrEmpty(command.Email))
			{
				errorsList.Add("Почта не указана.");
			}
			else
			{
				if (!emailRegex.IsMatch(command.Email))
				{
					errorsList.Add("Почта имеет неверный формат.");
				}
			}

			//TODO: длина?
			if (string.IsNullOrWhiteSpace(command.Requester))
			{
				errorsList.Add("Укажите имя заявителя.");
			}

			if (string.IsNullOrEmpty(command.Description))
			{
				errorsList.Add("Укажите описание заявки.");
			}

			if (command.Description.Length > 300)
			{
				errorsList.Add("Длина описания заявки не должна превышать 300 символов.");
			}

			result.AddErrors(errorsList);
			return result;
		}
	}
}
