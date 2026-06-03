using AutoMapper;
using MediatR;
using VehicleAuction.Application.DTOs.Bid;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Application.Features.Bid;

public record GetBidsByAuctionQuery(Guid AuctionId) : IRequest<IEnumerable<BidDto>>;

public class GetBidsByAuctionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBidsByAuctionQuery, IEnumerable<BidDto>>
{
    public async Task<IEnumerable<BidDto>> Handle(GetBidsByAuctionQuery query, CancellationToken cancellationToken)
    {
        var bids = await unitOfWork.Bids.GetByAuctionIdAsync(query.AuctionId, cancellationToken);
        return mapper.Map<IEnumerable<BidDto>>(bids);
    }
}
