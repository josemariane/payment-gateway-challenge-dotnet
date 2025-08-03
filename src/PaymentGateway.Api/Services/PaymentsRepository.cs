using System.Collections.Concurrent;

using PaymentGateway.Domain.Merchant;
using PaymentGateway.Domain.Models.Mappers;
using PaymentGateway.Domain.Models.Payment;

namespace PaymentGateway.Api.Services;

public class PaymentsRepository : IPaymentsRepository
{
    private readonly ConcurrentDictionary<Guid, PaymentEntity> _payments = new();

    public bool Add(PaymentEntity payment)
    {
        return _payments.TryAdd(payment.Id, payment);
    }

    public PaymentResponseToMerchant? GetForMerchant(Guid id)
    {
        return _payments.TryGetValue(id, out var payment) ? payment.MapToPaymentResponseToMerchant() : null;
    }
}