using Microsoft.EntityFrameworkCore;
using VehicleAuction.Domain.Entities;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Infrastructure.Persistence.Repositories;

public class TokenTransactionRepository(ApplicationDbContext dbContext) : ITokenTransactionRepository
{
    public async Task<IEnumerable<TokenTransaction>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => await dbContext.TokenTransactions.AsNoTracking().Where(t => t.UserId == userId).OrderByDescending(t => t.CreatedAt).ToListAsync(cancellationToken);

    public Task AddAsync(TokenTransaction transaction, CancellationToken cancellationToken = default)
        => dbContext.TokenTransactions.AddAsync(transaction, cancellationToken).AsTask();
}
