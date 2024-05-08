using GoldenPixel.Core.Orders;
using GoldenPixel.CQRS.Handlers.Commands;
using GoldenPixel.CQRS.Handlers.Queries;
using GoldenPixel.Db;
using Microsoft.AspNetCore.Mvc;

namespace GoldenPixelBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private GpDbConnection _connection;
    private ILogger<OrdersController> _logger;

    public OrdersController(GpDbConnection connection, ILogger<OrdersController> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOrderByIdResponse))]
    [ProducesErrorResponseType(typeof(BadRequestResult))]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogInformation($"Get order with id = {id}");
        var result = await OrderQueryHandler.HandleGetOrderById(_connection, new(id));
        return result.Error.HasValue ? BadRequest(result) : Ok(result);
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOrdersResponse))]
    [ProducesErrorResponseType(typeof(BadRequestResult))]
    public async Task<IActionResult> Get(GetOrdersQuery query)
    {
        _logger.LogInformation($"Get orders");
        var result = await OrderQueryHandler.HandleGetOrders(_connection, query);
        return result.Error.HasValue ? BadRequest(result) : Ok(result);
    }

    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateOrderResponse))]
    [ProducesErrorResponseType(typeof(BadRequestResult))]
    public async Task<IActionResult> Create(CreateOrderCommand command)
    {
        var result = await OrderCommandHandler.HandleInsertCommand(_connection, command);
        var isError = result.Error.HasValue;
        _logger.LogInformation(isError ? $"Create order with error: {result.Error}" : $"Create order with id {result.Id}");
        return isError ? BadRequest(result) : Ok(result);
    }
}
