namespace GoldenPixel.Core.Orders;

#region Query+response GetOrderById
public readonly record struct GetOrderByIdQuery(Guid Id);
public readonly record struct GetOrderByIdResponse(Orders? Order, OrderError? Error = null);
#endregion

#region Query+response GetOrders
public readonly record struct GetOrdersQuery(int? count); // TODO: с фильтрами непонятно пока
public readonly record struct GetOrdersResponse(Orders[]? Orders, OrderError? Error = null);
#endregion
