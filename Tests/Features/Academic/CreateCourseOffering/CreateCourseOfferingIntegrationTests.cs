namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_course_offering()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        CourseOut course = await client.CreateCourse("ADS");
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
        var client = await _back.LoggedAsAcademic();

        // Act
        var response = await client.CreateCourseOffering(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "2024.1", Shift.Matutino);

        // Assert
        response.ShouldBeError(new CampusNotFound());      
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
        var response = await clientNovaRoma.CreateCourseOffering(campusUfpe.Id, Guid.NewGuid(), Guid.NewGuid(), "2024.1", Shift.Matutino);

        // Assert
        response.ShouldBeError(new CampusNotFound());      
    }

    [Test]
    public async Task Should_not_create_course_offering_without_course()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");

        // Act
        var response = await client.CreateCourseOffering(campus.Id, Guid.NewGuid(), Guid.NewGuid(), "2024.1", Shift.Matutino);

        // Assert
        response.ShouldBeError(new CourseNotFound());
    }

    [Test]
    public async Task Should_not_create_course_offering_with_other_institution_course()
    {
        // Arrange
        var clientNovaRoma = await _back.LoggedAsAcademic();
        var clientUfpe = await _back.LoggedAsAcademic();

        CourseOut courseUfpe = await clientUfpe.CreateCourse("Direito");
        var campusNovaRoma = await clientNovaRoma.CreateCampus("Agreste I", "Caruaru - PE");

        // Act
        var response = await clientNovaRoma.CreateCourseOffering(campusNovaRoma.Id, courseUfpe.Id, Guid.NewGuid(), "2024.1", Shift.Matutino);

        // Assert
        response.ShouldBeError(new CourseNotFound());
    }

    [Test]
    public async Task Should_not_create_course_offering_without_course_curriculum()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        CourseOut course = await client.CreateCourse("Direito");

        // Act
        var response = await client.CreateCourseOffering(campus.Id, course.Id, Guid.NewGuid(), "2024.1", Shift.Matutino);

        // Assert
        response.ShouldBeError(new CourseCurriculumNotFound());
    }

    [Test]
    public async Task Should_not_create_course_offering_with_other_course_curriculum()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        CourseOut courseAds = await client.CreateCourse("ADS");
        CourseOut courseDireito = await client.CreateCourse("Direito");
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
        var client = await _back.LoggedAsAcademic();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        CourseOut course = await client.CreateCourse("Direito");
        CourseCurriculumOut cc = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);

        // Act
        var response = await client.CreateCourseOffering(campus.Id, course.Id, cc.Id, "2024.1", Shift.Matutino);
        
        // Assert
        response.ShouldBeError(new AcademicPeriodNotFound());
    }

    [Test]
    public async Task Should_not_create_course_offering_with_invalid_shift()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var campus = await client.CreateCampus();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        CourseOut course = await client.CreateCourse();
        CourseCurriculumOut cc = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);

        // Act
        var response = await client.CreateCourseOffering(campus.Id, course.Id, cc.Id, period.Id, (Shift)69);
        
        // Assert
        response.ShouldBeError(new InvalidShift());
    }
    
    [Test]
    public async Task Should_not_create_course_offering_with_other_institution_period()
    {
        // Arrange
        var clientNovaRoma = await _back.LoggedAsAcademic();
        var clientUfpe = await _back.LoggedAsAcademic();

        await clientUfpe.CreateAcademicPeriod("2023.1");

        var campus = await clientNovaRoma.CreateCampus("Agreste I", "Caruaru - PE");
        CourseOut course = await clientNovaRoma.CreateCourse("Direito");
        CourseCurriculumOut cc = await clientNovaRoma.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);

        // Act
        var response = await clientNovaRoma.CreateCourseOffering(campus.Id, course.Id, cc.Id, "2023.1", Shift.Matutino);

        // Assert
        response.ShouldBeError(new AcademicPeriodNotFound());
    }
}
