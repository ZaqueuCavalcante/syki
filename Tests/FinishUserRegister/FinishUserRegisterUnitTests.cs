using Syki.Front.FinishUserRegister;
using Syki.Back.Features.Cross.CreatePendingUserRegister;

namespace Syki.Tests.FinishUserRegister;

public class FinishUserRegisterUnitTests
{
    [Test]
    public void Should_set_start_date()
    {
        // Arrange
        var register = new UserRegister("demail@syki.com");

        // Act
        register.Finish();

        // Assert
        register.TrialStart.Should().Be(DateOnly.FromDateTime(DateTime.Now));
    }

    [Test]
    public void Should_set_end_date()
    {
        // Arrange
        var register = new UserRegister("demail@syki.com");

        // Act
        register.Finish();

        // Assert
        register.TrialEnd.Should().Be(DateOnly.FromDateTime(DateTime.Now.AddDays(7)));
    }

    [Test]
    public void Should_return_error_when_register_is_already_done()
    {
        // Arrange
        var register = new UserRegister("demail@syki.com");
        register.Finish();

        // Act
        Action act = () => register.Finish();

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE025);
    }

    [Test]
    public void Should_return_a_new_user_in_academico()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var email = "demail@syki.com";
        var password = "Test@123";

        // Act
        var user = CreateUserIn.NewAcademico(institutionId, email, password);

        // Assert
        user.Name.Should().Be(email);
        user.Email.Should().Be(email);
        user.Password.Should().Be(password);
        user.InstitutionId.Should().Be(institutionId);
        user.Role.Should().Be("Academico");
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidPasswords))]
    public void Should_return_false_when_password_is_invalid(string password)
    {
        // Arrange
        var setup = new SetupPassword { Password = password };

        // Act
        setup.Validate();

        // Assert
        setup.IsValid().Should().BeFalse();
    }
}
