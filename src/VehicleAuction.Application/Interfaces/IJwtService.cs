using VehicleAuction.Domain.Entities;

namespace VehicleAuction.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}
