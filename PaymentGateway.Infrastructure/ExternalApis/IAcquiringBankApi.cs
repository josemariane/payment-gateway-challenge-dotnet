using PaymentGateway.Domain.Payment;
using PaymentGateway.Domain.AcquiringBank;

using Refit;

namespace PaymentGateway.Infrastructure.ExternalApis;

public interface  IAcquiringBankApi
{
    [Refit.Post("/payments")]
    public Task<IApiResponse<SubmittedPaymentResponse>> PostPayment(PaymentSubmissionRequest req);
}