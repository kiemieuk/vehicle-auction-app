namespace VehicleAuction.Domain.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IVehicleRepository Vehicles { get; }
    IAuctionRepository Auctions { get; }
    IBidRepository Bids { get; }
    ITokenTransactionRepository TokenTransactions { get; }
    ITopUpRequestRepository TopUpRequests { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
