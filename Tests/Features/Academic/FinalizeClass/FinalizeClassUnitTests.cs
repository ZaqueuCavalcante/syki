namespace Syki.Tests.Features.Academic.FinalizeClass;

public class FinalizeClassUnitTests
{
    [Test]
    public void Should_finalize_class()
    {
        // Arrange
        var @class = TestData.GetClass("05/08/2024", "23/08/2024", [new(Day.Monday, Hour.H19_00, Hour.H20_00)]);
        @class.CreateLessons();
        @class.Lessons.ForEach(x => x.Finish());

        // Act
        var result = @class.Finish();

        // Assert
        result.ShouldBeSuccess();
        @class.Status.Should().Be(ClassStatus.Finalized);
    }

    [Test]
    public void Should_not_finalize_class_with_pending_lessons()
    {
        // Arrange
        var @class = TestData.GetClass("05/08/2024", "23/08/2024", [new(Day.Monday, Hour.H19_00, Hour.H20_00)]);
        @class.CreateLessons();

        // Act
        var result = @class.Finish();

        // Assert
        result.ShouldBeError(new AllClassLessonsMustHaveFinalizedStatus());
    }
}
