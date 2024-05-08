namespace GoldenPixel.Core.Orders;

public enum OrdersErrorCode
{
    BadOrderId = 1,
    ManyRecordsByOrderId = 2,
    OrdersIsNull = 3,
    FailedInsert = 4,
    FailedValidation = 5
}
public readonly record struct OrderError(OrdersErrorCode ErrorCode, string ErrorMessage);

public static class Errors
{
    private static readonly string BadOrderIdMessage = "Записи о запросе с данным Id не существует";
    private static readonly string ManyRecordsByIdMessage = "По введенному Id существует несколько записей о запросах";
    private static readonly string OrdersIsNullMessage = "Выборка заказов равна null";
    private static readonly string FailedInsertMessage = "Запрос не был добавлен";

    public static readonly OrderError BadOrderIdError = new(OrdersErrorCode.BadOrderId, BadOrderIdMessage);
    public static readonly OrderError ManyRecordsByOrderId = new(OrdersErrorCode.ManyRecordsByOrderId, ManyRecordsByIdMessage);
    public static readonly OrderError OrderIsNull = new(OrdersErrorCode.OrdersIsNull, OrdersIsNullMessage);
    public static readonly OrderError FailedInsert = new(OrdersErrorCode.FailedInsert, FailedInsertMessage);

    public static OrderError CreateValidationError(string message)
        => new(OrdersErrorCode.FailedValidation, message);
}
