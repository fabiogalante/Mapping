using OrderManagement.Api.Domain.Aggregates;
using OrderManagement.Api.Domain.ValueObjects;

namespace OrderManagement.Api.Domain.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(OrderId id);
    Task AddAsync(Order order);
    Task SaveChangesAsync();
}
