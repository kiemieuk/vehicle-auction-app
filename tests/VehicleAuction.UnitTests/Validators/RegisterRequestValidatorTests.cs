using VehicleAuction.Application.DTOs.Auth;
using VehicleAuction.Application.Validators;

namespace VehicleAuction.UnitTests.Validators;

public class RegisterRequestValidatorTests
{
    [Fact]
    public void Should_Fail_When_Email_Is_Invalid()
    {
        var validator = new RegisterRequestValidator();
        var request = new RegisterRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "invalid-email",
            Password = "Password123"
        };

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
    }
}
