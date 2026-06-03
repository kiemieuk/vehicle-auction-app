using AutoMapper;
using MediatR;
using VehicleAuction.Application.DTOs.Auction;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Application.Features.Auction;

public record GetAuctionsQuery : IRequest<IEnumerable<AuctionSummaryDto>>;

public class GetAuctionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAuctionsQuery, IEnumerable<AuctionSummaryDto>>
{
    public async Task<IEnumerable<AuctionSummaryDto>> Handle(GetAuctionsQuery query, CancellationToken cancellationToken)
    {
        var auctions = await unitOfWork.Auctions.GetAllAsync(cancellationToken);
        return mapper.Map<IEnumerable<AuctionSummaryDto>>(auctions);
    }
}
