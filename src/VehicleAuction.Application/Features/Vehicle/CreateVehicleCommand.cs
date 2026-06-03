using MediatR;
using VehicleAuction.Application.DTOs.Vehicle;
using VehicleAuction.Domain.Entities;
using VehicleAuction.Domain.Enums;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Application.Features.Vehicle;

public record CreateVehicleCommand(Guid AuctioneerId, CreateVehicleRequest Request) : IRequest<Guid>;

public class CreateVehicleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateVehicleCommand, Guid>
{
    public async Task<Guid> Handle(CreateVehicleCommand command, CancellationToken cancellationToken)
    {
        var vehicle = new Domain.Entities.Vehicle
        {
            AuctioneerId = command.AuctioneerId,
            Title = command.Request.Title,
            Description = command.Request.Description,
            Make = command.Request.Make,
            Model = command.Request.Model,
            Year = command.Request.Year,
            ImageUrls = command.Request.ImageUrls,
            ReservePrice = command.Request.ReservePrice,
            Status = VehicleStatus.PendingApproval
        };

        await unitOfWork.Vehicles.AddAsync(vehicle, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return vehicle.Id;
    }
}
