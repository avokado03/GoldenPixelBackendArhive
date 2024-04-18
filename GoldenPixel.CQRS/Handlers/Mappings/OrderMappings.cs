using GoldenPixel.Db.Entities;
using Domain = GoldenPixel.Core.Orders;

namespace GoldenPixel.CQRS.Handlers.Mappings;

internal static class OrderMappings
{
	public static Domain.Order ToDomain(this Order dbOrder)
	{
		return new Domain.Order(
			Id: dbOrder.Id, Email: dbOrder.Email,
			Description: dbOrder.Description, Requester: dbOrder.Requester
		);
	}

	public static Domain.Order[] ToDomain(this Order[] dbOrders)
	{
		var domains = new Domain.Order[dbOrders.Length];

		for(int i = 0; i < dbOrders.Length; i++)
		{
			domains[i] = dbOrders[i].ToDomain();
		}

		return domains;
	}
}
