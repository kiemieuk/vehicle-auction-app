using MediatR;
using VehicleAuction.Application.DTOs.Token;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Application.Features.Token;

public record GetTokenBalanceQuery(Guid UserId) : IRequest<TokenBalanceResponse>;

public class GetTokenBalanceQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetTokenBalanceQuery, TokenBalanceResponse>
{
    public async Task<TokenBalanceResponse> Handle(GetTokenBalanceQuery query, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetByIdAsync(query.UserId, cancellationToken)
                   ?? throw new KeyNotFoundException("User not found.");

        return new TokenBalanceResponse { UserId = user.Id, Balance = user.TokenBalance };
    }
}
