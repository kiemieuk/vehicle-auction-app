using AutoMapper;
using MediatR;
using VehicleAuction.Application.DTOs.Auction;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Application.Features.Auction;

public record GetAuctionByIdQuery(Guid AuctionId) : IRequest<AuctionDto?>;

public class GetAuctionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAuctionByIdQuery, AuctionDto?>
{
    public async Task<AuctionDto?> Handle(GetAuctionByIdQuery query, CancellationToken cancellationToken)
    {
        var auction = await unitOfWork.Auctions.GetByIdAsync(query.AuctionId, cancellationToken);
        return auction is null ? null : mapper.Map<AuctionDto>(auction);
    }
}
