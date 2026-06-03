using VehicleAuction.Domain.Entities;

namespace VehicleAuction.Domain.Interfaces;

public interface IBidRepository
{
    Task<IEnumerable<Bid>> GetByAuctionIdAsync(Guid auctionId, CancellationToken cancellationToken = default);
    Task<Bid?> GetHighestBidAsync(Guid auctionId, CancellationToken cancellationToken = default);
    Task AddAsync(Bid bid, CancellationToken cancellationToken = default);
    Task UpdateAsync(Bid bid, CancellationToken cancellationToken = default);
}
