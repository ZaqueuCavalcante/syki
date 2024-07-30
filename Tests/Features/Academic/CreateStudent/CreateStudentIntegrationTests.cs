namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_student_with_course_offering()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var campus = await client.CreateCampus();
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse();
        var courseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        // Act
        var response = await client.CreateStudent(courseOffering.Id);

        // Assert
        response.Id.Should().NotBeEmpty(); 
        response.CourseOfferingId.Should().Be(courseOffering.Id); 
        response.Name.Should().Be("Zezin"); 
    }

    [Test]
    public async Task Should_not_create_student_without_course_offering()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var (_, response) = await client.CreateStudentTuple(Guid.NewGuid());

        // Assert
        await response.AssertBadRequest(new CourseOfferingNotFound());
    }

    [Test]
    public async Task Should_not_create_student_with_invalid_email()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var campus = await client.CreateCampus();
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse();
        var courseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        var email = TestData.InvalidEmailsList.OrderBy(_ => Guid.NewGuid()).First();
        
        // Act
        var (_, response) = await client.CreateStudentTuple(courseOffering.Id, "Zezin", email);

        // Assert
        await response.AssertBadRequest(new InvalidEmail()); 
    }

    [Test]
    public async Task Should_not_create_student_with_duplicated_email()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var campus = await client.CreateCampus();
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse();
        var courseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        // Act
        var email = TestData.Email;
        var (_, firstResponse) = await client.CreateStudentTuple(courseOffering.Id, "Zezin", email);
        var (_, secondResponse) = await client.CreateStudentTuple(courseOffering.Id, "Zezin", email);

        // Assert
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        await secondResponse.AssertBadRequest(new EmailAlreadyUsed());
    }

    [Test]
    public async Task Should_create_student_only_with_student_role()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var campus = await client.CreateCampus();
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse();
        var courseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        // Act
        var student = await client.CreateStudent(courseOffering.Id);

        // Assert
        using var userManager = _back.GetUserManager();
        var user = await userManager.FindByEmailAsync(student.Email);
        var isOnlyInStudentRole = await userManager.IsOnlyInRole(user!, UserRole.Student);
        isOnlyInStudentRole.Should().BeTrue();
    }
}
