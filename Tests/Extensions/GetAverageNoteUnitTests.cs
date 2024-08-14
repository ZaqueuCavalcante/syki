using Syki.Back.Features.Student.CreateStudentEnrollment;

namespace Syki.Tests.Extensions;

public class GetAverageNoteUnitTests
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ExamGrades))]
    public void Should_get_average_note((List<ExamGrade> examGrades, decimal average) data)
    {
        // Arrange / Act
        var result = data.examGrades.GetAverageNote();

        // Assert
        var because = $"{data.examGrades[0].Note} & {data.examGrades[1].Note} & {data.examGrades[2].Note} = {data.average}";
        result.Should().Be(data.average, because);
    }
}
