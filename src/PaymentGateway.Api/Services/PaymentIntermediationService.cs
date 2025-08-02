using PaymentGateway.Domain.AcquiringBank;
using PaymentGateway.Domain.Merchant;
using PaymentGateway.Domain.Models.Mappers;
using PaymentGateway.Domain.Models.Payment;
using PaymentGateway.Infrastructure.ExternalApis.AcquiringBank;

using Refit;

namespace PaymentGateway.Api.Services;

public class PaymentIntermediationService(
    IAcquiringBankApi acquiringBankApi,
    IPaymentsRepository repository) : IPaymentIntermediationService
{

    public async Task<PaymentResponseToMerchant> ExecutePaymentOrder(PaymentRequestFromMerchant request)
    {
        var paymentEntity = request.ToPaymentEntity();
        repository.Add(paymentEntity);

        if (!paymentEntity.ValidateAndRejectIfInvalid())
        {
            return paymentEntity.MapToPaymentResponseToMerchant();
        }

        var response = await SendOrderToBank(paymentEntity);
        paymentEntity.RegisterResponseStatusAndSanitize(response.Content, response.IsSuccessStatusCode);
        return paymentEntity.MapToPaymentResponseToMerchant();
    }

    private async Task<IApiResponse<PaymentResponseFromBank>> SendOrderToBank(PaymentEntity paymentEntity)
    {
        return await acquiringBankApi.PostPayment(paymentEntity.MapToPaymentOrderToBank());
    }
}