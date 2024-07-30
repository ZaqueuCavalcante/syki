namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_add_exam_grade_note()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();

        var period = await academicClient.CreateAcademicPeriod($"{DateTime.Now.Year}.1");
        await academicClient.CreateEnrollmentPeriod(period.Id);

        var campus = await academicClient.CreateCampus();
        var ads = await academicClient.CreateCourse();
        var math = await academicClient.CreateDiscipline("Matemática Discreta", [ads.Id]);
        var courseCurriculumAds = await academicClient.CreateCourseCurriculum("Grade ADS 1.0", ads.Id, [ new(math.Id, 1, 7, 73) ]);
        var courseOfferingAds = await academicClient.CreateCourseOffering(campus.Id, ads.Id, courseCurriculumAds.Id, period.Id, Shift.Noturno);

        var chico = await academicClient.CreateTeacher("Chico");
        var student = await academicClient.CreateStudent(courseOfferingAds.Id, "Zaqueu");
        var mathClass = await academicClient.CreateClass(math.Id, chico.Id, period.Id, 40, [ new(Day.Segunda, Hour.H07_00, Hour.H10_00) ]);

        var studentClient = await _back.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([ mathClass.Id ]);

        var teacherClient = await _back.LoggedAsTeacher(chico.Email);
        var @class = await teacherClient.GetTeacherClass(mathClass.Id);
        var examGradeId = @class.Students.First().ExamGrades.First().Id;

        // Act
        var response = await teacherClient.AddExamGradeNote(examGradeId, 5.67M);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        var classAfter = await teacherClient.GetTeacherClass(mathClass.Id);
        var examGrades = classAfter.Students.First().ExamGrades;
        examGrades.First(x => x.ExamType == ExamType.N1).Note.Should().Be(5.67M);
        examGrades.First(x => x.ExamType == ExamType.N2).Note.Should().Be(0);
        examGrades.First(x => x.ExamType == ExamType.N3).Note.Should().Be(0);
    }
}
