using GoldenPixel.Core.Orders;
using GoldenPixel.CQRS.Handlers.Validation;
using GoldenPixel.Db;
using LinqToDB;

namespace GoldenPixel.CQRS.Handlers.Commands;

public static class OrderCommandHandler
{

	public static async Task<CreateOrderResponse> HandleInsertCommand(GpDbConnection connection, 
		CreateOrderCommand command)
	{
		if (connection is null)
			throw new ArgumentNullException(nameof(connection));

		var validationResult = OrdersValidator.ValidateCreateOrderCommand(command);

		if (validationResult.ErrorsCount != 0)
		{
			return new(null, Errors.CreateValidationError(validationResult.ToString()));
		}

		var id = Guid.NewGuid();
		var order = new Orders
		{
			Id = id,
			Email = command.Email,
			Requester = command.Requester,
			Description = command.Description,
			Date = DateTime.Now
		};

		var result = await connection.InsertAsync(order);

		if (result == 0)
			return new(null, Errors.FailedInsert);

		return new(id, null);
	}
}
