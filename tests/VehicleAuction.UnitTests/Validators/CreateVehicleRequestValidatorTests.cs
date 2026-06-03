using VehicleAuction.Application.DTOs.Vehicle;
using VehicleAuction.Application.Validators;

namespace VehicleAuction.UnitTests.Validators;

public class CreateVehicleRequestValidatorTests
{
    [Fact]
    public void Should_Pass_With_Valid_Request()
    {
        var validator = new CreateVehicleRequestValidator();
        var request = new CreateVehicleRequest
        {
            Title = "SUV",
            Description = "Family car",
            Make = "Toyota",
            Model = "RAV4",
            Year = DateTime.UtcNow.Year,
            ReservePrice = 1000
        };

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }
}
