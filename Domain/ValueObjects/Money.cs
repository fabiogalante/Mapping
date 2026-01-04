using OrderManagement.Api.Domain.Exceptions;

namespace OrderManagement.Api.Domain.ValueObjects;

public sealed record Money(decimal Amount, string Currency)
{
    public static Money Zero => new(0, "USD");

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new DomainException("Currencies must match.");

        return new Money(a.Amount + b.Amount, a.Currency);
    }
}
