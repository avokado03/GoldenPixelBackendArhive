using GoldenPixel.Core.Orders;
using GoldenPixel.CQRS.Handlers.Mappings;
using GoldenPixel.Db;
using LinqToDB;

namespace GoldenPixel.CQRS.Handlers.Queries;

public static class OrderQueryHandler
{
	public static async Task<GetOrderByIdResponse> HandleGetOrderById(GpDbConnection connection,
		GetOrderByIdRequest query)
	{
		if (connection == null) 
			throw new ArgumentNullException(nameof(connection));

		try
		{
			var result = await connection.Orders.SingleAsync(x => x.Id ==  query.Id);
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

	public static async Task<GetOrdersResponse> HandleGetOrders(GpDbConnection connection,
		GetOrdersRequest query)
	{
		if (connection == null)
			throw new ArgumentNullException(nameof(connection));
		try
		{
			var orders = from order in connection.Orders
						select order;
			if (query.count != null)
			{
				orders = orders.Take(query.count.Value);
			}
			var ordersResult = await orders.ToArrayAsync();
			var result = ordersResult.ToDomain();
			return new(result, null);
		}
		catch (ArgumentNullException)
		{
			return new(null, Errors.OrderIsNull);
		}
	}
}
