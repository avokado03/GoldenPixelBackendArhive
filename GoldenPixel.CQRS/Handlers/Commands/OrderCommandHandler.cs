using GoldenPixel.Core.Orders;
using GoldenPixel.CQRS.Handlers.Validation;
using GoldenPixel.Db;
using LinqToDB;
using GoldenPixelBackend.Mail;
using GoldenPixelBackend.Mail.Templates.MailModels;
using GoldenPixelBackend.Mail.Templates;
using FluentEmail.Core.Models;

namespace GoldenPixel.CQRS.Handlers.Commands;

public static class OrderCommandHandler
{
    public static async Task<CreateOrderResponse> HandleInsertCommand(GpDbConnection connection,
        CreateOrderCommand command, MailService mailService)
    {
        if (connection is null)
            throw new ArgumentNullException(nameof(connection));

        var validationResult = OrdersValidator.ValidateCreateOrderCommand(command);

        if (validationResult.ErrorsCount != 0)
        {
            return new(null, Errors.CreateValidationError(validationResult.ToString()));
        }

        var id = Guid.NewGuid();
        var order = new Orders
        {
            Id = id,
            Email = command.Email,
            Requester = command.Requester,
            Description = command.Description,
            Date = DateTime.Now.Date
        };

        int result = 0;

        try
        {
            result = await connection.InsertAsync(order);
        }
        catch
        {
            return new(null, Errors.FailedInsert);
        }

#if !DEBUG
        var emailSendResult = await SendEmailMessages(order, mailService);
        if(!string.IsNullOrEmpty(emailSendResult))
            return new(null, Errors.CreateEmailFailedError(emailSendResult));
#endif

        if (result == 0)
            return new(null, Errors.FailedInsert);

        return new(id, null);
    }

    /// <summary>
    /// Отправка оповещений о новой заявке
    /// </summary>
    /// <returns>Текст с ошибками smtp</returns>
    private static async Task<string> SendEmailMessages(Orders order, MailService mailService)
    {
        var errorMessage = string.Empty;

#nullable enable
        SendResponse? emailNotificationResult = null;
        SendResponse? emailRequesterResult = null;
#nullable disable

        string CreateErrorMessage(Exception ex, string mailError)
        {
            return $"{ex.Message}: {mailError}. {ex.StackTrace} \n";
        }

        try
        {
            emailNotificationResult = await mailService.SendAsync(new MailModel<NotificationMailModel>
            {
                To = mailService.EmailOptions.Value.NoReplyMail,
                Template = MailTemplates.GetNotificationTemplate(order.Id, order.Email, order.Description, order.Requester, order.Date),
            });

            emailRequesterResult = await mailService.SendAsync(new MailModel<RequestMailModel>
            {
                To = order.Email,
                Template = MailTemplates.GetRequestTemplate(order.Requester),

            }).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            if (emailNotificationResult != null && emailNotificationResult.ErrorMessages.Any())
                errorMessage += CreateErrorMessage(ex, emailNotificationResult.ErrorMessages.First());
            if (emailRequesterResult != null && emailRequesterResult.ErrorMessages.Any())
                errorMessage += CreateErrorMessage(ex, emailRequesterResult.ErrorMessages.First());
        }
        return errorMessage;
    }
}
