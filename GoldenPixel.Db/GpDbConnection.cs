using GoldenPixel.Db.Entities;
using LinqToDB;
using LinqToDB.Data;

namespace GoldenPixel.Db;

public class GpDbConnection : DataConnection
{
    public ITable<Orders> Orders => this.GetTable<Orders>();

    /// <summary>
    /// ctor for unit tests and DI
    /// </summary>
    public GpDbConnection()
    {

    }

    public GpDbConnection(DataOptions<GpDbConnection> options) : base(options.Options)
    {

    }
}
