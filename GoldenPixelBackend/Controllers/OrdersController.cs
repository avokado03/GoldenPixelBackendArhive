using GoldenPixel.Core.Orders;
using GoldenPixel.CQRS.Handlers.Commands;
using GoldenPixel.CQRS.Handlers.Queries;
using GoldenPixel.Db;
using GoldenPixelBackend.Mail;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GoldenPixelBackend.Controllers;

/// <summary>
/// Контролер заявок
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private GpDbConnection _connection;
    private ILogger<OrdersController> _logger;

    /// <summary>
    /// ctor
    /// </summary>
    public OrdersController(GpDbConnection connection, ILogger<OrdersController> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    /// <summary>
    /// Получить сведения о заявке по ID
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOrderByIdResponse))]
    [ProducesErrorResponseType(typeof(BadRequestResult))]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogInformation($"Get order with id = {id}");
        var result = await OrderQueryHandler.HandleGetOrderById(_connection, new(id));
        var isError = result.Error.HasValue;
        if (isError)
            _logger.LogError($"Can't get order with {id}: {result.Error}");
        return result.Error.HasValue ? BadRequest(result) : Ok(result);
    }

    /// <summary>
    /// Получить список заявок
    /// </summary>
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOrdersResponse))]
    [ProducesErrorResponseType(typeof(BadRequestResult))]
    public async Task<IActionResult> Get(GetOrdersQuery query)
    {
        _logger.LogInformation($"Get orders");
        var result = await OrderQueryHandler.HandleGetOrders(_connection, query);
        var isError = result.Error.HasValue;
        if (isError)
            _logger.LogError($"Can't get orders: {result.Error}");
        return isError ? BadRequest(result) : Ok(result);
    }

    /// <summary>
    /// Добавить новую заявку
    /// </summary>
    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateOrderResponse))]
    [ProducesErrorResponseType(typeof(BadRequestResult))]
    public async Task<IActionResult> Create(CreateOrderCommand command)
    {
        var mailMock = new Mock<IMailService>();
        var result = await OrderCommandHandler.HandleInsertCommand(_connection, command, mailMock.Object);
        var isError = result.Error.HasValue;
        if(isError)
            _logger.LogError($"Create order with error: {result.Error}");
        else
            _logger.LogInformation($"Create order with id {result.Id}");
        return isError ? BadRequest(result) : Ok(result);
    }
}
