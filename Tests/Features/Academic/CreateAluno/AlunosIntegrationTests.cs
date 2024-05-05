namespace Syki.Tests.Integration;

[Parallelizable(ParallelScope.All)]
public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_aluno_o_vinculando_com_uma_oferta()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.CreateCurso("ADS");
        var grade = await client.CreateGrade("Grade de ADS 1.0", curso.Id);
        var oferta = await client.CreateOferta(campus.Id, curso.Id, grade.Id, periodo.Id, Turno.Noturno);

        // Act
        var response = await client.CreateAluno(oferta.Id);

        // Assert
        response.Id.Should().NotBeEmpty(); 
        response.OfertaId.Should().Be(oferta.Id); 
        response.Name.Should().Be("Zezin"); 
    }

    [Test]
    public async Task Nao_deve_criar_um_aluno_sem_vinculo_com_oferta()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        var response = await client.CreateAlunoHttp(Guid.NewGuid());

        // Assert
        await response.AssertBadRequest(Throw.DE012);
    }

    [Test]
    public async Task Deve_retornar_as_disciplinas_cursadas_pelo_aluno()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.CreateCurso("ADS");

        var disciplina01 = await client.CreateDisciplina("Banco de Dados", [curso.Id]);
        var disciplina02 = await client.CreateDisciplina("Estrutura de Dados", [curso.Id]);
        var disciplina03 = await client.CreateDisciplina("Programação Orientada a Objetos", [curso.Id]);

        var disciplinas = new List<GradeDisciplinaIn>() { new() { Id = disciplina01.Id }, new() { Id = disciplina02.Id }, new() { Id = disciplina03.Id } };

        var grade = await client.CreateGrade("Grade de ADS 1.0", curso.Id, disciplinas);
        var oferta = await client.CreateOferta(campus.Id, curso.Id, grade.Id, periodo.Id, Turno.Noturno);

        var aluno = await client.CreateAluno(oferta.Id, "Zaqueu");

        var token = await _factory.GetResetPasswordToken(aluno.Email);
        var password = await client.ResetPassword(token!);
        await client.Login(aluno.Email, password);

        // Act
        var response = await client.GetAlunoDisciplinas();

        // Assert
        response.Count.Should().Be(3);
        response[0].Name.Should().Be("Banco de Dados");
        response[1].Name.Should().Be("Estrutura de Dados");
        response[2].Name.Should().Be("Programação Orientada a Objetos");
    }

    [Test]
    public async Task Deve_retornar_os_alunos()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.CreateCurso("ADS");
        var grade = await client.CreateGrade("Grade de ADS 1.0", curso.Id);
        var oferta = await client.CreateOferta(campus.Id, curso.Id, grade.Id, periodo.Id, Turno.Noturno);

        await client.CreateAluno(oferta.Id, "Zaqueu");
        await client.CreateAluno(oferta.Id, "Maju");

        // Act
        var response = await client.GetAsync<List<AlunoOut>>("/alunos");

        // Assert
        response.Count.Should().Be(2); 
    }
}
