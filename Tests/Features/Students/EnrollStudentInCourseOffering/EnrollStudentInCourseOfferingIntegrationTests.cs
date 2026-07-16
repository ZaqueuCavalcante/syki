namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Students_EnrollStudentInCourseOffering_Should_not_enroll_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.EnrollStudentInCourseOffering(1, 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Students_EnrollStudentInCourseOffering_Should_not_enroll_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.EnrollStudentInCourseOffering(1, 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Students_EnrollStudentInCourseOffering_Should_not_enroll_when_student_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = (await client.CreateCampus()).Success;
        var course = (await client.CreateCourse()).Success;
        var curriculum = (await client.CreateCourseCurriculum(course.Id)).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var offering = (await client.CreateCourseOffering(campus.Id, course.Id, curriculum.Id, period.Id)).Success;

        // Act
        var result = await client.EnrollStudentInCourseOffering(999999, offering.Id);

        // Assert
        result.ShouldBeError(StudentNotFound.I);
    }

    [Test]
    public async Task Students_EnrollStudentInCourseOffering_Should_not_enroll_when_course_offering_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var student = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success;

        // Act
        var result = await client.EnrollStudentInCourseOffering(student.Id, 999999);

        // Assert
        result.ShouldBeError(CourseOfferingNotFound.I);
    }

    [Test]
    public async Task Students_EnrollStudentInCourseOffering_Should_not_enroll_when_student_already_enrolled()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var student = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success;
        var campus = (await client.CreateCampus()).Success;
        var course = (await client.CreateCourse()).Success;
        var curriculum = (await client.CreateCourseCurriculum(course.Id)).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var offering = (await client.CreateCourseOffering(campus.Id, course.Id, curriculum.Id, period.Id)).Success;
        await client.EnrollStudentInCourseOffering(student.Id, offering.Id);

        // Act
        var result = await client.EnrollStudentInCourseOffering(student.Id, offering.Id);

        // Assert
        result.ShouldBeError(StudentAlreadyEnrolledInCourseOffering.I);
    }

    [Test]
    public async Task Students_EnrollStudentInCourseOffering_Should_not_enroll_student_from_another_institution()
    {
        // Arrange
        var otherClient = await _back.LoggedAsDirector();
        var student = (await otherClient.CreateStudent(DataGen.UserName, DataGen.Email)).Success;

        var client = await _back.LoggedAsDirector();
        var campus = (await client.CreateCampus()).Success;
        var course = (await client.CreateCourse()).Success;
        var curriculum = (await client.CreateCourseCurriculum(course.Id)).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var offering = (await client.CreateCourseOffering(campus.Id, course.Id, curriculum.Id, period.Id)).Success;

        // Act
        var result = await client.EnrollStudentInCourseOffering(student.Id, offering.Id);

        // Assert
        result.ShouldBeError(StudentNotFound.I);
    }

    [Test]
    public async Task Students_EnrollStudentInCourseOffering_Should_not_enroll_in_course_offering_from_another_institution()
    {
        // Arrange
        var otherClient = await _back.LoggedAsDirector();
        var campus = (await otherClient.CreateCampus()).Success;
        var course = (await otherClient.CreateCourse()).Success;
        var curriculum = (await otherClient.CreateCourseCurriculum(course.Id)).Success;
        var period = (await otherClient.CreateAcademicPeriod()).Success;
        var offering = (await otherClient.CreateCourseOffering(campus.Id, course.Id, curriculum.Id, period.Id)).Success;

        var client = await _back.LoggedAsDirector();
        var student = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success;

        // Act
        var result = await client.EnrollStudentInCourseOffering(student.Id, offering.Id);

        // Assert
        result.ShouldBeError(CourseOfferingNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Students_EnrollStudentInCourseOffering_Should_enroll_student_in_course_offering()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var student = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success;
        var campus = (await client.CreateCampus()).Success;
        var course = (await client.CreateCourse()).Success;
        var curriculum = (await client.CreateCourseCurriculum(course.Id)).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var offering = (await client.CreateCourseOffering(campus.Id, course.Id, curriculum.Id, period.Id)).Success;

        // Act
        var result = await client.EnrollStudentInCourseOffering(student.Id, offering.Id);

        // Assert
        result.Success.Id.Should().BePositive();

        await using var db = _back.GetDbContext();
        var enrollment = await db.StudentCourseEnrollments.FirstAsync(x => x.StudentId == student.Id && x.CourseOfferingId == offering.Id);
        enrollment.EnrolledAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        enrollment.LeftAt.Should().BeNull();
    }

    #endregion
}
