using DataModels;
using GoldenPixel.Core.Orders;
using GoldenPixel.CQRS.Handlers.Mappings;
using LinqToDB;

namespace GoldenPixel.CQRS.Handlers.Queries;

internal class OrderQueryHandler
{
	public async Task<GetOrderByIdResponse> HandleGetOrderById(GetOrderByIdRequest request)
	{
		using (var db = new GPDB())
		{
			try
			{
				var result = await db.Orders.SingleAsync(x => x.Id ==  request.Id);
				return new(result.ToDomain());
			}
			catch (ArgumentNullException)
			{
				return new(null, Errors.BadOrderIdError);
			}
			catch (InvalidOperationException)
			{
				return new(null, Errors.ManyRecordsByOrderId);
			}
		}
	}

	public async Task<GetOrdersResponse> HandleGetOrders(GetOrdersRequest request)
	{
		using (var db = new GPDB())
		{
			try
			{
				var query = from order in db.Orders
							 select order;
				if(request.count != null)
				{
					query = query.Take(request.count.Value);
				}
				var orders = await query.ToArrayAsync();
				var result = orders.ToDomain();
				return new(result, null);
			}
			catch (ArgumentNullException)
			{
				return new(null, Errors.OrderIsNull);
			}
		}
	}
}
