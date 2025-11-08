using Exato.Shared.Auth;

namespace Exato.Back.Auth;

public static partial class Policies
{
    public static List<PolicyMetadata> Office = [];

	public const string GetUsers = nameof(GetUsers);
	public const string CreateUser = nameof(CreateUser);
    public const string GetOrganizationRoles = nameof(GetOrganizationRoles);

    public const string GetRoles = nameof(GetRoles);
    public const string CreateRole = nameof(CreateRole);
    public const string UpdateRole = nameof(UpdateRole);
    public const string GetFeatures = nameof(GetFeatures);
    public const string GetPolicies = nameof(GetPolicies);

    public const string GetDomainEvents = nameof(GetDomainEvents);
	public const string GetDomainEvent = nameof(GetDomainEvent);

	public const string GetCommands = nameof(GetCommands);
	public const string GetCommand = nameof(GetCommand);
	public const string ReprocessCommand = nameof(ReprocessCommand);

	public const string GetCommandBatches = nameof(GetCommandBatches);
	public const string GetCommandBatch = nameof(GetCommandBatch);
	public const string GetCommandBatchCommands = nameof(GetCommandBatchCommands);

	public const string GetAuditTrails = nameof(GetAuditTrails);
	public const string GetAuditTrail = nameof(GetAuditTrail);
	public const string GetAuditTrailOperations = nameof(GetAuditTrailOperations);

	public const string BuscarEmpresas = nameof(BuscarEmpresas);
    public const string CriarEmpresa = nameof(CriarEmpresa);

	public const string BuscarEmpresa = nameof(BuscarEmpresa);
	public const string BuscarFiliaisDaEmpresa = nameof(BuscarFiliaisDaEmpresa);
    public const string BuscarUsuariosDaEmpresa = nameof(BuscarUsuariosDaEmpresa);
	public const string BuscarPotenciaisMatrizesDaEmpresa = nameof(BuscarPotenciaisMatrizesDaEmpresa);
	public const string BuscarPotenciaisUsuariosDaEmpresa = nameof(BuscarPotenciaisUsuariosDaEmpresa);
	public const string BuscarConfiguracoesDeFaturamentoDaEmpresa = nameof(BuscarConfiguracoesDeFaturamentoDaEmpresa);
    public const string BuscarTokensDeAcessoDaEmpresa = nameof(BuscarTokensDeAcessoDaEmpresa);
    public const string BuscarVendedores = nameof(BuscarVendedores);
	public const string BuscarTiposDeRelatorios = nameof(BuscarTiposDeRelatorios);
	public const string EditarCadastroDaEmpresa = nameof(EditarCadastroDaEmpresa);
    public const string EditarConsultasDaEmpresa = nameof(EditarConsultasDaEmpresa);
	public const string EditarRelatoriosDaEmpresa = nameof(EditarRelatoriosDaEmpresa);
	public const string EditarFaturamentoDaEmpresa = nameof(EditarFaturamentoDaEmpresa);
    public const string EditarSaldoDaEmpresa = nameof(EditarSaldoDaEmpresa);
    public const string CriarTokenDeAcessoDaEmpresa = nameof(CriarTokenDeAcessoDaEmpresa);
    public const string EditarTokenDeAcessoDaEmpresa = nameof(EditarTokenDeAcessoDaEmpresa);

    public const string BuscarUsuarios = nameof(BuscarUsuarios);
    public const string CriarUsuario = nameof(CriarUsuario);
    public const string ExcluirUsuario = nameof(ExcluirUsuario);

	public const string BuscarUsuario = nameof(BuscarUsuario);
	public const string BuscarEventosDoUsuario = nameof(BuscarEventosDoUsuario);
	public const string BuscarEmpresasDoUsuario = nameof(BuscarEmpresasDoUsuario);
	public const string BuscarPotenciaisEmpresasDoUsuario = nameof(BuscarPotenciaisEmpresasDoUsuario);
	public const string EditarClaimsDoUsuario = nameof(EditarClaimsDoUsuario);
	public const string EditarCadastroDoUsuario = nameof(EditarCadastroDoUsuario);
	public const string VincularEmpresaUsuario = nameof(VincularEmpresaUsuario);
	public const string VincularEmpresasAoUsuario = nameof(VincularEmpresasAoUsuario);
	public const string DesvincularEmpresaUsuario = nameof(DesvincularEmpresaUsuario);
	public const string BuscarVinculoEmpresaUsuario = nameof(BuscarVinculoEmpresaUsuario);

