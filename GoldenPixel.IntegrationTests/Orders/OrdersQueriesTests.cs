using GoldenPixel.Core.Orders;
using GoldenPixel.CQRS.Handlers.Queries;
using LinqToDB.Data;

namespace GoldenPixel.IntegrationTests.Orders;

/// <summary>
/// Интеграционные тесты для <see cref="OrderQueryHandler"/>
/// </summary>
public class OrdersQueriesTests : TestBase
{
    private Db.Entities.Orders[] _testOrders = new Db.Entities.Orders[2];

    [SetUp]
    protected void Init() 
    {
        _testOrders = GetTestsOrders();
        InsertTestOrders(_testOrders);
    }

    [Test]
    public async Task HandleGetOrderById_ReturnOrder()
    {
        var id = _testOrders[0].Id;
        var response = new GetOrderByIdResponse();

        using (var connection = CreateDBConnection())
        {
            response = await OrderQueryHandler.HandleGetOrderById(connection,
                new GetOrderByIdQuery(id));
        }

        Assert.That(response.Error, Is.Null);
        Assert.That(response.Order.Value.Id, Is.EqualTo(id));
    }

    [Test]
    [TestCase(null)]
    [TestCase(2)]
    public async Task HandleGetOrders_ReturnOrders(int? count)
    {
        var ids = _testOrders.Select(x => x.Id)
            .Take(count.HasValue ? count.Value : _testOrders.Length)
            .ToArray();
        var response = new GetOrdersResponse();

        using (var connection = CreateDBConnection())
        {
            response = await OrderQueryHandler.HandleGetOrders(connection,
                new GetOrdersQuery(count));
        }

        var resultIds = response.Orders.Select(x => x.Id).ToArray();

        Assert.That(response.Error, Is.Null);
        Assert.That(response.Orders.Length, Is.EqualTo(
            count.HasValue ? count.Value : ids.Length
        ));
        for (int i = 0; i < ids.Count(); i++)
        {
            Assert.That(ids[i], Is.EqualTo(resultIds[i]));
        }

    }

    private Db.Entities.Orders[] GetTestsOrders()
    {
        Db.Entities.Orders[] orders =
        [
            new() { Id = Guid.NewGuid(), Description = "desc1", Email = "email1", Requester = "req1", Date = DateTime.Now },
            new() { Id = Guid.NewGuid(), Description = "desc1", Email = "email1", Requester = "req1", Date = DateTime.Now },
            new() { Id = Guid.NewGuid(), Description = "desc1", Email = "email1", Requester = "req1", Date = DateTime.Now },
        ];
        return orders;
    }


    private void InsertTestOrders(Db.Entities.Orders[] orders)
    {
        using (var connection = CreateDBConnection())
        {
            connection.BulkCopy(orders);
        }
    }
}
