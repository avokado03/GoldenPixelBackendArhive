using GoldenPixel.Db.Entities;
using LinqToDB;
using LinqToDB.Data;

namespace GoldenPixel.Db;

public class GpDbConnection : DataConnection
{
	public ITable<Orders> Orders { get { return this.GetTable<Orders>(); } }

    public GpDbConnection()
    {
        
    }

    public GpDbConnection(DataOptions<GpDbConnection> options) : base(options.Options)
	{

	}
}