namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_class_activity()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        var note = ClassNoteType.N1;
        var title = "Modelagem de Banco de Dados";
        var description = "Modele um banco de dados para uma barbearia.";
        var type = ClassActivityType.Work;
        var weight = 20;
        var dueDate = DateTime.Now.AddDays(15).ToDateOnly();

        // Act
        var response = await teacherClient.CreateClassActivity(mathClass.Id, note, title, description, type, weight, dueDate, Hour.H10_00);

        // Assert
        response.ShouldBeSuccess();
    }

    [Test]
    public async Task Should_not_create_class_activity_when_class_not_found()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        // Act
        var response = await teacherClient.CreateClassActivity(Guid.NewGuid(), ClassNoteType.N2, "", "", ClassActivityType.Exam, 50, DateTime.Now.ToDateOnly(), Hour.H08_00);

        // Assert
        response.ShouldBeError(new ClassNotFound());
    }

    [Test]
    [TestCase(-1)]
    [TestCase(101)]
    public async Task Should_not_create_class_activity_with_invalid_weight(int weight)
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        var note = ClassNoteType.N1;
        var title = "Modelagem de Banco de Dados";
        var description = "Modele um banco de dados para uma barbearia.";
        var type = ClassActivityType.Work;
        var dueDate = DateTime.Now.AddDays(15).ToDateOnly();

        // Act
        var response = await teacherClient.CreateClassActivity(mathClass.Id, note, title, description, type, weight, dueDate, Hour.H10_00);

        // Assert
        response.ShouldBeError(new InvalidClassActivityWeight());
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ClassActivityInvalidWeightsLists))]
    public async Task Should_not_add_class_activities_with_invalid_weights(List<int> weights)
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        var note = ClassNoteType.N1;
        var title = "Modelagem de Banco de Dados";
        var description = "Modele um banco de dados para uma barbearia.";
        var type = ClassActivityType.Work;
        var dueDate = DateTime.Now.AddDays(15).ToDateOnly();
        OneOf<SuccessOut, ErrorOut> response = new SuccessOut();

        // Act
        foreach (var weight in weights)
        {
            response = await teacherClient.CreateClassActivity(mathClass.Id, note, title, description, type, weight, dueDate, Hour.H10_00);
        }

        // Assert
        response.ShouldBeError(new InvalidClassActivityWeight());
    }
}
