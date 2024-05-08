namespace GoldenPixel.Core.Orders;

/// <summary>
/// Order domain
/// </summary>
public readonly record struct Orders(Guid Id, string Email, string Requester, string Description, DateTime Date);
