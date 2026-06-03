using MediatR;
using System.Security.Cryptography;
using System.Text;
using VehicleAuction.Application.DTOs.Auth;
using VehicleAuction.Domain.Entities;
using VehicleAuction.Domain.Enums;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Application.Features.Auth;

public record RegisterUserCommand(RegisterRequest Request) : IRequest<Guid>;

public class RegisterUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RegisterUserCommand, Guid>
{
    public async Task<Guid> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var existing = await unitOfWork.Users.GetByEmailAsync(command.Request.Email, cancellationToken);
        if (existing is not null)
        {
            throw new InvalidOperationException("User already exists.");
        }

        var user = new User
        {
            FirstName = command.Request.FirstName,
            LastName = command.Request.LastName,
            Email = command.Request.Email,
            PasswordHash = ComputeHash(command.Request.Password),
            Role = UserRole.Bidder,
            TokenBalance = 0m
        };

        await unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }

    private static string ComputeHash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }
}
