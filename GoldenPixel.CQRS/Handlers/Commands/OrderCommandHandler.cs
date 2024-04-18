using GoldenPixel.Core.Orders;
using GoldenPixel.Db;
using LinqToDB;

namespace GoldenPixel.CQRS.Handlers.Commands;

internal class OrderCommandHandler
{

	public async Task<CreateOrderResponse> HandleInsertCommand(GpDbConnection connection, 
		CreateOrderCommand request)
	{
		var id = Guid.NewGuid();
		var order = new Order
		{
			Id = id,
			Email = request.Email,
			Requester = request.Requester,
			Description = request.Description
		};

		var result = await connection.InsertAsync(order);

		if (result == 0)
			return new(null, Errors.FailedInsert);

		return new(id, null);
	}
}
