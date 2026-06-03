using AutoMapper;
using MediatR;
using VehicleAuction.Application.DTOs.Token;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Application.Features.Token;

public record GetTransactionHistoryQuery(Guid UserId) : IRequest<IEnumerable<TransactionDto>>;

public class GetTransactionHistoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetTransactionHistoryQuery, IEnumerable<TransactionDto>>
{
    public async Task<IEnumerable<TransactionDto>> Handle(GetTransactionHistoryQuery query, CancellationToken cancellationToken)
    {
        var transactions = await unitOfWork.TokenTransactions.GetByUserIdAsync(query.UserId, cancellationToken);
        return mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }
}
