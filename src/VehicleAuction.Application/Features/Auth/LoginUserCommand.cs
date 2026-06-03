using MediatR;
using System.Security.Cryptography;
using System.Text;
using VehicleAuction.Application.DTOs.Auth;
using VehicleAuction.Application.Interfaces;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Application.Features.Auth;

public record LoginUserCommand(LoginRequest Request) : IRequest<LoginResponse>;

public class LoginUserCommandHandler(IUnitOfWork unitOfWork, IJwtService jwtService) : IRequestHandler<LoginUserCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetByEmailAsync(command.Request.Email, cancellationToken)
                   ?? throw new UnauthorizedAccessException("Invalid credentials.");

        var hashedPassword = ComputeHash(command.Request.Password);
        if (!string.Equals(user.PasswordHash, hashedPassword, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        return new LoginResponse
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role,
            Token = jwtService.GenerateToken(user)
        };
    }

    private static string ComputeHash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }
}
