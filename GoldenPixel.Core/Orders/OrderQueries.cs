namespace GoldenPixel.Core.Orders;

public readonly record struct GetOrderByIdRequest(Guid Id);
public readonly record struct GetOrderByIdResponse(Order Order, OrderError? Error);

//public readonly record struct GetOrdersRequest(); // TODO: с фильтрами непонятно пока
public readonly record struct GetOrdersResponse(Order[] Orders, OrderError? Error);

