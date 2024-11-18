namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_cross_login_from_academic_to_teacher_account()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        TeacherOut teacher = await client.CreateTeacher();

        // Act
        CrossLoginOut response = await client.Http.CrossLogin(teacher.Id);

        // Assert
        response.AccessToken.Should().StartWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.");
    }

    [Test]
    public async Task Should_cross_login_from_academic_to_student_account()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        StudentOut student = await client.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");

        // Act
        CrossLoginOut response = await client.Http.CrossLogin(student.Id);

        // Assert
        response.AccessToken.Should().StartWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.");
    }

    [Test]
    public async Task Should_not_cross_login_from_academic_to_another_institution_teacher_account()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        await client.CreateTeacher();

        var otherClient = await _api.LoggedAsAcademic();
        TeacherOut otherTeacher = await otherClient.CreateTeacher();

        // Act
        var response = await client.Http.CrossLogin(otherTeacher.Id);

        // Assert
        response.ShouldBeError(new UserNotFound());
    }

    [Test]
    public async Task Should_not_cross_login_from_academic_to_another_institution_student_account()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        await client.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");

        var otherClient = await _api.LoggedAsAcademic();
        var otherData = await otherClient.CreateBasicInstitutionData();
        StudentOut otherStudent = await otherClient.CreateStudent(otherData.AdsCourseOffering.Id, "Zezin");

        // Act
        var response = await client.Http.CrossLogin(otherStudent.Id);

        // Assert
        response.ShouldBeError(new UserNotFound());
    }

    [Test]
    public async Task Should_not_cross_login_from_teacher_to_academic_account()
    {
        // Arrange
        var client = _api.GetClient();
        var academicUser = await client.RegisterUser(_api);
        await client.Login(academicUser.Email, academicUser.Password);
        var academicClient = new AcademicHttpClient(client);

        TeacherOut teacher = await academicClient.CreateTeacher();
        var teacherClient = await _api.LoggedAsTeacher(teacher.Email);

        // Act
        var response = await teacherClient.Cross.CrossLogin(academicUser.Id);

        // Assert
        response.ShouldBeError(new ForbiddenErrorOut());
    }

    [Test]
    public async Task Should_not_cross_login_from_student_to_academic_account()
    {
        // Arrange
        var client = _api.GetClient();
        var academicUser = await client.RegisterUser(_api);
        await client.Login(academicUser.Email, academicUser.Password);
        var academicClient = new AcademicHttpClient(client);

        var data = await academicClient.CreateBasicInstitutionData();
        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // Act
        var response = await studentClient.Http.CrossLogin(academicUser.Id);

        // Assert
        response.ShouldBeError(new ForbiddenErrorOut());
    }
}
