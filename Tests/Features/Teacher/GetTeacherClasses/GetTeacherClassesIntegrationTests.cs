namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_teacher_classes()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var period = await client.CreateAcademicPeriod($"{DateTime.Now.Year}.1");
        await client.CreateEnrollmentPeriod(period.Id);

        var ads = await client.CreateCourse("ADS");
        var direito = await client.CreateCourse("Direito");

        var math = await client.CreateDiscipline("Matemática Discreta", [ads.Id]);
        var db = await client.CreateDiscipline("Banco de Dados", [ads.Id]);
        var ds = await client.CreateDiscipline("Estrutura de Dados", [ads.Id]);
        var infoSociedade = await client.CreateDiscipline("Informática e Sociedade", [ads.Id, direito.Id]);

        var chico = await client.CreateTeacher("Chico");
        var ana = await client.CreateTeacher("Ana");

        await client.CreateClass(math.Id, chico.Id, period.Id, 40, [new(Day.Segunda, Hour.H07_00, Hour.H10_00)]);
        await client.CreateClass(db.Id, chico.Id, period.Id, 40, [new(Day.Terca, Hour.H07_00, Hour.H10_00)]);
        await client.CreateClass(ds.Id, chico.Id, period.Id, 40, [new(Day.Quarta, Hour.H07_00, Hour.H10_00)]);
        await client.CreateClass(infoSociedade.Id, ana.Id, period.Id, 40, [new(Day.Segunda, Hour.H07_00, Hour.H08_00)]);

        var teacherClient = await _back.LoggedAsTeacher(chico.Email);

        // Act
        var classes = await teacherClient.GetTeacherClasses();

        // Assert
        classes.Should().HaveCount(3);
        classes[0].Discipline.Should().Be(db.Name);
        classes[1].Discipline.Should().Be(ds.Name);
        classes[2].Discipline.Should().Be(math.Name);
    }

    [Test]
    public async Task Should_not_return_other_teacher_classes()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var period = await client.CreateAcademicPeriod($"{DateTime.Now.Year}.1");
        await client.CreateEnrollmentPeriod(period.Id);

        var ads = await client.CreateCourse("ADS");
        var direito = await client.CreateCourse("Direito");

        var infoSociedade = await client.CreateDiscipline("Informática e Sociedade", [ads.Id, direito.Id]);

        var chico = await client.CreateTeacher("Chico");
        var ana = await client.CreateTeacher("Ana");

        await client.CreateClass(infoSociedade.Id, ana.Id, period.Id, 40, [new(Day.Segunda, Hour.H07_00, Hour.H08_00)]);

        var teacherClient = await _back.LoggedAsTeacher(chico.Email);

        // Act
        var classes = await teacherClient.GetTeacherClasses();

        // Assert
        classes.Should().BeEmpty();
    }
}
