using Microsoft.EntityFrameworkCore;
using VehicleAuction.Domain.Entities;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.UsersData.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => dbContext.UsersData.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var exists = await dbContext.UsersData.AnyAsync(x => x.Id == user.Id, cancellationToken);
        if (exists)
        {
            dbContext.UsersData.Update(user);
        }
        else
        {
            await dbContext.UsersData.AddAsync(user, cancellationToken);
        }
    }
}
