using MediatR;
using VehicleAuction.Domain.Enums;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Application.Features.Vehicle;

public record ApproveVehicleCommand(Guid VehicleId) : IRequest<bool>;

public class ApproveVehicleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ApproveVehicleCommand, bool>
{
    public async Task<bool> Handle(ApproveVehicleCommand command, CancellationToken cancellationToken)
    {
        var vehicle = await unitOfWork.Vehicles.GetByIdAsync(command.VehicleId, cancellationToken);
        if (vehicle is null)
        {
            return false;
        }

        vehicle.Status = VehicleStatus.Approved;
        await unitOfWork.Vehicles.UpdateAsync(vehicle, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
