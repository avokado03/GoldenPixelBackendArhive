namespace GoldenPixel.Core.Orders;

public readonly record struct Order(Guid Id, string Email, string Requester, string Description);

