namespace GoldenPixel.Core.Orders;

public readonly record struct Orders(Guid Id, string Email, string Requester, string Description);

