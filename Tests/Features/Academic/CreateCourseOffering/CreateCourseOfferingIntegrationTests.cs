namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_course_offering()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        CampusOut campus = await client.CreateCampus();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        CourseOut course = await client.CreateCourse("ADS", CourseType.Tecnologo, []);
        CourseCurriculumOut courseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);

        // Act
        CourseOfferingOut courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Matutino);

        // Assert
        courseOffering.Id.Should().NotBeEmpty();
        courseOffering.CourseCurriculumId.Should().Be(courseCurriculum.Id);
        courseOffering.Period.Should().Be(period.Id);
    }

    [Test]
    public async Task Should_not_create_course_offering_without_campus()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.CreateCourseOffering(Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(), "2024.1", Shift.Matutino);

        // Assert
        response.ShouldBeError(new CampusNotFound());      
    }

    [Test]
    public async Task Should_not_create_course_offering_with_other_institution_campus()
    {
        // Arrange
        var clientNovaRoma = await _api.LoggedAsAcademic();
        var clientUfpe = await _api.LoggedAsAcademic();

        await clientNovaRoma.CreateCampus();
        CampusOut campusUfpe = await clientUfpe.CreateCampus();

        // Act
        var response = await clientNovaRoma.CreateCourseOffering(campusUfpe.Id, Guid.CreateVersion7(), Guid.CreateVersion7(), "2024.1", Shift.Matutino);

        // Assert
        response.ShouldBeError(new CampusNotFound());      
    }

    [Test]
    public async Task Should_not_create_course_offering_without_course()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CampusOut campus = await client.CreateCampus();

        // Act
        var response = await client.CreateCourseOffering(campus.Id, Guid.CreateVersion7(), Guid.CreateVersion7(), "2024.1", Shift.Matutino);

        // Assert
        response.ShouldBeError(new CourseNotFound());
    }

    [Test]
    public async Task Should_not_create_course_offering_with_other_institution_course()
    {
        // Arrange
        var clientNovaRoma = await _api.LoggedAsAcademic();
        var clientUfpe = await _api.LoggedAsAcademic();

        CourseOut courseUfpe = await clientUfpe.CreateCourse("Direito", CourseType.Bacharelado, []);
        CampusOut campusNovaRoma = await clientNovaRoma.CreateCampus();

        // Act
        var response = await clientNovaRoma.CreateCourseOffering(campusNovaRoma.Id, courseUfpe.Id, Guid.CreateVersion7(), "2024.1", Shift.Matutino);

        // Assert
        response.ShouldBeError(new CourseNotFound());
    }

    [Test]
    public async Task Should_not_create_course_offering_without_course_curriculum()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CampusOut campus = await client.CreateCampus();
        CourseOut course = await client.CreateCourse("Direito", CourseType.Bacharelado, []);

        // Act
        var response = await client.CreateCourseOffering(campus.Id, course.Id, Guid.CreateVersion7(), "2024.1", Shift.Matutino);

        // Assert
        response.ShouldBeError(new CourseCurriculumNotFound());
    }

    [Test]
    public async Task Should_not_create_course_offering_with_other_course_curriculum()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CampusOut campus = await client.CreateCampus();
        CourseOut courseAds = await client.CreateCourse("ADS", CourseType.Tecnologo, []);
        CourseOut courseDireito = await client.CreateCourse("Direito", CourseType.Bacharelado, []);
        CourseCurriculumOut cc = await client.CreateCourseCurriculum("Grade de ADS 1.0", courseAds.Id);

        // Act
        var response = await client.CreateCourseOffering(campus.Id, courseDireito.Id, cc.Id, "2024.1", Shift.Matutino);

        // Assert
        response.ShouldBeError(new CourseCurriculumNotFound());
    }

    [Test]
    public async Task Should_not_create_course_offering_without_period()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CampusOut campus = await client.CreateCampus();
        CourseOut course = await client.CreateCourse("Direito", CourseType.Bacharelado, []);
        CourseCurriculumOut cc = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);

        // Act
        var response = await client.CreateCourseOffering(campus.Id, course.Id, cc.Id, "2024.1", Shift.Matutino);
        
        // Assert
        response.ShouldBeError(new AcademicPeriodNotFound());
    }

    [Test]
    [TestCase(null)]
    [TestCase((Shift)69)]
    public async Task Should_not_create_course_offering_with_invalid_shift(Shift? shift)
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CampusOut campus = await client.CreateCampus();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        CourseOut course = await client.CreateCourse("ADS", CourseType.Tecnologo, []);
        CourseCurriculumOut cc = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);

        // Act
        var response = await client.CreateCourseOffering(campus.Id, course.Id, cc.Id, period.Id, shift);

        // Assert
        response.ShouldBeError(new InvalidShift());
    }
    
    [Test]
    public async Task Should_not_create_course_offering_with_other_institution_period()
    {
        // Arrange
        var clientNovaRoma = await _api.LoggedAsAcademic();
        var clientUfpe = await _api.LoggedAsAcademic();

        await clientUfpe.CreateAcademicPeriod("2023.1");

        CampusOut campus = await clientNovaRoma.CreateCampus();
        CourseOut course = await clientNovaRoma.CreateCourse("Direito", CourseType.Bacharelado, []);
        CourseCurriculumOut cc = await clientNovaRoma.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);

        // Act
        var response = await clientNovaRoma.CreateCourseOffering(campus.Id, course.Id, cc.Id, "2023.1", Shift.Matutino);

        // Assert
        response.ShouldBeError(new AcademicPeriodNotFound());
    }
}
