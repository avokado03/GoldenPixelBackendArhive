using GoldenPixel.Db.Entities;
using Domain = GoldenPixel.Core.Orders;

namespace GoldenPixel.CQRS.Handlers.Mappings;

internal static class OrderMappings
{
	public static Domain.Orders ToDomain(this Orders dbOrder)
	{
		return new Domain.Orders(
			Id: dbOrder.Id, Email: dbOrder.Email,
			Description: dbOrder.Description, Requester: dbOrder.Requester,
			Date: dbOrder.Date
		);
	}

	public static Domain.Orders[] ToDomain(this Orders[] dbOrders)
	{
		var domains = new Domain.Orders[dbOrders.Length];

		for(int i = 0; i < dbOrders.Length; i++)
		{
			domains[i] = dbOrders[i].ToDomain();
		}

		return domains;
	}
}
