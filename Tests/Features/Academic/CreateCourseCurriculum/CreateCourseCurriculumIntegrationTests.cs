namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_course_curriculum_without_disciplines()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CourseOut course = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");

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
        CourseOut course = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");

        var bd = await client.CreateDiscipline("Banco de Dados", [course.Id]);
        var ed = await client.CreateDiscipline("Estrutura de Dados", [course.Id]);
        var poo = await client.CreateDiscipline("Programação Orientada a Objetos", [course.Id]);
        var disciplines = new List<CreateCourseCurriculumDisciplineIn> { new(bd.Id, 1, 10, 70), new(ed.Id, 2, 8, 55), new(poo.Id, 3, 12, 60) };

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
        CourseOut course = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");

        var bd = await client.CreateDiscipline("Banco de Dados", [course.Id]);
        var disciplines = new List<CreateCourseCurriculumDisciplineIn> { new(bd.Id, 1, 10, 80) };

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
        var response = await client.CreateCourseCurriculum("Grade de ADS 1.0", Guid.NewGuid(), []);
        
        // Assert
        response.ShouldBeError(new CourseNotFound());
    }

    [Test]
    public async Task Should_not_create_course_curriculum_with_another_institution_course()
    {
        // Arrange
        var clientNovaRoma = await _api.LoggedAsAcademic();
        var clientUfpe = await _api.LoggedAsAcademic();

        CourseOut course = await clientUfpe.CreateCourse("Análise e Desenvolvimento de Sistemas");

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

        CourseOut ads = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");
        CourseOut direito = await client.CreateCourse("Direito");

        var bd = await client.CreateDiscipline("Banco de Dados", [ads.Id]);

        // Act
        var response = await client.CreateCourseCurriculum("Grade de Direito 1.0", direito.Id, [ new(bd.Id, 1, 10, 70) ]);

        // Assert
        response.ShouldBeError(new InvalidDisciplinesList());
    }

    [Test]
    public async Task Should_not_create_course_curriculum_with_another_institution_discipline()
    {
        // Arrange
        var clientNovaRoma = await _api.LoggedAsAcademic();
        var clientUfpe = await _api.LoggedAsAcademic();

        CourseOut courseNovaRoma = await clientNovaRoma.CreateCourse("Análise e Desenvolvimento de Sistemas");
        await clientNovaRoma.CreateDiscipline("Banco de Dados", [courseNovaRoma.Id]);

        CourseOut courseUfpe = await clientUfpe.CreateCourse("Análise e Desenvolvimento de Sistemas");
        var disciplineUfpe = await clientUfpe.CreateDiscipline("Banco de Dados", [courseUfpe.Id]);

        // Act
        var response = await clientNovaRoma.CreateCourseCurriculum("Grade ADS", courseNovaRoma.Id, [ new(disciplineUfpe.Id, 1, 10, 70) ]);

        // Assert
        response.ShouldBeError(new InvalidDisciplinesList());
    }

    [Test]
    public async Task Should_not_create_course_curriculum_with_repeated_disciplines()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CourseOut course = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");

        var bd = await client.CreateDiscipline("Banco de Dados", [course.Id]);
        var ed = await client.CreateDiscipline("Estrutura de Dados", [course.Id]);
        var poo = await client.CreateDiscipline("Programação Orientada a Objetos", [course.Id]);

        var disciplines = new List<CreateCourseCurriculumDisciplineIn>
        {
            new(bd.Id, 1, 10, 70),
            new(ed.Id, 2, 8, 55),
            new(ed.Id, 2, 8, 55),
            new(poo.Id, 3, 12, 60),
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
        CourseOut course = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");

        var bd = await client.CreateDiscipline("Banco de Dados", [course.Id]);
        var poo = await client.CreateDiscipline("Programação Orientada a Objetos", [course.Id]);
        var disciplines = new List<CreateCourseCurriculumDisciplineIn>
        {
            new(bd.Id, 1, 10, 70),
            new(poo.Id, 3, 12, 60),
            new(Guid.NewGuid(), 2, 9, 55)
        };

        // Act
        var response = await client.CreateCourseCurriculum("Grade de Direito 1.0", course.Id, disciplines);

        // Assert
        response.ShouldBeError(new InvalidDisciplinesList());     
    }
}
