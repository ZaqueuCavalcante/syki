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
        var firstLesson = mathClass.Lessons.First();

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        var title = "Modelagem de Banco de Dados";
        var description = "Modele um banco de dados para uma barbearia.";
        var dueDate = DateTime.Now.AddDays(15);

        // Act
        var response = await teacherClient.CreateClassActivity(mathClass.Id, firstLesson.Id, title, description, dueDate);

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
        var response = await teacherClient.CreateClassActivity(Guid.NewGuid(), Guid.NewGuid(), "", "", DateTime.Now);

        // Assert
        response.ShouldBeError(new ClassNotFound());
    }

    [Test]
    public async Task Should_not_create_class_activity_when_lesson_not_found()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        // Act
        var response = await teacherClient.CreateClassActivity(mathClass.Id, Guid.NewGuid(), "", "", DateTime.Now);

        // Assert
        response.ShouldBeError(new LessonNotFound());
    }
}
