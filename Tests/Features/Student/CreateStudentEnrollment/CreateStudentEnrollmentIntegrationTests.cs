namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_student_enrollment()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();

        var period = await academicClient.CreateAcademicPeriod($"{DateTime.Now.Year}.1");
        await academicClient.CreateEnrollmentPeriod(period.Id);

        var campus = await academicClient.CreateCampus();
        var ads = await academicClient.CreateCourse("ADS");
        var direito = await academicClient.CreateCourse("Direito");

        var math = await academicClient.CreateDiscipline("Matemática Discreta", [ads.Id]);
        var database = await academicClient.CreateDiscipline("Banco de Dados", [ads.Id]);
        var dataStructures = await academicClient.CreateDiscipline("Estrutura de Dados", [ads.Id]);
        var infoSociety = await academicClient.CreateDiscipline("Informática e Sociedade", [ads.Id, direito.Id]);

        var courseCurriculumAds = await academicClient.CreateCourseCurriculum("Grade ADS 1.0", ads.Id,
        [
            new(math.Id, 1, 7, 73),
            new(database.Id, 1, 7, 73),
            new(dataStructures.Id, 2, 7, 73),
            new(infoSociety.Id, 2, 7, 73),
        ]);

        var courseOfferingAds = await academicClient.CreateCourseOffering(campus.Id, ads.Id, courseCurriculumAds.Id, period.Id, Shift.Noturno);

        var chico = await academicClient.CreateTeacher("Chico");
        var ana = await academicClient.CreateTeacher("Ana");

        var mathClass = await academicClient.CreateClass(math.Id, chico.Id, period.Id, 40, [new(Day.Segunda, Hour.H07_00, Hour.H10_00)]);
        var databaseClass = await academicClient.CreateClass(database.Id, chico.Id, period.Id, 40, [new(Day.Terca, Hour.H07_00, Hour.H10_00)]);
        var dataStructuresClass = await academicClient.CreateClass(dataStructures.Id, chico.Id, period.Id, 40, [new(Day.Quarta, Hour.H07_00, Hour.H10_00)]);
        var infoSocietyClass = await academicClient.CreateClass(infoSociety.Id, ana.Id, period.Id, 40, [new(Day.Segunda, Hour.H07_00, Hour.H08_00)]);

        var student = await academicClient.CreateStudent(courseOfferingAds.Id, "Zaqueu");

        var studentClient = await _back.LoggedAsStudent(student.Email);
        var options = await studentClient.GetStudentEnrollmentClasses();
        var selectedClasses = options.Where(x => x.Id == mathClass.Id || x.Id == databaseClass.Id).Select(x => x.Id).ToList();

        // Act
        await studentClient.CreateStudentEnrollment(selectedClasses);

        // Assert
        var classes = await studentClient.GetStudentEnrollmentClasses();

        classes.Should().HaveCount(4);
        classes.First(x => x.Id == mathClass.Id).IsSelected.Should().BeTrue();
        classes.First(x => x.Id == databaseClass.Id).IsSelected.Should().BeTrue();
        classes.First(x => x.Id == dataStructuresClass.Id).IsSelected.Should().BeFalse();
        classes.First(x => x.Id == infoSocietyClass.Id).IsSelected.Should().BeFalse();
    }
}
