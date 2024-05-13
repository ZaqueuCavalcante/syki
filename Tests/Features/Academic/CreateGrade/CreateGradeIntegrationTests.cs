namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_uma_nova_grade_mesmo_sem_disciplines()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var curso = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");

        // Act
        var grade = await client.CreateGrade("Grade de ADS 1.0", curso.Id);

        // Assert
        grade.Id.Should().NotBeEmpty();
        grade.Name.Should().Be("Grade de ADS 1.0");
        grade.CourseId.Should().Be(curso.Id);
        grade.CourseName.Should().Be(curso.Name);
        grade.Disciplines.Should().HaveCount(0);        
    }

    [Test]
    public async Task Deve_criar_uma_nova_grade_com_disciplines()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var curso = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");

        var bd = await client.CreateDiscipline("Banco de Dados", [curso.Id]);
        var ed = await client.CreateDiscipline("Estrutura de Dados", [curso.Id]);
        var poo = await client.CreateDiscipline("Programação Orientada a Objetos", [curso.Id]);
        var disciplines = new List<CreateCourseCurriculumDisciplineIn> { new(bd.Id, 1, 10, 70), new(ed.Id, 2, 8, 55), new(poo.Id, 3, 12, 60) };

        // Act
        var grade = await client.CreateGrade("Grade de ADS 1.0", curso.Id, disciplines);

        // Assert
        grade.Id.Should().NotBeEmpty();
        grade.Name.Should().Be("Grade de ADS 1.0");
        grade.CourseId.Should().Be(curso.Id);
        grade.CourseName.Should().Be(curso.Name);
        grade.Disciplines.Should().HaveCount(3);        
    }

    [Test]
    public async Task Deve_criar_uma_nova_grade_com_disciplines_que_tem_carga_horaria_diferente_da_padrao()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var curso = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");

        var bd = await client.CreateDiscipline("Banco de Dados", [curso.Id]);
        var disciplines = new List<CreateCourseCurriculumDisciplineIn> { new(bd.Id, 1, 10, 80) };

        // Act
        var grade = await client.CreateGrade("Grade de ADS 1.0", curso.Id, disciplines);

        // Assert
        grade.Id.Should().NotBeEmpty();
        grade.Name.Should().Be("Grade de ADS 1.0");
        grade.CourseId.Should().Be(curso.Id);
        grade.CourseName.Should().Be(curso.Name);
        grade.Disciplines.Should().HaveCount(1);
        grade.Disciplines[0].Workload.Should().Be(80);
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_grade_sem_vinculo_com_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var response = await client.CreateGradeHttp("Grade de ADS 1.0", Guid.NewGuid(), []);
        
        // Assert
        await response.AssertBadRequest(Throw.DE002);
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_grade_com_curso_de_outra_institution()
    {
        // Arrange
        var clientNovaRoma = await _factory.LoggedAsAcademic();
        var clientUfpe = await _factory.LoggedAsAcademic();

        var curso = await clientUfpe.CreateCourse("Análise e Desenvolvimento de Sistemas");

        // Act
        var response = await clientNovaRoma.CreateGradeHttp("Grade de ADS 1.0", curso.Id, []);
        
        // Assert
        await response.AssertBadRequest(Throw.DE002);
    }

    [Test]
    public async Task Nao_deve_criar_uma_grade_vinculando_disciplines_que_nao_sao_do_curso_escolhido()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var ads = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");
        var direito = await client.CreateCourse("Direito");

        var bd = await client.CreateDiscipline("Banco de Dados", [ads.Id]);

        // Act
        var response = await client.CreateGradeHttp("Grade de Direito 1.0", direito.Id, [ new(bd.Id, 1, 10, 70) ]);

        // Assert
        await response.AssertBadRequest(Throw.DE003);
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_grade_com_discipline_de_outra_institution()
    {
        // Arrange
        var clientNovaRoma = await _factory.LoggedAsAcademic();
        var clientUfpe = await _factory.LoggedAsAcademic();

        var cursoNovaRoma = await clientNovaRoma.CreateCourse("Análise e Desenvolvimento de Sistemas");
        await clientNovaRoma.CreateDiscipline("Banco de Dados", [cursoNovaRoma.Id]);

        var cursoUfpe = await clientUfpe.CreateCourse("Análise e Desenvolvimento de Sistemas");
        var disciplineUfpe = await clientUfpe.CreateDiscipline("Banco de Dados", [cursoUfpe.Id]);

        // Act
        var response = await clientNovaRoma.CreateGradeHttp("Grade ADS", cursoNovaRoma.Id, [ new(disciplineUfpe.Id, 1, 10, 70) ]);

        // Assert
        await response.AssertBadRequest(Throw.DE003);
    }

    [Test]
    public async Task Nao_deve_criar_uma_grade_com_disciplines_repetidas()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var curso = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");

        var bd = await client.CreateDiscipline("Banco de Dados", [curso.Id]);
        var ed = await client.CreateDiscipline("Estrutura de Dados", [curso.Id]);
        var poo = await client.CreateDiscipline("Programação Orientada a Objetos", [curso.Id]);

        var disciplines = new List<CreateCourseCurriculumDisciplineIn>
        {
            new(bd.Id, 1, 10, 70),
            new(ed.Id, 2, 8, 55),
            new(ed.Id, 2, 8, 55),
            new(poo.Id, 3, 12, 60),
        };

        // Act
        var response = await client.CreateGradeHttp("Grade de Direito 1.0", curso.Id, disciplines);

        // Assert
        await response.AssertBadRequest(Throw.DE003);
    }

    [Test]
    public async Task Nao_deve_criar_uma_grade_com_disciplines_que_nao_existem()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var curso = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");

        var bd = await client.CreateDiscipline("Banco de Dados", [curso.Id]);
        var poo = await client.CreateDiscipline("Programação Orientada a Objetos", [curso.Id]);
        var disciplines = new List<CreateCourseCurriculumDisciplineIn>
        {
            new(bd.Id, 1, 10, 70),
            new(poo.Id, 3, 12, 60),
            new(Guid.NewGuid(), 2, 9, 55)
        };

        // Act
        var response = await client.CreateGradeHttp("Grade de Direito 1.0", curso.Id, disciplines);

        // Assert
        await response.AssertBadRequest(Throw.DE003);     
    }

    [Test, Ignore("")]
    public async Task Deve_retornar_todas_as_grades_apenas_daquela_institution()
    {
        // Arrange
        var clientNovaRoma = await _factory.LoggedAsAcademic();
        var clientUfpe = await _factory.LoggedAsAcademic();

        var cursoNovaRoma = await clientNovaRoma.CreateCourse("Análise e Desenvolvimento de Sistemas");
        var gradeNovaRoma = await clientNovaRoma.CreateGrade("NR - Grade de ADS - 1.0", cursoNovaRoma.Id);

        var cursoUfpe = await clientUfpe.CreateCourse("Análise e Desenvolvimento de Sistemas");
        await clientUfpe.CreateGrade("UFPE - Grade de ADS - 1.0", cursoUfpe.Id);

        // Act
        var grades = await clientNovaRoma.GetAsync<List<CourseCurriculumOut>>("/grades");

        // Assert
        grades.Should().HaveCount(1);
        grades[0].Id.Should().Be(gradeNovaRoma.Id);
        grades[0].Name.Should().Be(gradeNovaRoma.Name);
    }

    [Test, Ignore("")]
    public async Task Deve_retornar_todas_as_grades_ordenadas_por_nome()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var direito = await client.CreateCourse("Direito");
        var gradeDireito = await client.CreateGrade("Grade de Direito 1.0", direito.Id);

        var ads = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");
        var gradeAds = await client.CreateGrade("Grade de ADS 1.0", ads.Id);

        // Act
        var grades = await client.GetAsync<List<CourseCurriculumOut>>("/grades");

        // Assert
        grades.Should().HaveCount(2);
        grades[0].Id.Should().Be(gradeAds.Id);
        grades[0].Name.Should().Be(gradeAds.Name);
        grades[1].Id.Should().Be(gradeDireito.Id);
        grades[1].Name.Should().Be(gradeDireito.Name);
    }
}
