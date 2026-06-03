using MediatR;
using VehicleAuction.Application.Common;
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

        if (!PasswordHasher.Verify(command.Request.Password, user.PasswordHash))
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
}
