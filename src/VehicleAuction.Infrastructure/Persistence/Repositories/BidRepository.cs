using Microsoft.EntityFrameworkCore;
using VehicleAuction.Domain.Entities;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Infrastructure.Persistence.Repositories;

public class BidRepository(ApplicationDbContext dbContext) : IBidRepository
{
    public async Task<IEnumerable<Bid>> GetByAuctionIdAsync(Guid auctionId, CancellationToken cancellationToken = default)
        => await dbContext.Bids.AsNoTracking().Where(b => b.AuctionId == auctionId).OrderByDescending(b => b.TokenAmount).ToListAsync(cancellationToken);

    public Task<Bid?> GetHighestBidAsync(Guid auctionId, CancellationToken cancellationToken = default)
        => dbContext.Bids.FirstOrDefaultAsync(b => b.AuctionId == auctionId && b.IsWinning, cancellationToken);

    public Task AddAsync(Bid bid, CancellationToken cancellationToken = default)
        => dbContext.Bids.AddAsync(bid, cancellationToken).AsTask();

    public Task UpdateAsync(Bid bid, CancellationToken cancellationToken = default)
    {
        dbContext.Bids.Update(bid);
        return Task.CompletedTask;
    }
}
