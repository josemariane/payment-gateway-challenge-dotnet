using PaymentGateway.Domain.Merchant;
using PaymentGateway.Domain.Models.Payment;

namespace PaymentGateway.Api.Services;

public interface IPaymentsRepository
{
    bool Add(PaymentEntity payment);

    PaymentResponseToMerchant? GetForMerchant(Guid id);
}