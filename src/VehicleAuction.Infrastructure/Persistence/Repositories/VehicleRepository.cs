using Microsoft.EntityFrameworkCore;
using VehicleAuction.Domain.Entities;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Infrastructure.Persistence.Repositories;

public class VehicleRepository(ApplicationDbContext dbContext) : IVehicleRepository
{
    public Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.Vehicles.Include(v => v.Auction).FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

    public async Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default)
        => await dbContext.Vehicles.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IEnumerable<Vehicle>> GetByAuctioneerIdAsync(Guid auctioneerId, CancellationToken cancellationToken = default)
        => await dbContext.Vehicles.AsNoTracking().Where(v => v.AuctioneerId == auctioneerId).ToListAsync(cancellationToken);

    public Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
        => dbContext.Vehicles.AddAsync(vehicle, cancellationToken).AsTask();

    public Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        dbContext.Vehicles.Update(vehicle);
        return Task.CompletedTask;
    }
}
