namespace GoldenPixel.Core.Orders;

public readonly record struct CreateOrderCommand (string Email, string Requester, string Description);
public readonly record struct CreateOrderResponse (Guid Id, OrderError? Error);
