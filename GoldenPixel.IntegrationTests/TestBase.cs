using GoldenPixel.Db;
using GoldenPixel.TestInfrastructure.DummyDb;
using LinqToDB;
using LinqToDB.Data;

namespace GoldenPixel.IntegrationTests;

[TestFixture]
public class TestBase
{
    public string? DbConnectionString { get; private set; }

    [SetUp]
    protected void Init()
    {
        DummyDb.CreateDbIfNotExist();
        DbConnectionString = DummyDb.DbConnectionString;
    }

    protected GpDbConnection CreateDBConnection()
    {
        var connection = new GpDbConnection(
            new DataOptions<GpDbConnection>(
                new DataOptions(
                    new ConnectionOptions(
                        ConnectionString: DbConnectionString,
                        ProviderName: "Npgsql")
                    )));
        return connection;
    }

    [TearDown]
    public void Cleanup()
    {
        DummyDb.DropDbIfExist();
        DbConnectionString = null;
    }
}