using VehicleAuction.Domain.Entities;

namespace VehicleAuction.Domain.Interfaces;

public interface IAuctionRepository
{
    Task<Auction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Auction>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Auction>> GetLiveAuctionsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Auction>> GetScheduledAuctionsAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Auction auction, CancellationToken cancellationToken = default);
    Task UpdateAsync(Auction auction, CancellationToken cancellationToken = default);
}
