using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Services;
using PaymentGateway.Domain.Merchant;
using PaymentGateway.Domain.Payment;

namespace PaymentGateway.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : Controller
{
    private readonly PaymentsRepository _paymentsRepository;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(PaymentsRepository paymentsRepository, ILogger<PaymentsController> logger)
    {
        _paymentsRepository = paymentsRepository;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<PostPaymentResponse>> SubmitPaymentRequest([FromBody] MerchantPaymentSubmission request)
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