using OrderManagement.Api.Domain.Entities;
using OrderManagement.Api.Domain.Exceptions;
using OrderManagement.Api.Domain.ValueObjects;

namespace OrderManagement.Api.Domain.Aggregates;

public sealed class Order
{
    private readonly List<OrderItem> _items = new();
    
    private readonly List<Address> _shippingAddresses = new();

    public OrderId Id { get; }
    public CustomerId CustomerId { get; }
    public Money TotalPrice { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public IReadOnlyCollection<Address> ShippingAddresses => _shippingAddresses.AsReadOnly();

    private Order() { } // For EF Core

    public Order(OrderId id, CustomerId customerId)
    {
        Id = id;
        CustomerId = customerId;
        TotalPrice = Money.Zero;
    }

    public void AddItem(ProductId productId, int quantity, Money unitPrice)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be positive.");

        var item = new OrderItem(productId, quantity, unitPrice);
        _items.Add(item);
        RecalculateTotal();
    }

    public void AddShippingAddress(Address address)
    {
        if (address == null)
            throw new DomainException("Address cannot be null.");

        _shippingAddresses.Add(address);
    }

    private void RecalculateTotal()
    {
        TotalPrice = _items
            .Select(i => i.SubTotal)
            .Aggregate(Money.Zero, (sum, next) => sum + next);
    }
}
