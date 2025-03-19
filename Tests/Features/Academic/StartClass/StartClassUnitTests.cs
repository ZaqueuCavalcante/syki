using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.CreateStudent;

namespace Syki.Tests.Features.Academic.StartClass;

public class StartClassUnitTests
{
    [Test]
    public void Should_start_class_with_2_students()
    {
        // Arrange
        var studentA = new SykiStudent(Guid.NewGuid(), Guid.NewGuid(), "Zaqueu", Guid.NewGuid());
        var studentB = new SykiStudent(Guid.NewGuid(), Guid.NewGuid(), "Maria", Guid.NewGuid());
        var @class = Class.New(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "2023.2", 40, []).GetSuccess();
        @class.Students = [studentA, studentB];

        // Act
        @class.Start();

        // Assert
        @class.Status.Should().Be(ClassStatus.Started);

        var examGradesA = @class.ExamGrades.Where(x => x.StudentId == studentA.Id);
        examGradesA.Should().HaveCount(3);
        examGradesA.Count(x => x.ClassStudentNoteType == ClassStudentNoteType.N1).Should().Be(1);
        examGradesA.Count(x => x.ClassStudentNoteType == ClassStudentNoteType.N2).Should().Be(1);
        examGradesA.Count(x => x.ClassStudentNoteType == ClassStudentNoteType.N3).Should().Be(1);
        examGradesA.Should().AllSatisfy(x => x.Note.Should().Be(0));

        var examGradesB = @class.ExamGrades.Where(x => x.StudentId == studentB.Id);
        examGradesB.Should().HaveCount(3);
        examGradesB.Count(x => x.ClassStudentNoteType == ClassStudentNoteType.N1).Should().Be(1);
        examGradesB.Count(x => x.ClassStudentNoteType == ClassStudentNoteType.N2).Should().Be(1);
        examGradesB.Count(x => x.ClassStudentNoteType == ClassStudentNoteType.N3).Should().Be(1);
        examGradesB.Should().AllSatisfy(x => x.Note.Should().Be(0));
    }
}
