using AutoMapper;
using VehicleAuction.Application.DTOs.Auction;
using VehicleAuction.Application.DTOs.Bid;
using VehicleAuction.Application.DTOs.Token;
using VehicleAuction.Application.DTOs.Vehicle;
using VehicleAuction.Domain.Entities;

namespace VehicleAuction.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Vehicle, VehicleDto>();
        CreateMap<Auction, AuctionDto>();
        CreateMap<Auction, AuctionSummaryDto>()
            .ForMember(dest => dest.VehicleTitle, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Title : string.Empty));
        CreateMap<Bid, BidDto>();
        CreateMap<TokenTransaction, TransactionDto>();
    }
}
