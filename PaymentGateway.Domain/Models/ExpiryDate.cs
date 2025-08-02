namespace PaymentGateway.Domain.Models;

public record struct ExpiryDate
{
    private DateOnly _state = default;

    public ExpiryDate() { }

    /// <summary>
    /// Checks if this expiry date is still valid.
    /// Assumes the card is valid until the last day of its expiry month.
    /// </summary>
    /// <returns></returns>
    public bool IsInFuture()
    {
        var today = DateTime.Today;
        return (_state.Month, _state.Year) switch
        {
            var (_, year) when year > today.Year => true,
            var (month, year) when year == today.Year => month >= today.Month,
            _ => false,
        };
    }

    public int Month => _state.Month;
    public int Year => _state.Year;

    /// <summary>
    /// Creates a new expiry date from strings.
    /// Doesn't validate if it's in the future, only if the strings represent a month/year date. 
    /// </summary>
    public static bool TryParse(int month, int year, out ExpiryDate? expiryDate)
    {
        expiryDate = null;
        if (month is < 1 or > 12)
        {
            return false;
        }
        if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
        {
            return false;
        }
        expiryDate = new ExpiryDate { _state = new DateOnly(year, month, 1) };
        return true;
    }
    
    public override string ToString()
    {
        return _state.ToString("MM/yyyy");
    }

}