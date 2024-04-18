namespace GoldenPixel.Db.Entities;

public class Order
{
	public Guid Id { get; set; }
	public string Email { get; set; }
	public string Requester { get; set; }
	public string Description { get; set; }
}

