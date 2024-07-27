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

        var period = await academicClient.CreateAcademicPeriod($"{DateTime.Now.Year}.1");
        await academicClient.CreateEnrollmentPeriod(period.Id);

        var campus = await academicClient.CreateCampus("Agreste I", "Caruaru - PE");
        var ads = await academicClient.CreateCourse("ADS");

        var math = await academicClient.CreateDiscipline("Matemática Discreta", [ads.Id]);
        var courseCurriculumAds = await academicClient.CreateCourseCurriculum("Grade ADS 1.0", ads.Id,
        [
            new(math.Id, 1, 7, 73),
        ]);

        var courseOfferingAds = await academicClient.CreateCourseOffering(campus.Id, ads.Id, courseCurriculumAds.Id, period.Id, Shift.Noturno);
        var chico = await academicClient.CreateTeacher("Chico");
        var mathClass = await academicClient.CreateClass(math.Id, chico.Id, period.Id, 40, [new(Day.Segunda, Hour.H07_00, Hour.H10_00)]);
        var student = await academicClient.CreateStudent(courseOfferingAds.Id, "Zaqueu");

        var studentClient = await _back.LoggedAsStudent(student.Email);
        var options = await studentClient.GetStudentEnrollmentClasses();
        var selectedClasses = options.Where(x => x.Id == mathClass.Id).Select(x => x.Id).ToList();

        await studentClient.CreateStudentEnrollment(selectedClasses);

        // Act
        var classes = await academicClient.GetClasses();

        // Assert
        classes.Count.Should().Be(1);
        classes[0].FillRatio.Should().Be("1/40");
    }
}