	public const string BuscarConsultas = nameof(BuscarConsultas);
	public const string BuscarTiposDeConsulta = nameof(BuscarTiposDeConsulta);
	public const string BuscarTiposDeResultado = nameof(BuscarTiposDeResultado);

	public const string BuscarConsulta = nameof(BuscarConsulta);

    public const string GetWebPayments = nameof(GetWebPayments);

    public const string GetCompany = nameof(GetCompany);
	public const string CreateCompany = nameof(CreateCompany);
    public const string UpdateCompany = nameof(UpdateCompany);

    public static AuthorizationBuilder AddOfficePolicy(this AuthorizationBuilder builder, string name, params ExatoFeature[] features)
    {
        Office.Add(new() { Name = name, Features = features.ToList() });

        var ids = features.Select(x => x.Id).ToList();

        return builder.AddPolicy(name, policy => policy
            .RequireAuthenticatedUser()
            .RequireAssertion(x => x.User.Features.Any(f => ids.Contains(f)))
            .AddAuthenticationSchemes(AuthenticationConfigs.BearerScheme));
    }

    public static AuthorizationBuilder AddOfficePolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddOfficePolicy(GetUsers, ExatoFeaturesStore.ViewExatoUsers)
            .AddOfficePolicy(CreateUser, ExatoFeaturesStore.CreateExatoUser)
            .AddOfficePolicy(GetOrganizationRoles, ExatoFeaturesStore.CreateExatoUser);

        builder
            .AddOfficePolicy(GetRoles, ExatoFeaturesStore.ViewExatoRoles)
            .AddOfficePolicy(CreateRole, ExatoFeaturesStore.CreateExatoRole)
            .AddOfficePolicy(UpdateRole, ExatoFeaturesStore.UpdateExatoRole)
            .AddOfficePolicy(GetFeatures, ExatoFeaturesStore.ViewExatoFeatures)
            .AddOfficePolicy(GetPolicies, ExatoFeaturesStore.ViewExatoPolicies);

        builder
            .AddOfficePolicy(GetDomainEvents, ExatoFeaturesStore.ViewDomainEvents)
            .AddOfficePolicy(GetDomainEvent, ExatoFeaturesStore.ViewDomainEventDetails)
            .AddOfficePolicy(GetCommands, ExatoFeaturesStore.ViewCommands)
            .AddOfficePolicy(GetCommand, ExatoFeaturesStore.ViewCommandDetails)
            .AddOfficePolicy(ReprocessCommand, ExatoFeaturesStore.ReprocessCommand)
            .AddOfficePolicy(GetCommandBatches, ExatoFeaturesStore.ViewCommandBatches)
            .AddOfficePolicy(GetCommandBatch, ExatoFeaturesStore.ViewCommandBatchDetails)
            .AddOfficePolicy(GetCommandBatchCommands, ExatoFeaturesStore.ViewCommandBatchDetails);

        builder
            .AddOfficePolicy(GetAuditTrails, ExatoFeaturesStore.ViewAuditTrails)
            .AddOfficePolicy(GetAuditTrail, ExatoFeaturesStore.ViewAuditTrailDetails)
            .AddOfficePolicy(GetAuditTrailOperations, ExatoFeaturesStore.ViewAuditTrails);

        builder
            .AddOfficePolicy(BuscarEmpresas, ExatoFeaturesStore.VerEmpresas, ExatoFeaturesStore.VerConsultas, ExatoFeaturesStore.CreateExatoUser)
            .AddOfficePolicy(CriarEmpresa, ExatoFeaturesStore.CriarEmpresa);

