using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.CreateStudent;

namespace Syki.Tests.Features.Academic.StartClass;

public class StartClassUnitTests
{
    [Test]
    public void Should_start_class_with_2_students()
    {
        // Arrange
        var studentA = new SykiStudent(Guid.CreateVersion7(), Guid.CreateVersion7(), "Zaqueu", Guid.CreateVersion7(), System.DateTime.Parse("2005-01-01"));
        var studentB = new SykiStudent(Guid.CreateVersion7(), Guid.CreateVersion7(), "Maria", Guid.CreateVersion7(), System.DateTime.Parse("2006-02-02"));
        var @class = new Class(Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(), "2023.2", 40, new System.Collections.Generic.List<Schedule>());
        @class.Students = new System.Collections.Generic.List<SykiStudent> { studentA, studentB };

        // Act
        @class.Start();

        // Assert
        @class.Status.Should().Be(ClassStatus.Started);
    }
}
