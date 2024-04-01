using DataModels;
using LinqToDB;

namespace GoldenPixel.CQRS.Handlers.Queries;

internal class OrderQueryHandler
{
	public async Task<Order> HandleOrderById(Guid id)
	{
		using (var db = new GPDB())
		{
			try
			{
				var result = db.Orders.SingleAsync(x => x.Id ==  id);
			}
			catch (ArgumentNullException ex)
			{

			}
		}
	}
}
