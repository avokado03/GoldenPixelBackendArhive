using GoldenPixel.Core.Orders;
using GoldenPixel.CQRS.Handlers.Mappings;
using GoldenPixel.CQRS.Handlers.Validation;
using GoldenPixel.Db;
using LinqToDB;

namespace GoldenPixel.CQRS.Handlers.Queries;

public static class OrderQueryHandler
{
    public static async Task<GetOrderByIdResponse> HandleGetOrderById(GpDbConnection connection,
        GetOrderByIdQuery request)
    {
        if (connection == null)
            throw new ArgumentNullException(nameof(connection));

        var validationResult = OrdersValidator.VadidateGetOrderByIdRequest(request);

        if (validationResult.ErrorsCount != 0)
        {
            return new(null, Errors.CreateValidationError(validationResult.ToString()));
        }

        try
        {
            var result = await connection.Orders.SingleAsync(x => x.Id == request.Id);
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
        GetOrdersQuery request)
    {
        if (connection == null)
            throw new ArgumentNullException(nameof(connection));
        try
        {
            var orders = from order in connection.Orders
                         select order;
            if (request.count != null)
            {
                orders = orders.Take(request.count.Value);
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
