using VehicleAuction.Domain.Entities;

namespace VehicleAuction.Domain.Interfaces;

public interface ITopUpRequestRepository
{
    Task<TopUpRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(TopUpRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(TopUpRequest request, CancellationToken cancellationToken = default);
}
