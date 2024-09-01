namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_academic_class()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        TeacherOut teacher = await client.CreateTeacher();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        var schedules = new List<ScheduleIn>() { new(Day.Monday, Hour.H07_00, Hour.H08_00) };

        ClassOut createdClass = await client.CreateClass(discipline.Id, teacher.Id, period.Id, 40, schedules);

        // Act
        GetAcademicClassOut @class = await client.GetAcademicClass(createdClass.Id);

        // Assert
        @class.Discipline.Should().Be(discipline.Name);
    }

    [Test]
    public async Task Should_return_academic_class_with_fill_ratio()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        ClassOut mathClass = await academicClient.CreateClass(
            data.Disciplines.DiscreteMath.Id,
            chico.Id,
            data.AcademicPeriod2.Id,
            40,
            [new(Day.Monday, Hour.H07_00, Hour.H10_00)]
        );
        StudentOut student = await academicClient.CreateStudent(data.CourseOffering.Id, "Zaqueu");

        var studentClient = await _back.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([mathClass.Id]);

        // Act
        GetAcademicClassOut @class = await academicClient.GetAcademicClass(mathClass.Id);

        // Assert
        @class.FillRatio.Should().Be("1/40");
    }

    [Test]
    public async Task Should_not_get_random_academic_class()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var response = await client.GetAcademicClass(Guid.NewGuid());

        // Assert
        response.ShouldBeError(new ClassNotFound());
    }
}
