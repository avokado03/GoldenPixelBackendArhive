using GoldenPixel.Core.Orders;
using GoldenPixel.CQRS.Handlers.Commands;

namespace GoldenPixel.UnitTests.Orders;

[TestFixture]
public class OrderCommandsTests
{
    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task HandlerInsertCommand_NullConnection_ThrowExANE()
    {
        var command = new CreateOrderCommand
        {
            Email = "email",
            Description = "description",
            Requester = "requester"
        };

        await OrderCommandHandler.HandleInsertCommand(null, command);
    }
}