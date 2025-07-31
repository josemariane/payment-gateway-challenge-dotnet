using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Services;
using PaymentGateway.Domain.Payment;

namespace PaymentGateway.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : Controller
{
    private readonly PaymentsRepository _paymentsRepository;

    public PaymentsController(PaymentsRepository paymentsRepository)
    {
        _paymentsRepository = paymentsRepository;
    }

    [HttpPost()]
    public async Task<ActionResult<PostPaymentResponse>> SubmitPaymentRequest([FromBody] PostPaymentRequest request)
    {
        return NotFound();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PostPaymentResponse?>> GetPaymentAsync(Guid id)
    {
        var payment = _paymentsRepository.Get(id);

        return payment is null ? new NotFoundResult() : new OkObjectResult(payment);
    }
}