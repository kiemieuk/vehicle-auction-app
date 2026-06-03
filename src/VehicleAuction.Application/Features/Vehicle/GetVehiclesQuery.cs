using AutoMapper;
using MediatR;
using VehicleAuction.Application.DTOs.Vehicle;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Application.Features.Vehicle;

public record GetVehiclesQuery(int PageNumber = 1, int PageSize = 20) : IRequest<IEnumerable<VehicleDto>>;

public class GetVehiclesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetVehiclesQuery, IEnumerable<VehicleDto>>
{
    public async Task<IEnumerable<VehicleDto>> Handle(GetVehiclesQuery query, CancellationToken cancellationToken)
    {
        var vehicles = await unitOfWork.Vehicles.GetAllAsync(cancellationToken);
        var paged = vehicles.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);
        return mapper.Map<IEnumerable<VehicleDto>>(paged);
    }
}
