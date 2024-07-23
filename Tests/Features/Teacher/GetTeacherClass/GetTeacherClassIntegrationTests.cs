namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_return_teacher_class()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();

        var period = await academicClient.CreateAcademicPeriod($"{DateTime.Now.Year}.1");
        await academicClient.CreateEnrollmentPeriod(period.Id);

        var ads = await academicClient.CreateCourse("ADS");

        var matematica = await academicClient.CreateDiscipline("Matemática Discreta", [ads.Id]);
        var bancoDeDados = await academicClient.CreateDiscipline("Banco de Dados", [ads.Id]);

        var chico = await academicClient.CreateTeacher("Chico");

        var classOut = await academicClient.CreateClass(matematica.Id, chico.Id, period.Id, 40, [new(Day.Segunda, Hour.H07_00, Hour.H10_00)]);
        await academicClient.CreateClass(bancoDeDados.Id, chico.Id, period.Id, 40, [new(Day.Terca, Hour.H07_00, Hour.H10_00)]);

        var teacherClient = await _back.LoggedAsTeacher(chico.Email);

        // Act
        var @class = await teacherClient.GetTeacherClass(classOut.Id);

        // Assert
        @class.Discipline.Should().Be(matematica.Name);
        @class.Code.Should().Be(matematica.Code);
        @class.Period.Should().Be(period.Id);
        @class.Schedules.Should().HaveCount(1);
    }
}
