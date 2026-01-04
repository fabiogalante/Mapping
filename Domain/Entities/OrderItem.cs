using OrderManagement.Api.Domain.ValueObjects;

namespace OrderManagement.Api.Domain.Entities;

public sealed class OrderItem
{
    public ProductId ProductId { get; }
    public int Quantity { get; }
    public Money UnitPrice { get; }
    public Money SubTotal => new(UnitPrice.Amount * Quantity, UnitPrice.Currency);

    private OrderItem() { } // For EF Core

    internal OrderItem(ProductId productId, int quantity, Money unitPrice)
    {
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}
