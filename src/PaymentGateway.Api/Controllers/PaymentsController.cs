using System.Net;

using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Services;
using PaymentGateway.Domain.Merchant;
using PaymentGateway.Domain.Models.Payment;

namespace PaymentGateway.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController(
    IPaymentsRepository paymentsRepository,
    IPaymentIntermediationService paymentIntermediationService,
    ILogger<PaymentsController> logger)
    : Controller
{

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(PaymentResponseToMerchant), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaymentResponseToMerchant>>
        SubmitPaymentRequest([FromBody] PaymentRequestFromMerchant request)
    {
        try
        {
            var response = await paymentIntermediationService.ExecutePaymentOrder(request);
            if (response.Status is PaymentStatus.Rejected)
            {
                return new UnprocessableEntityResult();
            }
            return CreatedAtAction("GetPayment", new { response.Id }, response);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error processing payment request from merchant.");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(PaymentResponseToMerchant), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaymentResponseToMerchant>> GetPaymentAsync(Guid id)
    {
        try
        {
            var payment = paymentsRepository.GetForMerchant(id);
            return payment is null ? new NotFoundResult() : payment;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error processing GET request");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}