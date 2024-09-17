namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_all_institution_academic_classes()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        TeacherOut teacher = await client.CreateTeacher();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        var schedules = new List<ScheduleIn>() { new(Day.Monday, Hour.H07_00, Hour.H08_00) };

        ClassOut @class = await client.CreateClass(discipline.Id, teacher.Id, period.Id, 40, schedules);

        // Act
        var classes = await client.GetAcademicClasses();

        // Assert
        classes.Count.Should().Be(1);
        classes[0].Id.Should().Be(@class.Id);
    }

    [Test]
    public async Task Should_return_academic_classes_with_fill_ratio()
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
        var classes = await academicClient.GetAcademicClasses();

        // Assert
        classes.Count.Should().Be(1);
        classes[0].FillRatio.Should().Be("1/40");
    }

    [Test]
    public async Task Should_return_academic_classes_filtered_by_status_when_result_is_empty()
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
        var classes = await academicClient.GetAcademicClasses(new() { Status = ClassStatus.Started });

        // Assert
        classes.Should().BeEmpty();
    }
}