using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Domain.Aggregates;
using OrderManagement.Api.Domain.Repositories;
using OrderManagement.Api.Domain.ValueObjects;
using OrderManagement.Api.Infrastructure.Persistence;

namespace OrderManagement.Api.Infrastructure.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(OrderId id)
    {
        return await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
