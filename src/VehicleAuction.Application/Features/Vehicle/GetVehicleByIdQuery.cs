using AutoMapper;
using MediatR;
using VehicleAuction.Application.DTOs.Vehicle;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Application.Features.Vehicle;

public record GetVehicleByIdQuery(Guid VehicleId) : IRequest<VehicleDto?>;

public class GetVehicleByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetVehicleByIdQuery, VehicleDto?>
{
    public async Task<VehicleDto?> Handle(GetVehicleByIdQuery query, CancellationToken cancellationToken)
    {
        var vehicle = await unitOfWork.Vehicles.GetByIdAsync(query.VehicleId, cancellationToken);
        return vehicle is null ? null : mapper.Map<VehicleDto>(vehicle);
    }
}
