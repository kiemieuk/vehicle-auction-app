using Microsoft.EntityFrameworkCore;
using VehicleAuction.Domain.Entities;
using VehicleAuction.Domain.Enums;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Infrastructure.Persistence.Repositories;

public class AuctionRepository(ApplicationDbContext dbContext) : IAuctionRepository
{
    public Task<Auction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.Auctions.Include(a => a.Vehicle).Include(a => a.Bids).FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task<IEnumerable<Auction>> GetAllAsync(CancellationToken cancellationToken = default)
        => await dbContext.Auctions.Include(a => a.Vehicle).AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IEnumerable<Auction>> GetLiveAuctionsAsync(CancellationToken cancellationToken = default)
        => await dbContext.Auctions.AsNoTracking().Where(a => a.Status == AuctionStatus.Live).ToListAsync(cancellationToken);

    public async Task<IEnumerable<Auction>> GetScheduledAuctionsAsync(CancellationToken cancellationToken = default)
        => await dbContext.Auctions.AsNoTracking().Where(a => a.Status == AuctionStatus.Scheduled).ToListAsync(cancellationToken);

    public Task AddAsync(Auction auction, CancellationToken cancellationToken = default)
        => dbContext.Auctions.AddAsync(auction, cancellationToken).AsTask();

    public Task UpdateAsync(Auction auction, CancellationToken cancellationToken = default)
    {
        dbContext.Auctions.Update(auction);
        return Task.CompletedTask;
    }
}
