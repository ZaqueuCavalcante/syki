namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Students_GetStudentDetails_Should_not_get_student_details_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetStudentDetails(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Students_GetStudentDetails_Should_not_get_student_details_when_user_is_a_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetStudentDetails(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Students_GetStudentDetails_Should_not_get_student_details_when_user_is_a_student()
    {
        // Arrange
        var directorClient = await _back.LoggedAsDirector();
        var email = DataGen.Email;
        var student = await directorClient.CreateStudent(DataGen.UserName, email).Success();
        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentDetails(student.Id);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Students_GetStudentDetails_Should_not_get_student_details_when_student_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetStudentDetails(999999);

        // Assert
        result.ShouldBeError(StudentNotFound.I);
    }

    [Test]
    public async Task Students_GetStudentDetails_Should_not_get_student_details_of_another_institution()
    {
        // Arrange
        var otherClient = await _back.LoggedAsDirector();
        var student = await otherClient.CreateStudent(DataGen.UserName, DataGen.Email).Success();

        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetStudentDetails(student.Id);

        // Assert
        result.ShouldBeError(StudentNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Students_GetStudentDetails_Should_get_student_details()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var email = DataGen.Email;
        var student = await client.CreateStudent("Ana Lima", email).Success();

        // Act
        var result = await client.GetStudentDetails(student.Id);

        // Assert
        var details = result.Success;
        details.Id.Should().Be(student.Id);
        details.Name.Should().Be("Ana Lima");
        details.Email.Should().Be(email);
        details.EnrollmentCode.Should().NotBeEmpty();
        details.Status.Should().Be(StudentStatus.Enrolled);
        details.Course.Should().BeNull();
        details.Classes.Should().BeEmpty();
    }

    [Test]
    public async Task Students_GetStudentDetails_Should_get_student_details_with_current_course_offering()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var student = await client.CreateStudent(DataGen.UserName, DataGen.Email).Success();
        var campus = await client.CreateCampus().Success();
        var course = await client.CreateCourse().Success();
        var curriculum = await client.CreateCourseCurriculum(course.Id).Success();
        var period = await client.CreateAcademicPeriod().Success();
        var offering = await client.CreateCourseOffering(campus.Id, course.Id, curriculum.Id, period.Id).Success();
        await client.EnrollStudentInCourseOffering(student.Id, offering.Id);

        // Act
        var result = await client.GetStudentDetails(student.Id);

        // Assert
        var details = result.Success;
        details.Course.Should().NotBeNull();
        details.Course!.CourseOfferingId.Should().Be(offering.Id);
        details.Course.Period.Should().Be("2024.1");
    }

    [Test]
    public async Task Students_GetStudentDetails_Should_get_student_details_with_classes()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var student = await client.CreateStudent(DataGen.UserName, DataGen.Email).Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await client.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await client.ReleaseClassForEnrollment(@class.Id);
        await client.AssignStudentToClass(student.Id, @class.Id);

        // Act
        var result = await client.GetStudentDetails(student.Id);

        // Assert
        var details = result.Success;
        details.Classes.Should().ContainSingle();
        details.Classes[0].Id.Should().Be(@class.Id);
        details.Classes[0].Discipline.Should().Be("Geometria");
        details.Classes[0].Period.Should().Be("2024.1");
        details.Classes[0].Status.Should().Be(ClassStatus.OnEnrollment);
        details.Classes[0].MyStatus.Should().Be(StudentClassStatus.Matriculado);
    }

    #endregion
}
