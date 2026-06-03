using VehicleAuction.Domain.Entities;

namespace VehicleAuction.Domain.Interfaces;

public interface ITokenTransactionRepository
{
    Task<IEnumerable<TokenTransaction>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(TokenTransaction transaction, CancellationToken cancellationToken = default);
}
