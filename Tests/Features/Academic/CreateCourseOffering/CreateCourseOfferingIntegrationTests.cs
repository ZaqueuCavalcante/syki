namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_course_offering()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse("ADS");
        var courseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);

        // Act
        var courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Matutino);

        // Assert
        courseOffering.Id.Should().NotBeEmpty();
        courseOffering.CourseCurriculumId.Should().Be(courseCurriculum.Id);
        courseOffering.Period.Should().Be(period.Id);
    }

    [Test]
    public async Task Should_not_create_course_offering_without_campus()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var response = await client.CreateCourseOfferingHttp(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "2024.1", Shift.Matutino);

        // Assert
        await response.AssertBadRequest(Throw.DE010);      
    }

    [Test]
    public async Task Should_not_create_course_offering_with_other_institution_campus()
    {
        // Arrange
        var clientNovaRoma = await _back.LoggedAsAcademic();
        var clientUfpe = await _back.LoggedAsAcademic();

        await clientNovaRoma.CreateCampus("Agreste I", "Caruaru - PE");
        var campusUfpe = await clientUfpe.CreateCampus("Suassuna", "Recife - PE");

        // Act
        var response = await clientNovaRoma.CreateCourseOfferingHttp(campusUfpe.Id, Guid.NewGuid(), Guid.NewGuid(), "2024.1", Shift.Matutino);

        // Assert
        await response.AssertBadRequest(Throw.DE010);      
    }

    [Test]
    public async Task Should_not_create_course_offering_without_course()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");

        // Act
        var response = await client.CreateCourseOfferingHttp(campus.Id, Guid.NewGuid(), Guid.NewGuid(), "2024.1", Shift.Matutino);

        // Assert
        await response.AssertBadRequest(new CourseNotFound().Message);
    }

    [Test]
    public async Task Should_not_create_course_offering_with_other_institution_course()
    {
        // Arrange
        var clientNovaRoma = await _back.LoggedAsAcademic();
        var clientUfpe = await _back.LoggedAsAcademic();

        var courseUfpe = await clientUfpe.CreateCourse("Direito");
        var campusNovaRoma = await clientNovaRoma.CreateCampus("Agreste I", "Caruaru - PE");

        // Act
        var response = await clientNovaRoma.CreateCourseOfferingHttp(campusNovaRoma.Id, courseUfpe.Id, Guid.NewGuid(), "2024.1", Shift.Matutino);

        // Assert
        await response.AssertBadRequest(new CourseNotFound().Message);
    }

    [Test]
    public async Task Should_not_create_course_offering_without_course_curriculum()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var course = await client.CreateCourse("Direito");

        // Act
        var response = await client.CreateCourseOfferingHttp(campus.Id, course.Id, Guid.NewGuid(), "2024.1", Shift.Matutino);

        // Assert
        await response.AssertBadRequest(Throw.DE011);
    }

    [Test]
    public async Task Should_not_create_course_offering_with_other_course_curriculum()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var courseAds = await client.CreateCourse("ADS");
        var courseDireito = await client.CreateCourse("Direito");
        var cc = await client.CreateCourseCurriculum("Grade de ADS 1.0", courseAds.Id);

        // Act
        var response = await client.CreateCourseOfferingHttp(campus.Id, courseDireito.Id, cc.Id, "2024.1", Shift.Matutino);

        // Assert
        await response.AssertBadRequest(Throw.DE011);
    }

    [Test]
    public async Task Should_not_create_course_offering_without_period()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var course = await client.CreateCourse("Direito");
        var cc = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);

        // Act
        var response = await client.CreateCourseOfferingHttp(campus.Id, course.Id, cc.Id, "2024.1", Shift.Matutino);
        
        // Assert
        await response.AssertBadRequest(new AcademicPeriodNotFound().Message);
    }

    [Test]
    public async Task Should_not_create_course_offering_with_other_institution_period()
    {
        // Arrange
        var clientNovaRoma = await _back.LoggedAsAcademic();
        var clientUfpe = await _back.LoggedAsAcademic();

        await clientUfpe.CreateAcademicPeriod("2023.1");

        var campus = await clientNovaRoma.CreateCampus("Agreste I", "Caruaru - PE");
        var course = await clientNovaRoma.CreateCourse("Direito");
        var cc = await clientNovaRoma.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);

        // Act
        var response = await clientNovaRoma.CreateCourseOfferingHttp(campus.Id, course.Id, cc.Id, "2023.1", Shift.Matutino);

        // Assert
        await response.AssertBadRequest(new AcademicPeriodNotFound().Message);
    }
}
