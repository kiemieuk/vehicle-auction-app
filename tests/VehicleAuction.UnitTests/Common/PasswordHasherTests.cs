using VehicleAuction.Application.Common;

namespace VehicleAuction.UnitTests.Common;

public class PasswordHasherTests
{
    [Fact]
    public void Verify_Should_Return_True_For_Valid_Password()
    {
        const string password = "Password123!";
        var hash = PasswordHasher.Hash(password);

        var isValid = PasswordHasher.Verify(password, hash);

        Assert.True(isValid);
    }
}
