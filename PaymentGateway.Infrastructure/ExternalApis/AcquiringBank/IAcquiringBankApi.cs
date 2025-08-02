using PaymentGateway.Domain.AcquiringBank;

using Refit;

namespace PaymentGateway.Infrastructure.ExternalApis.AcquiringBank;

public interface IAcquiringBankApi
{
    [Post("/payments")]
    public Task<IApiResponse<PaymentResponseFromBank>> PostPayment(PaymentOrderToBank req);
}