namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    [TestCase(new int[] { }, new int[] { }, new int[] { }, new int[] { 100, 100, 100 })]
    [TestCase(new int[] { 25 }, new int[] { }, new int[] { }, new int[] { 75, 100, 100 })]
    [TestCase(new int[] { 30, 55 }, new int[] { 20, 20 }, new int[] { 50 }, new int[] { 15, 60, 50 })]
    [TestCase(new int[] { 30, 40, 20, 10 }, new int[] { 20, 80 }, new int[] { 99 }, new int[] { 0, 0, 1 })]
    public async Task Should_return_class_notes_remaining_weights(int[] n1Weights, int[] n2Weights, int[] n3Weights, int[] result)
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        TeacherOut chico = await academicClient.CreateTeacher();
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        // Act
        foreach (var weight in n1Weights)
        {
            await teacherClient.CreateClassActivity(mathClass.Id, ClassNoteType.N1, type: ClassActivityType.Work, weight: weight);
        }
        foreach (var weight in n2Weights)
        {
            await teacherClient.CreateClassActivity(mathClass.Id, ClassNoteType.N2, type: ClassActivityType.Work, weight: weight);
        }
        foreach (var weight in n3Weights)
        {
            await teacherClient.CreateClassActivity(mathClass.Id, ClassNoteType.N3, type: ClassActivityType.Work, weight: weight);
        }

        // Assert
        var remainingWeights = (await teacherClient.GetClassNotesRemainingWeights(mathClass.Id)).GetSuccess();
        remainingWeights[0].Should().BeEquivalentTo(new ClassNoteRemainingWeightsOut { Note = ClassNoteType.N1, Weight = result[0] });
        remainingWeights[1].Should().BeEquivalentTo(new ClassNoteRemainingWeightsOut { Note = ClassNoteType.N2, Weight = result[1] });
        remainingWeights[2].Should().BeEquivalentTo(new ClassNoteRemainingWeightsOut { Note = ClassNoteType.N3, Weight = result[2] });
    }
}
