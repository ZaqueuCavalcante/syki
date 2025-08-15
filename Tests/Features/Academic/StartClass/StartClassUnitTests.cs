using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.CreateStudent;

namespace Syki.Tests.Features.Academic.StartClass;

public class StartClassUnitTests
{
    [Test]
    public void Should_start_class_with_2_students()
    {
        // Arrange
        var studentA = new SykiStudent(Guid.CreateVersion7(), Guid.CreateVersion7(), "Zaqueu", Guid.CreateVersion7());
        var studentB = new SykiStudent(Guid.CreateVersion7(), Guid.CreateVersion7(), "Maria", Guid.CreateVersion7());
        var @class = Class.New(Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(), "2023.2", 40, []).Success;
        @class.Students = [studentA, studentB];

        // Act
        @class.Start();

        // Assert
        @class.Status.Should().Be(ClassStatus.Started);
    }
}
