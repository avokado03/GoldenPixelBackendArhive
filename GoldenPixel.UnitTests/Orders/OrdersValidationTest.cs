using GoldenPixel.Core.Orders;
using GoldenPixel.CQRS.Handlers.Validation;

namespace GoldenPixel.UnitTests.Orders
{
	[TestFixture]
	internal class OrdersValidationTest
	{
		public static object[] GetOrderByIdCases =
		[
			new object[] { new GetOrderByIdQuery(Guid.Empty), "Идентификатор заявки невалиден."},
		];

		public static object[] CreateOrderCommandCases =
		[
			new object[] 
			{ 
				new CreateOrderCommand(string.Empty, "requester", string.Empty),
				new List<string> { "Почта не указана.", "Укажите описание заявки." }
			},
			new object[]
			{
				new CreateOrderCommand("dkflgjkldjfgkjdf", string.Empty, new string('x', 301)),
				new List<string> { "Почта имеет неверный формат.", "Укажите имя заявителя.", "Длина описания заявки не должна превышать 300 символов." },
			},
		];

		[Test]
		[TestCaseSource(nameof(GetOrderByIdCases))]
		public void ValidateGetOrderByIdRequest(GetOrderByIdQuery request, string errorMessage)
		{
			var result = OrdersValidator.VadidateGetOrderByIdRequest(request);

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ErrorsCount, Is.EqualTo(1));
			Assert.That(result.ErrorDescriptions.Contains(errorMessage));
		}

		[Test]
		[TestCaseSource(nameof(CreateOrderCommandCases))]
		public void ValidateCreateOrderCommand(CreateOrderCommand command, List<string> errorMessages)
		{
			var result = OrdersValidator.ValidateCreateOrderCommand(command);
			Assert.That(result, Is.Not.Null);
			Assert.That(result.ErrorsCount, Is.EqualTo(errorMessages.Count));
			Assert.That(result.ErrorDescriptions, Is.EquivalentTo(errorMessages));
		}
	}
}
