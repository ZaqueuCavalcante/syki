namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_all_institution_classes()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        var teacher = await client.CreateTeacher();
        var period = await client.CreateAcademicPeriod("2024.1");
        var schedules = new List<ScheduleIn>() { new(Day.Segunda, Hour.H07_00, Hour.H08_00) };

        var @class = await client.CreateClass(discipline.Id, teacher.Id, period.Id, 40, schedules);

        // Act
        var classes = await client.GetClasses();

        // Assert
        classes.Count.Should().Be(1);
        classes[0].Id.Should().Be(@class.Id);
    }

    [Test]
    public async Task Should_return_classes_with_fill_ratio()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        var chico = await academicClient.CreateTeacher("Chico");
        var mathClass = await academicClient.CreateClass(
            data.Disciplines.DiscreteMath.Id,
            chico.Id,
            data.AcademicPeriod.Id,
            40,
            [new(Day.Segunda, Hour.H07_00, Hour.H10_00)]
        );
        var student = await academicClient.CreateStudent(data.CourseOffering.Id, "Zaqueu");

        var studentClient = await _back.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([mathClass.Id]);

        // Act
        var classes = await academicClient.GetClasses();

        // Assert
        classes.Count.Should().Be(1);
        classes[0].FillRatio.Should().Be("1/40");
    }
}
