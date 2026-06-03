using Microsoft.EntityFrameworkCore;
using VehicleAuction.Domain.Entities;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Infrastructure.Persistence.Repositories;

public class TopUpRequestRepository(ApplicationDbContext dbContext) : ITopUpRequestRepository
{
    public Task<TopUpRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.TopUpRequests.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public Task AddAsync(TopUpRequest request, CancellationToken cancellationToken = default)
        => dbContext.TopUpRequests.AddAsync(request, cancellationToken).AsTask();

    public Task UpdateAsync(TopUpRequest request, CancellationToken cancellationToken = default)
    {
        dbContext.TopUpRequests.Update(request);
        return Task.CompletedTask;
    }
}
