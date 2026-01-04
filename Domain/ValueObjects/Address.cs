namespace OrderManagement.Api.Domain.ValueObjects;

public sealed record Address(
    string Street,
    string City,
    string Country,
    string ZipCode
);
