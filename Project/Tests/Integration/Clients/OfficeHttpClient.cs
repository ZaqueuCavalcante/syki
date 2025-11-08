using Exato.Front.Features.Office.CreateRole;
using Exato.Front.Features.Office.CreateUser;
using Exato.Shared.Features.Office.CreateRole;
using Exato.Shared.Features.Office.CreateUser;
using Exato.Front.Features.Office.CriarEmpresa;
using Exato.Front.Features.Office.CriarUsuario;
using Exato.Shared.Features.Office.CriarUsuario;
using Exato.Shared.Features.Office.CriarEmpresa;
using Exato.Front.Features.Office.BuscarEmpresa;
using Exato.Front.Features.Office.CreateCompany;
using Exato.Front.Features.Office.GetAuditTrail;
using Exato.Shared.Features.Office.GetAuditTrail;
using Exato.Shared.Features.Office.CreateCompany;
using Exato.Front.Features.Office.BuscarEmpresas;
using Exato.Shared.Features.Office.BuscarEmpresa;
using Exato.Front.Features.Office.ExcluirUsuario;
using Exato.Shared.Features.Office.ExcluirUsuario;
using Exato.Shared.Features.Office.BuscarEmpresas;
using Exato.Front.Features.Office.CriarTokenDeAcesso;
using Exato.Shared.Features.Office.CriarTokenDeAcesso;
using Exato.Front.Features.Office.EditarTokenDeAcesso;
using Exato.Shared.Features.Office.EditarTokenDeAcesso;
using Exato.Front.Features.Office.EditarSaldoDaEmpresa;
using Exato.Shared.Features.Office.EditarSaldoDaEmpresa;
using Exato.Front.Features.Office.EditarClaimsDoUsuario;
using Exato.Shared.Features.Office.EditarClaimsDoUsuario;
using Exato.Front.Features.Office.VincularEmpresaUsuario;
using Exato.Shared.Features.Office.VincularEmpresaUsuario;
using Exato.Front.Features.Office.BuscarEmpresasDoUsuario;
using Exato.Front.Features.Office.EditarCadastroDaEmpresa;
using Exato.Shared.Features.Office.BuscarEmpresasDoUsuario;
using Exato.Shared.Features.Office.EditarCadastroDaEmpresa;
using Exato.Front.Features.Office.EditarConsultasDaEmpresa;
using Exato.Shared.Features.Office.EditarConsultasDaEmpresa;
using Exato.Front.Features.Office.DesvincularEmpresaUsuario;
using Exato.Shared.Features.Office.DesvincularEmpresaUsuario;
using Exato.Front.Features.Office.EditarFaturamentoDaEmpresa;
using Exato.Shared.Features.Office.EditarFaturamentoDaEmpresa;

namespace Exato.Tests.Integration.Clients;

public class OfficeHttpClient(HttpClient http)
{
    public readonly HttpClient Cross = http;

    public CreateUserOut User { get; set; }
    public CriarEmpresaOut Org { get; set; }

    public async Task<OneOf<CreateUserOut, ErrorOut>> CreateUser(
        int organizationId,
        string name,
        string email,
        Guid roleId)
    {
        var client = new CreateUserClient(Cross);
        return await client.Create(organizationId, name, email, roleId);
    }

    public async Task<OneOf<CreateRoleOut, ErrorOut>> CreateRole(
        string name,
        string description,
        int organizationId,
        List<int> features)
    {
        var client = new CreateRoleClient(Cross);
        return await client.Create(name, description, organizationId, features);
    }

    public async Task<OneOf<CriarEmpresaOut, ErrorOut>> CriarEmpresa(
        string name = "Uber",
        string document = "17.895.646/0001-87",
        string razaoSocial = "UBER BR LTDA.",
        bool exatoWeb = true)
    {
        var client = new CriarEmpresaClient(Cross);
        return await client.Create(name, document, razaoSocial, exatoWeb);
    }

    public async Task<OneOf<CreateCompanyOut, ErrorOut>> CreateCompany(Guid externalId)
    {
        var client = new CreateCompanyClient(Cross);
        return await client.Create(externalId);
    }

    public async Task<OneOf<EditarCadastroDaEmpresaOut, ErrorOut>> EditarCadastroDaEmpresa(
        int id,
        bool ativa,
        string nome,
        string cnpj,
        string razaoSocial,
        int? matrizId,
        string? nomeFantasia,
        string? slug,
        string? salesContact)
    {
        var client = new EditarCadastroDaEmpresaClient(Cross);
        return await client.Editar(id, ativa, nome, cnpj, razaoSocial, matrizId, nomeFantasia, slug, salesContact);
    }

    public async Task<OneOf<EditarFaturamentoDaEmpresaOut, ErrorOut>> EditarFaturamentoDaEmpresa(
        int id,
        bool habilitado,
        MetodoDePagamento metodoDePagamento)
    {
        var client = new EditarFaturamentoDaEmpresaClient(Cross);
        return await client.Editar(id, habilitado, metodoDePagamento);
    }

    public async Task<OneOf<EditarSaldoDaEmpresaOut, ErrorOut>> EditarSaldoDaEmpresa(
        int id,
        decimal amount,
        int credits)
    {
        var client = new EditarSaldoDaEmpresaClient(Cross);
        return await client.Editar(id, amount, credits);
    }

    public async Task<BuscarEmpresasOut> BuscarEmpresas(BuscarEmpresasIn data)
    {
        var client = new BuscarEmpresasClient(Cross);
        return await client.Get(data);
    }

