using PaymentGateway.Domain.Merchant;

namespace PaymentGateway.Api.Services;

public interface IPaymentIntermediationService
{
    Task<PaymentResponseToMerchant> ExecutePaymentOrder(PaymentRequestFromMerchant request);
}