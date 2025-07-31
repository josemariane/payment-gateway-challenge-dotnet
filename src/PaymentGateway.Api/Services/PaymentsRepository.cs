using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Services;

public class PaymentsRepository
{
    private readonly Dictionary<Guid, PostPaymentResponse> _payments = new();
    
    public bool Add(PostPaymentResponse payment)
    {
       return _payments.TryAdd(payment.Id, payment);
    }

    public PostPaymentResponse? Get(Guid id)
    {
        return _payments.TryGetValue(id, out var payment) ? payment : null;
    }
}