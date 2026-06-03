using VehicleAuction.Domain.Entities;

namespace VehicleAuction.Domain.Interfaces;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Vehicle>> GetByAuctioneerIdAsync(Guid auctioneerId, CancellationToken cancellationToken = default);
    Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
}
