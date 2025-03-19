using Syki.Back.Features.Student.CreateStudentEnrollment;

namespace Syki.Tests.Extensions;

public class GetAverageNoteUnitTests
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.Notes))]
    public void Should_get_average_note(List<StudentClassNote> notes, decimal average)
    {
        // Arrange / Act
        var result = notes.GetAverageNote();

        // Assert
        var because = $"{notes[0].Note} & {notes[1].Note} & {notes[2].Note} = {average}";
        result.Should().Be(average, because);
    }
}