    public async Task<OneOf<BuscarEmpresaOut, ErrorOut>> BuscarEmpresa(int id)
    {
        var client = new BuscarEmpresaClient(Cross);
        return await client.Get(id);
    }

    public async Task<OneOf<EditarConsultasDaEmpresaOut, ErrorOut>> EditarConsultasDaEmpresa(
        int id,
        bool highPerformance,
        bool blockSensitiveDataInQueryString,
        DataAccessLevel dataAccessLevel,
        int? transLimitPerWeek,
        bool gerarPdfConsultas,
        bool habilitarConsultasPorEmail,
        bool receitaCpfUseSerproAsMainSource,
        bool receitaCpfShouldReturnMinor18AgeData)
    {
        var client = new EditarConsultasDaEmpresaClient(Cross);
        return await client.Editar(
            id,
            highPerformance,
            blockSensitiveDataInQueryString,
            dataAccessLevel,
            transLimitPerWeek,
            gerarPdfConsultas,
            habilitarConsultasPorEmail,
            receitaCpfUseSerproAsMainSource,
            receitaCpfShouldReturnMinor18AgeData
        );
    }

    public async Task<OneOf<CriarTokenDeAcessoOut, ErrorOut>> CriarTokenDeAcesso(
        int clienteId,
        TokenAcessoKeyType keyType,
        string? name = null,
        string? description = null,
        DateTime? validoAte = null,
        int? transLimitPerHour = null,
        int? transLimitPerDay = null,
        int? transLimitPerWeek = null,
        int? transLimitPerMonth = null,
        int? creditsLimitPerHour = null,
        int? creditsLimitPerDay = null,
        int? creditsLimitPerWeek = null,
        int? creditsLimitPerMonth = null,
        decimal? currencyLimitPerHour = null,
        decimal? currencyLimitPerDay = null,
        decimal? currencyLimitPerWeek = null,
        decimal? currencyLimitPerMonth = null)
    {
        var client = new CriarTokenDeAcessoClient(Cross);
        return await client.Criar(
            clienteId,
            keyType,
            name,
            description,
            validoAte,
            transLimitPerHour,
            transLimitPerDay,
            transLimitPerWeek,
            transLimitPerMonth,
            creditsLimitPerHour,
            creditsLimitPerDay,
            creditsLimitPerWeek,
            creditsLimitPerMonth,
            currencyLimitPerHour,
            currencyLimitPerDay,
            currencyLimitPerWeek,
            currencyLimitPerMonth);
    }

    public async Task<OneOf<EditarTokenDeAcessoOut, ErrorOut>> EditarTokenDeAcesso(
        int id,
        string? name = null,
        string? description = null,
        DateTime? validoAte = null,
        int? transLimitPerHour = null,
        int? transLimitPerDay = null,
        int? transLimitPerWeek = null,
        int? transLimitPerMonth = null,
        int? creditsLimitPerHour = null,
        int? creditsLimitPerDay = null,
        int? creditsLimitPerWeek = null,
        int? creditsLimitPerMonth = null,
        decimal? currencyLimitPerHour = null,
        decimal? currencyLimitPerDay = null,
        decimal? currencyLimitPerWeek = null,
        decimal? currencyLimitPerMonth = null)
    {
        var client = new EditarTokenDeAcessoClient(Cross);
        return await client.Editar(
            id,
            name,
            description,
            validoAte,
            transLimitPerHour,
            transLimitPerDay,
            transLimitPerWeek,
            transLimitPerMonth,
            creditsLimitPerHour,
            creditsLimitPerDay,
            creditsLimitPerWeek,
            creditsLimitPerMonth,
            currencyLimitPerHour,
            currencyLimitPerDay,
            currencyLimitPerWeek,
            currencyLimitPerMonth);
    }

    public async Task<OneOf<CriarUsuarioOut, ErrorOut>> CriarUsuario(
        string nome,
        string email,
        string? cpf,
        List<ExatoWebClaims> claims)
    {
        var client = new CriarUsuarioClient(Cross);
        return await client.Create(nome, email, cpf, claims);
    }

    public async Task<OneOf<ExcluirUsuarioOut, ErrorOut>> ExcluirUsuario(int id)
    {
        var client = new ExcluirUsuarioClient(Cross);
        return await client.Excluir(id);
    }

    public async Task<OneOf<VincularEmpresaUsuarioOut, ErrorOut>> VincularEmpresaUsuario(int clienteId, int userId)
    {
        var client = new VincularEmpresaUsuarioClient(Cross);
        return await client.Vincular(clienteId, userId);
    }

    public async Task<OneOf<DesvincularEmpresaUsuarioOut, ErrorOut>> DesvincularEmpresaUsuario(int clienteId, int userId)
    {
        var client = new DesvincularEmpresaUsuarioClient(Cross);
        return await client.Desvincular(clienteId, userId);
    }

    public async Task<BuscarEmpresasDoUsuarioOut> BuscarEmpresasDoUsuario(int id, BuscarEmpresasDoUsuarioIn query)
    {
        var client = new BuscarEmpresasDoUsuarioClient(Cross);
        return await client.Get(id, query);
    }

    public async Task<OneOf<EditarClaimsDoUsuarioOut, ErrorOut>> EditarClaimsDoUsuario(
        int id,
        List<ExatoWebClaims> claims)
    {
        var client = new EditarClaimsDoUsuarioClient(Cross);
        return await client.Editar(id, claims);
    }

    public async Task<OneOf<GetAuditTrailOut, ErrorOut>> GetAuditTrail(Guid id)
    {
        var client = new GetAuditTrailClient(Cross);
        return await client.Get(id);
    }
}
