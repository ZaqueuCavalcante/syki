using Syki.Back.Features.Academic.StartClass;

namespace Syki.Tests.Extensions;

public class GetAverageNoteUnitTests
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ExamGrades))]
    public void Shoud_get_average_note((List<ExamGrade> examGrades, decimal average) data)
    {
        // Arrange / Act
        var result = data.examGrades.GetAverageNote();

        // Assert
        result.Should().Be(data.average, $"{data.examGrades[0].Note} & {data.examGrades[1].Note} & {data.examGrades[2].Note} = {data.average}");
    }
}
