using GoldenPixel.Db.Entities;
using LinqToDB;
using LinqToDB.Data;

namespace GoldenPixel.Db;

public class GpDbConnection : DataConnection
{
	public ITable<Order> Orders { get { return this.GetTable<Order>(); } }

	public GpDbConnection(DataOptions<GpDbConnection> options) : base(options.Options)
	{

	}
}

