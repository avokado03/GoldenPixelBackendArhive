using GoldenPixel.Core.Orders;
using LinqToDB;

namespace GoldenPixel.CQRS.Handlers.Commands;

internal class OrderCommandHandler
{
	public async Task<CreateOrderResponse> HandleInsertCommand(CreateOrderCommand request)
	{
		using (var db = new DataModels.GPDB())
		{
			var id = Guid.NewGuid();
			var order = new DataModels.Order
			{
				Id = id,
				Email = request.Email,
				Requester = request.Requester,
				Description = request.Description
			};

			var result = await db.InsertAsync(order);

			if (result == 0)
				return new(null, Errors.FailedInsert);

			return new(id, null);
		}
	}
}

