using GoldenPixel.Core.Orders;
using GoldenPixel.CQRS.Handlers.Commands;
using GoldenPixel.TestInfrastructure.Attributes;
using GoldenPixelBackend.Mail;

namespace GoldenPixel.UnitTests.Orders;

/// <summary>
/// Unit-тесты для <see cref="OrderCommandHandler"/>
/// </summary>
[TestFixture]
public class OrdersCommandsTests
{
    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task HandlerInsertCommand_NullConnection_ThrowExANE()
    {
        var mailMock = new Mock<IMailService>();
        var command = new CreateOrderCommand
        {
            Email = "email",
            Description = "description",
            Requester = "requester"
        };

        await OrderCommandHandler.HandleInsertCommand(null, command, mailMock.Object);
    }
}
