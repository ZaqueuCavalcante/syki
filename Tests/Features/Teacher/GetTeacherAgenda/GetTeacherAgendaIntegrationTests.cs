namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_get_teacher_agenda()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var period = await client.CreateAcademicPeriod($"{DateTime.Now.Year}.1");
        var start = DateOnly.FromDateTime(DateTime.Now.AddDays(-2));
        var end = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
        await client.CreateEnrollmentPeriod(period.Id, start, end);

        var ads = await client.CreateCourse("ADS");
        var direito = await client.CreateCourse("Direito");

        var matematica = await client.CreateDiscipline("Matemática Discreta", [ads.Id]);
        var bancoDeDados = await client.CreateDiscipline("Banco de Dados", [ads.Id]);
        var estruturaDeDados = await client.CreateDiscipline("Estrutura de Dados", [ads.Id]);
        var infoSociedade = await client.CreateDiscipline("Informática e Sociedade", [ads.Id, direito.Id]);

        var chico = await client.CreateTeacher("Chico");
        var ana = await client.CreateTeacher("Ana");

        await client.CreateClass(matematica.Id, chico.Id, period.Id, [new(Day.Segunda, Hour.H07_00, Hour.H10_00)]);
        await client.CreateClass(bancoDeDados.Id, chico.Id, period.Id, [new(Day.Terca, Hour.H07_00, Hour.H10_00)]);
        await client.CreateClass(estruturaDeDados.Id, chico.Id, period.Id, [new(Day.Quarta, Hour.H07_00, Hour.H10_00)]);
        await client.CreateClass(infoSociedade.Id, ana.Id, period.Id, [new(Day.Segunda, Hour.H07_00, Hour.H08_00)]);

        var teacherClient = await _factory.LoggedAsTeacher(chico.Email);

        // Assert
        var agenda = await teacherClient.GetTeacherAgenda();

        agenda.Should().HaveCount(3);
    }
}
