using PaymentGateway.Domain.AcquiringBank;
using PaymentGateway.Domain.Merchant;
using PaymentGateway.Domain.Models.Payment;

using Riok.Mapperly.Abstractions;

namespace PaymentGateway.Domain.Models.Mappers;

[Mapper]
public static partial class PaymentEntityMapper
{
    [MapperIgnoreTarget(nameof(PaymentEntity.Id))]
    [MapperIgnoreTarget(nameof(PaymentEntity.Status))]
    [MapperIgnoreSource(nameof(PaymentRequestFromMerchant.ExpiryMonth))]
    [MapperIgnoreSource(nameof(PaymentRequestFromMerchant.ExpiryYear))]
    private static partial PaymentEntity MapRequestFromMerchantToPaymentEntity(
        PaymentRequestFromMerchant request,
        string cardNumberLastFour,
        ExpiryDate expiryDate);

    [UserMapping(Default = true)]
    public static PaymentEntity ToPaymentEntity(this PaymentRequestFromMerchant request)
    {
        ExpiryDate.TryParse(request.ExpiryMonth, request.ExpiryYear, out var expiryDate);
        return MapRequestFromMerchantToPaymentEntity(request, request.CardNumberSensitive[^4..], expiryDate.GetValueOrDefault());
    }

    [MapperIgnoreSource(nameof(PaymentEntity.Id))]
    [MapperIgnoreSource(nameof(PaymentEntity.Status))]
    [MapperIgnoreSource(nameof(PaymentEntity.CardNumberLastFour))]
    public static partial PaymentOrderToBank MapToPaymentOrderToBank(this PaymentEntity entity);

    [MapperIgnoreSource(nameof(PaymentEntity.CardNumberSensitive))]
    [MapProperty(nameof(PaymentEntity.ExpiryDate.Month), nameof(PaymentResponseToMerchant.ExpiryMonth))]
    [MapProperty(nameof(PaymentEntity.ExpiryDate.Year), nameof(PaymentResponseToMerchant.ExpiryYear))]
    public static partial PaymentResponseToMerchant MapToPaymentResponseToMerchant(this PaymentEntity entity);
}