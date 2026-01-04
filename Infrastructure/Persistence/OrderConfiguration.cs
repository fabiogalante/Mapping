using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Api.Domain.Aggregates;
using OrderManagement.Api.Domain.Entities;
using OrderManagement.Api.Domain.ValueObjects;

namespace OrderManagement.Api.Infrastructure.Persistence;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", table =>
        {
            table.HasCheckConstraint(
                "CK_Orders_TotalAmount_Positive",
                "[TotalAmount] >= 0");
        });

        builder.HasKey(o => o.Id);

        // Strongly Typed ID - OrderId
        builder.Property(o => o.Id)
            .HasConversion(
                id => id.Value,
                value => new OrderId(value));

        // Strongly Typed ID - CustomerId
        builder.Property(o => o.CustomerId)
            .HasConversion(
                id => id.Value,
                value => new CustomerId(value))
            .IsRequired();

        // Value Object - Money (TotalPrice)
        builder.OwnsOne(o => o.TotalPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("TotalAmount")
                .HasPrecision(18, 2);

            money.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3);
        });

        // Ignore read-only navigation properties
        builder.Ignore(o => o.Items);
        builder.Ignore(o => o.ShippingAddresses);

        // Collection of Entities - OrderItems
        builder.OwnsMany<OrderItem>("_items", items =>
        {
            items.ToTable("OrderItems", table =>
            {
                table.HasCheckConstraint(
                    "CK_OrderItems_Quantity_Positive",
                    "[Quantity] > 0");
            });

            items.WithOwner().HasForeignKey("OrderId");

            items.Property<ProductId>("ProductId")
                .HasConversion(
                    id => id.Value,
                    value => new ProductId(value));

            items.Property(i => i.Quantity);

            items.OwnsOne(i => i.UnitPrice, price =>
            {
                price.Property(p => p.Amount)
                    .HasColumnName("UnitPrice")
                    .HasPrecision(18, 2);

                price.Property(p => p.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3);
            });
        });

        // Collection of Value Objects - ShippingAddresses
        builder.OwnsMany(typeof(Address), "_shippingAddresses", addresses =>
        {
            addresses.ToTable("OrderShippingAddresses");
            addresses.WithOwner().HasForeignKey("OrderId");

            addresses.Property<string>("Street")
                .HasMaxLength(200);

            addresses.Property<string>("City")
                .HasMaxLength(100);

            addresses.Property<string>("Country")
                .HasMaxLength(100);

            addresses.Property<string>("ZipCode")
                .HasMaxLength(20);
        });
    }
}