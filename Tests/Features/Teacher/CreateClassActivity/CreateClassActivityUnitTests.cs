using Syki.Back.Features.Teacher.CreateClassActivity;

namespace Syki.Tests.Features.Teacher.CreateClassActivity;

public class CreateClassActivityUnitTests
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ClassActivityValidWeights))]
    public void Should_create_class_activity_with_valid_weight(int weight)
    {
        // Arrange // Act
        var activity = ClassActivity.New(
            Guid.CreateVersion7(),
            ClassNoteType.N1,
            "Banco de Dados",
            "Modele um banco de dados.",
            ClassActivityType.Work,
            weight,
            DateTime.UtcNow.AddDays(7).ToDateOnly(),
            Hour.H19_00,
            []
        );

        // Assert
        activity.ShouldBeSuccess();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ClassActivityInvalidWeights))]
    public void Should_not_create_class_activity_with_invalid_weight(int weight)
    {
        // Arrange // Act
        var activity = ClassActivity.New(
            Guid.CreateVersion7(),
            ClassNoteType.N1,
            "Banco de Dados",
            "Modele um banco de dados.",
            ClassActivityType.Work,
            weight,
            DateTime.UtcNow.AddDays(7).ToDateOnly(),
            Hour.H19_00,
            []
        );

        // Assert
        activity.ShouldBeError(new InvalidClassActivityWeight());
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ClassActivityValidWeightsLists))]
    public void Should_add_class_activities_with_valid_weights(int[] weights)
    {
        // Arrange
        var @class = TestData.GetClass();
        foreach (var weight in weights)
        {
            var activity = ClassActivity.New(
                @class.Id,
                ClassNoteType.N1,
                "Banco de Dados",
                "Modele um banco de dados.",
                ClassActivityType.Work,
                weight,
                DateTime.UtcNow.AddDays(7).ToDateOnly(),
                Hour.H19_00,
                []
            ).Success;
            
            // Act
            var result = @class.AddActivity(activity);

            // Assert
            result.ShouldBeSuccess();
        }
        @class.Activities.Should().HaveCount(weights.Length);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ClassActivityInvalidWeightsLists))]
    public void Should_not_add_class_activities_with_invalid_weights(int[] weights)
    {
        // Arrange
        var @class = TestData.GetClass();
        OneOf<SykiSuccess, SykiError> result = new SykiSuccess();

        foreach (var weight in weights)
        {
            var activity = ClassActivity.New(
                @class.Id,
                ClassNoteType.N1,
                "Banco de Dados",
                "Modele um banco de dados.",
                ClassActivityType.Work,
                weight,
                DateTime.UtcNow.AddDays(7).ToDateOnly(),
                Hour.H19_00,
                []
            ).Success;

            // Act
            result = @class.AddActivity(activity);
        }

        // Assert
        result.ShouldBeError(new InvalidClassActivityWeight());
        @class.Activities.Should().HaveCount(weights.Length-1);
    }
}
