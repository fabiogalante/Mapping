using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Domain.Aggregates;
using OrderManagement.Api.Domain.Exceptions;
using OrderManagement.Api.Domain.Repositories;
using OrderManagement.Api.Domain.ValueObjects;
using OrderManagement.Api.Infrastructure.Persistence;
using OrderManagement.Api.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext with In-Memory database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("OrderManagementDb");
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

// Configure Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Endpoints
app.MapPost("/api/orders", async (CreateOrderRequest request, IOrderRepository repository) =>
{
    try
    {
        var order = new Order(
            new OrderId(Guid.NewGuid()),
            new CustomerId(request.CustomerId)
        );

        foreach (var item in request.Items)
        {
            order.AddItem(
                new ProductId(item.ProductId),
                item.Quantity,
                new Money(item.UnitPrice, item.Currency)
            );
        }

        await repository.AddAsync(order);
        await repository.SaveChangesAsync();

        return Results.Created($"/api/orders/{order.Id.Value}", new
        {
            orderId = order.Id.Value,
            customerId = order.CustomerId.Value,
            totalAmount = order.TotalPrice.Amount,
            currency = order.TotalPrice.Currency,
            itemCount = order.Items.Count
        });
    }
    catch (DomainException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("CreateOrder")
.WithOpenApi();

app.MapGet("/api/orders/{id:guid}", async (Guid id, IOrderRepository repository) =>
{
    var order = await repository.GetByIdAsync(new OrderId(id));

    if (order == null)
        return Results.NotFound();

    return Results.Ok(new
    {
        orderId = order.Id.Value,
        customerId = order.CustomerId.Value,
        totalAmount = order.TotalPrice.Amount,
        currency = order.TotalPrice.Currency,
        items = order.Items.Select(i => new
        {
            productId = i.ProductId.Value,
            quantity = i.Quantity,
            unitPrice = i.UnitPrice.Amount,
            currency = i.UnitPrice.Currency,
            subTotal = i.SubTotal.Amount
        })
    });
})
.WithName("GetOrder")
.WithOpenApi();

app.MapGet("/api/orders", async (AppDbContext context) =>
{
    var orders = await context.Orders
        .Select(o => new OrderListItem(
            o.Id.Value,
            o.TotalPrice.Amount,
            o.Items.Count))
        .ToListAsync();

    return Results.Ok(orders);
})
.WithName("GetOrders")
.WithOpenApi();

app.MapPost("/api/orders/{id:guid}/items", async (
    Guid id,
    AddItemRequest request,
    IOrderRepository repository) =>
{
    try
    {
        var order = await repository.GetByIdAsync(new OrderId(id));

        if (order == null)
            return Results.NotFound();

        order.AddItem(
            new ProductId(request.ProductId),
            request.Quantity,
            new Money(request.UnitPrice, request.Currency)
        );

        await repository.SaveChangesAsync();

        return Results.Ok(new
        {
            orderId = order.Id.Value,
            totalAmount = order.TotalPrice.Amount,
            itemCount = order.Items.Count
        });
    }
    catch (DomainException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("AddOrderItem")
.WithOpenApi();

app.Run();

// DTOs
public record CreateOrderRequest(
    Guid CustomerId,
    List<OrderItemRequest> Items
);

public record OrderItemRequest(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    string Currency
);

public record AddItemRequest(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    string Currency
);

public record OrderListItem(
    Guid OrderId,
    decimal TotalAmount,
    int ItemsCount
);
