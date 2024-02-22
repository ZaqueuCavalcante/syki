using Syki.Shared;
using Syki.Back.CreatePendingDemo;

namespace Syki.Tests.SetupDemo;

public class SetupDemoUnitTests
{
    [Test]
    public void Should_set_start_date_on_demo_setup()
    {
        // Arrange
        var demo = new Demo("demail@syki.com");

        // Act
        demo.Setup();

        // Assert
        demo.Start.Should().Be(DateOnly.FromDateTime(DateTime.Now));
    }

    [Test]
    public void Should_set_end_date_on_demo_setup()
    {
        // Arrange
        var demo = new Demo("demail@syki.com");

        // Act
        demo.Setup();

        // Assert
        demo.End.Should().Be(DateOnly.FromDateTime(DateTime.Now.AddDays(7)));
    }

    [Test]
    public void Should_return_error_when_setup_is_already_done()
    {
        // Arrange
        var demo = new Demo("demail@syki.com");
        demo.Setup();

        // Act
        Action act = () => demo.Setup();

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE025);
    }

    [Test]
    public void Should_return_a_new_user_in_academico_for_demo()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var email = "demail@syki.com";
        var password = "Test@123";

        // Act
        var user = UserIn.NewDemoAcademico(institutionId, email, password);

        // Assert
        user.Name.Should().Be($"DEMO - {email}");
        user.Email.Should().Be(email);
        user.Password.Should().Be(password);
        user.InstitutionId.Should().Be(institutionId);
        user.Role.Should().Be("Academico");
    }
}