        builder
            .AddOfficePolicy(BuscarEmpresa, ExatoFeaturesStore.VerDetalhesDeUmaEmpresa)
            .AddOfficePolicy(BuscarFiliaisDaEmpresa, ExatoFeaturesStore.VerDetalhesDeUmaEmpresa)
            .AddOfficePolicy(BuscarUsuariosDaEmpresa, ExatoFeaturesStore.VerDetalhesDeUmaEmpresa)
            .AddOfficePolicy(BuscarPotenciaisMatrizesDaEmpresa, ExatoFeaturesStore.VerDetalhesDeUmaEmpresa)
            .AddOfficePolicy(BuscarPotenciaisUsuariosDaEmpresa, ExatoFeaturesStore.VerDetalhesDeUmaEmpresa)
            .AddOfficePolicy(BuscarConfiguracoesDeFaturamentoDaEmpresa, ExatoFeaturesStore.VerDetalhesDeUmaEmpresa)
            .AddOfficePolicy(BuscarTokensDeAcessoDaEmpresa, ExatoFeaturesStore.VerDetalhesDeUmaEmpresa)
            .AddOfficePolicy(BuscarTiposDeRelatorios, ExatoFeaturesStore.VerDetalhesDeUmaEmpresa)
            .AddOfficePolicy(BuscarVendedores, ExatoFeaturesStore.VerDetalhesDeUmaEmpresa)
            .AddOfficePolicy(EditarCadastroDaEmpresa, ExatoFeaturesStore.EditarCadastroDaEmpresa)
            .AddOfficePolicy(EditarConsultasDaEmpresa, ExatoFeaturesStore.EditarConsultasDaEmpresa)
            .AddOfficePolicy(EditarRelatoriosDaEmpresa, ExatoFeaturesStore.EditarRelatoriosDaEmpresa)
            .AddOfficePolicy(EditarFaturamentoDaEmpresa, ExatoFeaturesStore.EditarFaturamentoDaEmpresa)
            .AddOfficePolicy(CriarTokenDeAcessoDaEmpresa, ExatoFeaturesStore.CriarTokenDeAcessoDaEmpresa)
            .AddOfficePolicy(EditarTokenDeAcessoDaEmpresa, ExatoFeaturesStore.EditarTokenDeAcessoDaEmpresa);

        builder
            .AddOfficePolicy(EditarSaldoDaEmpresa, ExatoFeaturesStore.EditarSaldoDaEmpresa)
            .AddOfficePolicy(VincularEmpresaUsuario, ExatoFeaturesStore.VincularUsuarioEmpresa)
            .AddOfficePolicy(VincularEmpresasAoUsuario, ExatoFeaturesStore.VincularUsuarioEmpresa)
            .AddOfficePolicy(DesvincularEmpresaUsuario, ExatoFeaturesStore.DesvincularUsuarioEmpresa)
            .AddOfficePolicy(BuscarVinculoEmpresaUsuario, ExatoFeaturesStore.DesvincularUsuarioEmpresa);

        builder
            .AddOfficePolicy(BuscarUsuarios, ExatoFeaturesStore.VerUsuarios)
            .AddOfficePolicy(CriarUsuario, ExatoFeaturesStore.CriarUsuario)
            .AddOfficePolicy(ExcluirUsuario, ExatoFeaturesStore.ExcluirUsuario);

        builder
            .AddOfficePolicy(BuscarUsuario, ExatoFeaturesStore.VerDetalhesDeUmUsuario)
            .AddOfficePolicy(BuscarEventosDoUsuario, ExatoFeaturesStore.VerDetalhesDeUmUsuario)
            .AddOfficePolicy(BuscarPotenciaisEmpresasDoUsuario, ExatoFeaturesStore.VerDetalhesDeUmUsuario)
            .AddOfficePolicy(BuscarEmpresasDoUsuario, ExatoFeaturesStore.VerDetalhesDeUmUsuario)
            .AddOfficePolicy(EditarCadastroDoUsuario, ExatoFeaturesStore.EditarCadastroDoUsuario)
            .AddOfficePolicy(EditarClaimsDoUsuario, ExatoFeaturesStore.EditarPermissoesDoUsuario);

        builder
            .AddOfficePolicy(BuscarConsultas, ExatoFeaturesStore.VerConsultas)
            .AddOfficePolicy(BuscarTiposDeConsulta, ExatoFeaturesStore.VerConsultas)
            .AddOfficePolicy(BuscarTiposDeResultado, ExatoFeaturesStore.VerConsultas);

        builder
            .AddOfficePolicy(BuscarConsulta, ExatoFeaturesStore.VerDetalhesDeUmaConsulta);

        builder
            .AddOfficePolicy(GetWebPayments, ExatoFeaturesStore.ViewWebPayments);

        builder
            .AddOfficePolicy(GetCompany, ExatoFeaturesStore.VerDetalhesDeUmaEmpresa)
            .AddOfficePolicy(CreateCompany, ExatoFeaturesStore.CriarEmpresaNoExatoWeb)
            .AddOfficePolicy(UpdateCompany, ExatoFeaturesStore.EditarEmpresaNoExatoWeb);

        return builder;
    }
}
