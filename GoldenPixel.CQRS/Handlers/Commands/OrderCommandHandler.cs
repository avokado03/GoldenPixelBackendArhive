using GoldenPixel.Core.Orders;
using LinqToDB;

namespace GoldenPixel.CQRS.Handlers.Commands;

internal class OrderCommandHandler
{
	public async Task HandleInsertCommand(CreateOrderCommand request)
	{
		using (var db = new DataModels.GPDB())
		{
			var order = new DataModels.Order
			{
				Id = Guid.NewGuid(),
				Email = request.Email,
				Requester = request.Requester,
				Description = request.Description
			};

			var result = await db.InsertAsync(order);

			if (result == 0)
				throw new LinqToDBException("Ошибка при добавлении записи.");
		}
	}
}

