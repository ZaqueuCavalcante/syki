namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_class_activity()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        await academicClient.ReleaseClassesForEnrollment([mathClass.Id]);

        var studentClient = await _api.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([mathClass.Id]);

        await academicClient.UpdateEnrollmentPeriod(period, -2, -1);
        await academicClient.StartClasses([mathClass.Id]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        var title = "Modelagem de Banco de Dados";
        var description = "Modele um banco de dados para uma barbearia.";
        var dueDate = DateTime.Now.AddDays(15);

        // Act
        var response = await teacherClient.CreateClassActivity(mathClass.Id, title, description, dueDate);

        // Assert
        response.ShouldBeSuccess();
    }

    [Test]
    public async Task Should_not_create_class_activity_when_class_not_found()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        // Act
        var response = await teacherClient.CreateClassActivity(Guid.NewGuid(), "", "", DateTime.Now);

        // Assert
        response.ShouldBeError(new ClassNotFound());
    }
}
