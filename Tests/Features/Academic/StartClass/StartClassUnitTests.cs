using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.CreateStudent;

namespace Syki.Tests.Features.Academic.StartClass;

public class StartClassUnitTests
{
    [Test]
    public void Should_start_class_with_2_students()
    {
        // Arrange
        var studentAA = new SykiStudent(Guid.NewGuid(), Guid.NewGuid(), "Zaqueu", Guid.NewGuid());
        var studentBB = new SykiStudent(Guid.NewGuid(), Guid.NewGuid(), "Maria", Guid.NewGuid());
        var @class = new Class(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "2023.2", 40, [])
        {
            Students = [studentAA, studentBB]
        };

        // Act
        @class.Start();

        // Assert
        @class.Status.Should().Be(ClassStatus.Started);
        @class.ExamGrades.Should().HaveCount(6);

        @class.ExamGrades.Count(x => x.StudentId == studentAA.Id).Should().Be(3);
        @class.ExamGrades.Count(x => x.StudentId == studentBB.Id).Should().Be(3);

        @class.ExamGrades.Count(x => x.ExamType == ExamType.N1).Should().Be(2);
        @class.ExamGrades.Count(x => x.ExamType == ExamType.N2).Should().Be(2);
        @class.ExamGrades.Count(x => x.ExamType == ExamType.Final).Should().Be(2);

        @class.ExamGrades.Should().AllSatisfy(x => x.Note.Should().Be(0));
    }
}
