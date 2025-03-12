using Syki.Back.Features.Student.CreateStudentEnrollment;

namespace Syki.Tests.Extensions;

public class GetAverageNoteUnitTests
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ExamGrades))]
    public void Should_get_average_note(List<ExamGrade> examGrades, decimal average)
    {
        // Arrange / Act
        var result = examGrades.GetAverageNote();

        // Assert
        var because = $"{examGrades[0].Note} & {examGrades[1].Note} & {examGrades[2].Note} = {average}";
        result.Should().Be(average, because);
    }
}
