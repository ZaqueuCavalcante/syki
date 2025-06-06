namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_course_curriculum_without_disciplines()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CourseOut course = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo, []);

        // Act
        CourseCurriculumOut courseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);

        // Assert
        courseCurriculum.Id.Should().NotBeEmpty();
        courseCurriculum.Name.Should().Be("Grade de ADS 1.0");
        courseCurriculum.CourseId.Should().Be(course.Id);
        courseCurriculum.CourseName.Should().Be(course.Name);
        courseCurriculum.Disciplines.Should().HaveCount(0);
    }

    [Test]
    public async Task Should_create_course_curriculum_with_many_disciplines()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CourseOut course = await client.CreateCourse(
            "Análise e Desenvolvimento de Sistemas",
            CourseType.Tecnologo,
            ["Banco de Dados", "Estrutura de Dados", "Programação Orientada a Objetos"]
        );

        var disciplines = new List<CreateCourseCurriculumDisciplineIn>
        {
            new(course.Disciplines[0].Id, 1, 10, 70),
            new(course.Disciplines[1].Id, 2, 8, 55),
            new(course.Disciplines[2].Id, 3, 12, 60)
        };

        // Act
        CourseCurriculumOut courseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id, disciplines);

        // Assert
        courseCurriculum.Id.Should().NotBeEmpty();
        courseCurriculum.Name.Should().Be("Grade de ADS 1.0");
        courseCurriculum.CourseId.Should().Be(course.Id);
        courseCurriculum.CourseName.Should().Be(course.Name);
        courseCurriculum.Disciplines.Should().HaveCount(3);        
    }

    [Test]
    public async Task Should_create_course_curriculum_with_discipline_values()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CourseOut course = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo, ["Banco de Dados"]);

        var disciplines = new List<CreateCourseCurriculumDisciplineIn> { new(course.Disciplines[0].Id, 1, 10, 80) };

        // Act
        CourseCurriculumOut courseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id, disciplines);

        // Assert
        courseCurriculum.Id.Should().NotBeEmpty();
        courseCurriculum.Name.Should().Be("Grade de ADS 1.0");
        courseCurriculum.CourseId.Should().Be(course.Id);
        courseCurriculum.CourseName.Should().Be(course.Name);
        courseCurriculum.Disciplines.Should().HaveCount(1);
        courseCurriculum.Disciplines[0].Period.Should().Be(1);
        courseCurriculum.Disciplines[0].Credits.Should().Be(10);
        courseCurriculum.Disciplines[0].Workload.Should().Be(80);
    }

    [Test]
    public async Task Should_not_create_course_curriculum_without_course()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.CreateCourseCurriculum("Grade de ADS 1.0", Guid.CreateVersion7(), []);
        
        // Assert
        response.ShouldBeError(new CourseNotFound());
    }

    [Test]
    public async Task Should_not_create_course_curriculum_with_another_institution_course()
    {
        // Arrange
        var clientNovaRoma = await _api.LoggedAsAcademic();
        var clientUfpe = await _api.LoggedAsAcademic();

        CourseOut course = await clientUfpe.CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo, []);

        // Act
        var response = await clientNovaRoma.CreateCourseCurriculum("Grade de ADS 1.0", course.Id, []);
        
        // Assert
        response.ShouldBeError(new CourseNotFound());
    }

    [Test]
    public async Task Should_not_create_course_curriculum_with_another_course_disciplines()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        CourseOut ads = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo, ["Banco de Dados"]);
        CourseOut direito = await client.CreateCourse("Direito", CourseType.Bacharelado, []);

        // Act
        var response = await client.CreateCourseCurriculum("Grade de Direito 1.0", direito.Id, [ new(ads.Disciplines[0].Id, 1, 10, 70) ]);

        // Assert
        response.ShouldBeError(new InvalidDisciplinesList());
    }

    [Test]
    public async Task Should_not_create_course_curriculum_with_another_institution_discipline()
    {
        // Arrange
        var clientNovaRoma = await _api.LoggedAsAcademic();
        var clientUfpe = await _api.LoggedAsAcademic();

        CourseOut courseNovaRoma = await clientNovaRoma.CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo, ["Banco de Dados"]);

        CourseOut courseUfpe = await clientUfpe.CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo, ["Banco de Dados"]);

        // Act
        var response = await clientNovaRoma.CreateCourseCurriculum("Grade ADS", courseNovaRoma.Id, [ new(courseUfpe.Disciplines[0].Id, 1, 10, 70) ]);

        // Assert
        response.ShouldBeError(new InvalidDisciplinesList());
    }

    [Test]
    public async Task Should_not_create_course_curriculum_with_repeated_disciplines()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CourseOut course = await client.CreateCourse(
            "Análise e Desenvolvimento de Sistemas",
            CourseType.Tecnologo,
            ["Banco de Dados", "Estrutura de Dados", "Programação Orientada a Objetos"]
        );

        var disciplines = new List<CreateCourseCurriculumDisciplineIn>
        {
            new(course.Disciplines[0].Id, 1, 10, 70),
            new(course.Disciplines[1].Id, 2, 8, 55),
            new(course.Disciplines[1].Id, 2, 8, 55),
            new(course.Disciplines[2].Id, 3, 12, 60),
        };

        // Act
        var response = await client.CreateCourseCurriculum("Grade de Direito 1.0", course.Id, disciplines);

        // Assert
        response.ShouldBeError(new InvalidDisciplinesList());
    }

    [Test]
    public async Task Should_not_create_course_curriculum_with_random_disciplines()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CourseOut course = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo, ["Banco de Dados"]);

        var disciplines = new List<CreateCourseCurriculumDisciplineIn>
        {
            new(course.Disciplines[0].Id, 1, 10, 70),
            new(Guid.CreateVersion7(), 2, 9, 55)
        };

        // Act
        var response = await client.CreateCourseCurriculum("Grade de Direito 1.0", course.Id, disciplines);

        // Assert
        response.ShouldBeError(new InvalidDisciplinesList());     
    }
}
