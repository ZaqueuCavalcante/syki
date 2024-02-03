using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Exceptions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Nao_deve_criar_uma_oferta_sem_vinculo_com_campus()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var body = new OfertaIn { };

        // Act
        var response = await client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(Throw.DE0007);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_oferta_quando_o_campus_nao_existe()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var body = new OfertaIn { CampusId = Guid.NewGuid() };

        // Act
        var response = await client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(Throw.DE0007);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_oferta_quando_o_campus_pertence_a_outra_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var ufpe = await client.CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await client.RegisterUser(userUfpe);

        await client.Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new CampusIn { Nome = "Suassuna", Cidade = "Recife - PE" };
        var campusUfpe = await client.PostAsync<CampusOut>("/campi", bodyUfpe);

        var novaRoma = await client.CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await client.RegisterUser(userNovaRoma);

        await client.Login(userNovaRoma.Email, userNovaRoma.Password);
        var bodyNovaRoma = new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" };
        await client.PostAsync<CampusOut>("/campi", bodyNovaRoma);

        var body = new OfertaIn { CampusId = campusUfpe.Id };

        // Act
        var response = await client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(Throw.DE0007);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_oferta_sem_vinculo_com_periodo()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var body = new OfertaIn { CampusId = campus.Id };

        // Act
        var response = await client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(Throw.DE0003);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_oferta_com_periodo_que_nao_existe()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var body = new OfertaIn { CampusId = campus.Id, Periodo = "2025.1" };

        // Act
        var response = await client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(Throw.DE0003);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_oferta_com_periodo_de_outra_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var ufpe = await client.CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await client.RegisterUser(userUfpe);

        await client.Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new PeriodoIn("2023.1");
        var periodoUfpe = await client.PostAsync<PeriodoOut>("/periodos", bodyUfpe);

        var novaRoma = await client.CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await client.RegisterUser(userNovaRoma);

        await client.Login(userNovaRoma.Email, userNovaRoma.Password);
        var campusNovaRoma = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var bodyNovaRoma = new PeriodoIn("2023.2");
        await client.PostAsync<PeriodoOut>("/periodos", bodyNovaRoma);

        var body = new OfertaIn { CampusId = campusNovaRoma.Id, Periodo = periodoUfpe.Id };

        // Act
        var response = await client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(Throw.DE0003);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_oferta_sem_vinculo_com_curso()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn("2024.1"));
        await client.PostAsync<PeriodoOut>("/periodos", periodo);

        var body = new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, };

        // Act
        var response = await client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(Throw.DE0001);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_oferta_com_curso_que_nao_existe()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn("2024.1"));
        await client.PostAsync<PeriodoOut>("/periodos", periodo);

        var body = new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = Guid.NewGuid(), };

        // Act
        var response = await client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(Throw.DE0001);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_oferta_com_curso_de_outra_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var ufpe = await client.CreateFaculdade("UFPE");
        await client.RegisterAndLogin(ufpe.Id, Academico);

        var bodyUfpe = new CursoIn { Nome = "Direito", Tipo = TipoDeCurso.Licenciatura };
        var curso = await client.PostAsync<CursoOut>("/cursos", bodyUfpe);

        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn("2024.1"));
        await client.PostAsync<PeriodoOut>("/periodos", periodo);

        var body = new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, };

        // Act
        var response = await client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(Throw.DE0001);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_oferta_com_curso_sem_grade_vinculada()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn("2024.1"));
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Direito", Tipo = TipoDeCurso.Licenciatura });

        var body = new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, };

        // Act
        var response = await client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(Throw.DE0008);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_oferta_com_curso_com_grade_que_nao_existe()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn("2024.1"));
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Direito", Tipo = TipoDeCurso.Licenciatura });

        var body = new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = Guid.NewGuid(), };

        // Act
        var response = await client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(Throw.DE0008);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_oferta_com_curso_com_grade_que_nao_eh_do_curso_escolhido()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn("2024.1"));
        var cursoAds = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });
        var cursoDireito = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Direito", Tipo = TipoDeCurso.Licenciatura });
        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = cursoAds.Id, });

        var body = new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = cursoDireito.Id, GradeId = grade.Id, };

        // Act
        var response = await client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(Throw.DE0008);       
    }

    [Test]
    public async Task Deve_criar_uma_oferta()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn("2024.1"));
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });
        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, });

        var body = new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id, Turno = Turno.Noturno, };

        // Act
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", body);
        
        // Assert
        oferta.Id.Should().NotBeEmpty();
        oferta.GradeId.Should().Be(grade.Id);
        oferta.Periodo.Should().Be(periodo.Id);     
    }

    [Test]
    public async Task Deve_retornar_todas_as_ofertas()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn("2024.1"));
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });
        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, });

        var body = new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id, Turno = Turno.Noturno, };
        await client.PostAsync<OfertaOut>("/ofertas", body);

        // Act
        var ofertas = await client.GetAsync<List<OfertaOut>>("/ofertas");
        
        // Assert
        ofertas.Count.Should().Be(1);
    }
}
