using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;
using Syki.Back.Exceptions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class OfertasIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Nao_deve_criar_uma_oferta_sem_vinculo_com_campus()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new OfertaIn { };

        // Act
        var response = await _client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0007);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_oferta_quando_o_campus_nao_existe()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new OfertaIn { CampusId = Guid.NewGuid() };

        // Act
        var response = await _client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0007);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_oferta_quando_o_campus_pertence_a_outra_faculdade()
    {
        // Arrange
        var ufpe = await CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await RegisterUser(userUfpe);

        await Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new CampusIn { Nome = "Suassuna", Cidade = "Recife - PE" };
        var campusUfpe = await PostAsync<CampusOut>("/campi", bodyUfpe);

        var novaRoma = await CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await RegisterUser(userNovaRoma);

        await Login(userNovaRoma.Email, userNovaRoma.Password);
        var bodyNovaRoma = new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" };
        await PostAsync<CampusOut>("/campi", bodyNovaRoma);

        var body = new OfertaIn { CampusId = campusUfpe.Id };

        // Act
        var response = await _client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0007);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_oferta_sem_vinculo_com_periodo()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var campus = await PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var body = new OfertaIn { CampusId = campus.Id };

        // Act
        var response = await _client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0003);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_oferta_com_periodo_que_nao_existe()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var campus = await PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var body = new OfertaIn { CampusId = campus.Id, Periodo = "2025.1" };

        // Act
        var response = await _client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0003);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_oferta_com_periodo_de_outra_faculdade()
    {
        // Arrange
        var ufpe = await CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await RegisterUser(userUfpe);

        await Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new PeriodoIn { Id = "2023.1", Start = new DateOnly(2023, 02, 01), End = new DateOnly(2023, 06, 01) };
        var periodoUfpe = await PostAsync<PeriodoOut>("/periodos", bodyUfpe);

        var novaRoma = await CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await RegisterUser(userNovaRoma);

        await Login(userNovaRoma.Email, userNovaRoma.Password);
        var campusNovaRoma = await PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var bodyNovaRoma = new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) };
        await PostAsync<PeriodoOut>("/periodos", bodyNovaRoma);

        var body = new OfertaIn { CampusId = campusNovaRoma.Id, Periodo = periodoUfpe.Id };

        // Act
        var response = await _client.PostAsync("/ofertas", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0003);       
    }
}
