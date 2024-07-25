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

        var teacherClient = await _back.LoggedAsTeacher(chico.Email);

        // Act
        var @class = await teacherClient.GetTeacherClass(mathClass.Id);

        // Assert
        @class.Discipline.Should().Be(math.Name);
        @class.Code.Should().Be(math.Code);
        @class.Period.Should().Be(period.Id);
        @class.Status.Should().Be(ClassStatus.OnEnrollmentPeriod);

        @class.Students.Should().HaveCount(1);
        var examGrades = @class.Students.First().ExamGrades;
        examGrades.Should().AllSatisfy(x => x.StudentId.Should().Be(student.Id));
        examGrades.Count(x => x.ExamType == ExamType.N1).Should().Be(1);
        examGrades.Count(x => x.ExamType == ExamType.N2).Should().Be(1);
        examGrades.Count(x => x.ExamType == ExamType.Final).Should().Be(1);
        examGrades.Should().AllSatisfy(x => x.Note.Should().Be(0));
    }
}
