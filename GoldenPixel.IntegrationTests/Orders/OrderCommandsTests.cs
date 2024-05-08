using GoldenPixel.Core.Orders;
using GoldenPixel.CQRS.Handlers.Commands;

namespace GoldenPixel.IntegrationTests.Orders;

/// <summary>
/// Интеграционные тесты для <see cref="OrderCommandHandler"/>
/// </summary>
public class OrderCommandsTests : TestBase
{
    [Test]
    public async Task HandleInsertCommand_ReturnResponseWithoutError()
    {
        var result = new CreateOrderResponse();
        using (var connection = CreateDBConnection())
        {
            result = await OrderCommandHandler.HandleInsertCommand(connection,
            new CreateOrderCommand("test@ya.ru", "test", "test"));
        }
        Assert.That(result.Error, Is.Null);
        Assert.That(result.Id, Is.Not.Null);
    }
}
