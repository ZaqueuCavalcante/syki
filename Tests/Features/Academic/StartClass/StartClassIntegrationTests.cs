namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_start_class()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();

        var period = await academicClient.CreateAcademicPeriod($"{DateTime.Now.Year}.1");
        await academicClient.CreateEnrollmentPeriod(period.Id);

        var campus = await academicClient.CreateCampus("Agreste I", "Caruaru - PE");
        var ads = await academicClient.CreateCourse("ADS");
        var math = await academicClient.CreateDiscipline("Matemática Discreta", [ads.Id]);
        var courseCurriculumAds = await academicClient.CreateCourseCurriculum("Grade ADS 1.0", ads.Id, [ new(math.Id, 1, 7, 73) ]);
        var courseOfferingAds = await academicClient.CreateCourseOffering(campus.Id, ads.Id, courseCurriculumAds.Id, period.Id, Shift.Noturno);

        var chico = await academicClient.CreateTeacher("Chico");
        var student = await academicClient.CreateStudent(courseOfferingAds.Id, "Zaqueu");
        var mathClass = await academicClient.CreateClass(math.Id, chico.Id, period.Id, 40, [ new(Day.Segunda, Hour.H07_00, Hour.H10_00) ]);

        var studentClient = await _back.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([ mathClass.Id ]);

        // Act
        await academicClient.StartClass(mathClass.Id);

        // Assert
        using var ctx = _back.GetDbContext();
        var examGrades = await ctx.ExamGrades.Where(x => x.ClassId == mathClass.Id).ToListAsync();

        examGrades.Should().HaveCount(3);
        examGrades.Should().AllSatisfy(x => x.StudentId.Should().Be(student.Id));
        examGrades.Count(x => x.ExamType == ExamType.N1).Should().Be(1);
        examGrades.Count(x => x.ExamType == ExamType.N2).Should().Be(1);
        examGrades.Count(x => x.ExamType == ExamType.N3).Should().Be(1);
        examGrades.Should().AllSatisfy(x => x.Note.Should().Be(0));
    }

    [Test]
    public async Task Should_not_start_not_founded_class()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();

        // Act
        var response = await academicClient.StartClass(Guid.NewGuid());

        // Assert
        await response.AssertBadRequest(new ClassNotFound());
    }
}
