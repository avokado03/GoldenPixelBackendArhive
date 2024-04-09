namespace GoldenPixel.Core.Orders;

public enum OrdersErrorCode
{
    BadOrderId = 1,
    ManyRecordsByOrderId = 2,
    FailedInsert = 4,
}

public readonly record struct OrderError(OrdersErrorCode ErrorCode, string ErrorMessage);

public static class Errors
{
    private static readonly string BadOrderIdMessage = "Записи о запросе с данным Id не существует";
    private static readonly string ManyRecordsByIdMessage = "По введенному Id существует несколько записей о запросах";
    private static readonly string FailedInsertMessage = "Запрос не был добавлен";

    public static readonly OrderError BadOrderIdError = new(OrdersErrorCode.BadOrderId, BadOrderIdMessage);
    public static readonly OrderError ManyRecordsByOrderId = new(OrdersErrorCode.ManyRecordsByOrderId, ManyRecordsByIdMessage);
    public static readonly OrderError FailedInsert = new(OrdersErrorCode.FailedInsert, FailedInsertMessage);
}
