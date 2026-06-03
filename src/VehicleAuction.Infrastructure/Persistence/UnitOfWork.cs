using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Infrastructure.Persistence;

public class UnitOfWork(
    ApplicationDbContext dbContext,
    IUserRepository users,
    IVehicleRepository vehicles,
    IAuctionRepository auctions,
    IBidRepository bids,
    ITokenTransactionRepository tokenTransactions,
    ITopUpRequestRepository topUpRequests) : IUnitOfWork
{
    public IUserRepository Users { get; } = users;
    public IVehicleRepository Vehicles { get; } = vehicles;
    public IAuctionRepository Auctions { get; } = auctions;
    public IBidRepository Bids { get; } = bids;
    public ITokenTransactionRepository TokenTransactions { get; } = tokenTransactions;
    public ITopUpRequestRepository TopUpRequests { get; } = topUpRequests;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
