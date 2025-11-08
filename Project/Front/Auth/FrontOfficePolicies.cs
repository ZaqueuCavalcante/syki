using Exato.Shared.Auth;

namespace Exato.Front.Auth;

public static partial class Policies
{
    public static List<PolicyMetadata> Office = [];

	public const string ViewOfficeUsersPage = nameof(ViewOfficeUsersPage);
	public const string OfficeCreateExatoUser = nameof(OfficeCreateExatoUser);

	public const string ViewOfficeRolesPage = nameof(ViewOfficeRolesPage);
	public const string OfficeCreateExatoRole = nameof(OfficeCreateExatoRole);
	public const string OfficeUpdateExatoRole = nameof(OfficeUpdateExatoRole);
	public const string ViewOfficeFeaturesPage = nameof(ViewOfficeFeaturesPage);
	public const string ViewOfficePoliciesPage = nameof(ViewOfficePoliciesPage);

	public const string ViewOfficeDomainEventsPage = nameof(ViewOfficeDomainEventsPage);
	public const string ViewOfficeDomainEventPage = nameof(ViewOfficeDomainEventPage);

	public const string ViewOfficeCommandsPage = nameof(ViewOfficeCommandsPage);
	public const string ViewOfficeCommandPage = nameof(ViewOfficeCommandPage);

	public const string ViewOfficeCommandBatchesPage = nameof(ViewOfficeCommandBatchesPage);
	public const string ViewOfficeCommandBatchPage = nameof(ViewOfficeCommandBatchPage);
	public const string OfficeReprocessCommand = nameof(OfficeReprocessCommand);

	public const string ViewOfficeAuditTrailsPage = nameof(ViewOfficeAuditTrailsPage);
	public const string ViewOfficeAuditTrailDrawer = nameof(ViewOfficeAuditTrailDrawer);

	public const string ViewOfficeEmpresasPage = nameof(ViewOfficeEmpresasPage);
	public const string OfficeCriarEmpresa = nameof(OfficeCriarEmpresa);

	public const string ViewOfficeEmpresaPage = nameof(ViewOfficeEmpresaPage);
	public const string OfficeEditarCadastroDaEmpresa = nameof(OfficeEditarCadastroDaEmpresa);
	public const string OfficeEditarConsultasDaEmpresa = nameof(OfficeEditarConsultasDaEmpresa);
	public const string OfficeEditarRelatoriosDaEmpresa = nameof(OfficeEditarRelatoriosDaEmpresa);
	public const string OfficeEditarFaturamentoDaEmpresa = nameof(OfficeEditarFaturamentoDaEmpresa);
	public const string OfficeCriarTokenDeAcessoDaEmpresa = nameof(OfficeCriarTokenDeAcessoDaEmpresa);
	public const string OfficeEditarTokenDeAcessoDaEmpresa = nameof(OfficeEditarTokenDeAcessoDaEmpresa);

	public const string OfficeEditarSaldoDaEmpresa = nameof(OfficeEditarSaldoDaEmpresa);
	public const string OfficeVincularUsuarioEmpresa = nameof(OfficeVincularUsuarioEmpresa);
	public const string OfficeDesvincularUsuarioEmpresa = nameof(OfficeDesvincularUsuarioEmpresa);

	public const string ViewOfficeUsuariosPage = nameof(ViewOfficeUsuariosPage);
	public const string OfficeCriarUsuario = nameof(OfficeCriarUsuario);
	public const string OfficeExcluirUsuario = nameof(OfficeExcluirUsuario);

	public const string ViewOfficeUsuarioPage = nameof(ViewOfficeUsuarioPage);
	public const string OfficeEditarCadastroDoUsuario = nameof(OfficeEditarCadastroDoUsuario);
	public const string OfficeEditarPermissoesDoUsuario = nameof(OfficeEditarPermissoesDoUsuario);

	public const string ViewOfficeConsultasPage = nameof(ViewOfficeConsultasPage);
	public const string ViewOfficeConsultaPage = nameof(ViewOfficeConsultaPage);

	public const string ViewOfficeWebPaymentsPage = nameof(ViewOfficeWebPaymentsPage);

	public const string OfficeCriarEmpresaNoExatoWeb = nameof(OfficeCriarEmpresaNoExatoWeb);
	public const string OfficeEditarEmpresaNoExatoWeb = nameof(OfficeEditarEmpresaNoExatoWeb);

	public static AuthorizationOptions AddOfficePolicy(this AuthorizationOptions options, string name, params ExatoFeature[] features)
	{
		Office.Add(new() { Name = name, Features = features.ToList() });

		var ids = features.Select(x => x.Id).ToList();

		options.AddPolicy(name, policy => policy
			.RequireAuthenticatedUser()
			.RequireAssertion(x => x.User.Features.Any(f => ids.Contains(f))));

		return options;
	}

    public static AuthorizationOptions AddOfficePolicies(this AuthorizationOptions options)
	{
		options
			.AddOfficePolicy(ViewOfficeUsersPage, ExatoFeaturesStore.ViewExatoUsers)
			.AddOfficePolicy(OfficeCreateExatoUser, ExatoFeaturesStore.CreateExatoUser);

		options
			.AddOfficePolicy(ViewOfficeRolesPage, ExatoFeaturesStore.ViewExatoRoles)
			.AddOfficePolicy(OfficeCreateExatoRole, ExatoFeaturesStore.CreateExatoRole)
			.AddOfficePolicy(OfficeUpdateExatoRole, ExatoFeaturesStore.UpdateExatoRole)
			.AddOfficePolicy(ViewOfficeFeaturesPage, ExatoFeaturesStore.ViewExatoFeatures)
			.AddOfficePolicy(ViewOfficePoliciesPage, ExatoFeaturesStore.ViewExatoPolicies);

		options
			.AddOfficePolicy(ViewOfficeDomainEventsPage, ExatoFeaturesStore.ViewDomainEvents)
			.AddOfficePolicy(ViewOfficeDomainEventPage, ExatoFeaturesStore.ViewDomainEventDetails);

		options
			.AddOfficePolicy(ViewOfficeCommandsPage, ExatoFeaturesStore.ViewCommands)
			.AddOfficePolicy(ViewOfficeCommandPage, ExatoFeaturesStore.ViewCommandDetails)
			.AddOfficePolicy(OfficeReprocessCommand, ExatoFeaturesStore.ReprocessCommand);

		options
			.AddOfficePolicy(ViewOfficeCommandBatchesPage, ExatoFeaturesStore.ViewCommandBatches)
			.AddOfficePolicy(ViewOfficeCommandBatchPage, ExatoFeaturesStore.ViewCommandBatchDetails);

		options
			.AddOfficePolicy(ViewOfficeAuditTrailsPage, ExatoFeaturesStore.ViewAuditTrails)
			.AddOfficePolicy(ViewOfficeAuditTrailDrawer, ExatoFeaturesStore.ViewAuditTrailDetails);

		options
			.AddOfficePolicy(ViewOfficeEmpresasPage, ExatoFeaturesStore.VerEmpresas)
			.AddOfficePolicy(OfficeCriarEmpresa, ExatoFeaturesStore.CriarEmpresa);

		options
			.AddOfficePolicy(ViewOfficeEmpresaPage, ExatoFeaturesStore.VerDetalhesDeUmaEmpresa)
			.AddOfficePolicy(OfficeEditarCadastroDaEmpresa, ExatoFeaturesStore.EditarCadastroDaEmpresa)
			.AddOfficePolicy(OfficeEditarConsultasDaEmpresa, ExatoFeaturesStore.EditarConsultasDaEmpresa)
			.AddOfficePolicy(OfficeEditarRelatoriosDaEmpresa, ExatoFeaturesStore.EditarRelatoriosDaEmpresa)
			.AddOfficePolicy(OfficeEditarFaturamentoDaEmpresa, ExatoFeaturesStore.EditarFaturamentoDaEmpresa)
			.AddOfficePolicy(OfficeCriarTokenDeAcessoDaEmpresa, ExatoFeaturesStore.CriarTokenDeAcessoDaEmpresa)
			.AddOfficePolicy(OfficeEditarTokenDeAcessoDaEmpresa, ExatoFeaturesStore.EditarTokenDeAcessoDaEmpresa);

		options
			.AddOfficePolicy(OfficeEditarSaldoDaEmpresa, ExatoFeaturesStore.EditarSaldoDaEmpresa)
			.AddOfficePolicy(OfficeVincularUsuarioEmpresa, ExatoFeaturesStore.VincularUsuarioEmpresa)
			.AddOfficePolicy(OfficeDesvincularUsuarioEmpresa, ExatoFeaturesStore.DesvincularUsuarioEmpresa);

		options
			.AddOfficePolicy(ViewOfficeUsuariosPage, ExatoFeaturesStore.VerUsuarios)
			.AddOfficePolicy(OfficeCriarUsuario, ExatoFeaturesStore.CriarUsuario)
			.AddOfficePolicy(OfficeExcluirUsuario, ExatoFeaturesStore.ExcluirUsuario);

		options
			.AddOfficePolicy(ViewOfficeUsuarioPage, ExatoFeaturesStore.VerDetalhesDeUmUsuario)
			.AddOfficePolicy(OfficeEditarCadastroDoUsuario, ExatoFeaturesStore.EditarCadastroDoUsuario)
			.AddOfficePolicy(OfficeEditarPermissoesDoUsuario, ExatoFeaturesStore.EditarPermissoesDoUsuario);

		options
			.AddOfficePolicy(ViewOfficeConsultasPage, ExatoFeaturesStore.VerConsultas);

		options
			.AddOfficePolicy(ViewOfficeConsultaPage, ExatoFeaturesStore.VerDetalhesDeUmaConsulta);

		options
			.AddOfficePolicy(ViewOfficeWebPaymentsPage, ExatoFeaturesStore.ViewWebPayments);

		options
			.AddOfficePolicy(OfficeCriarEmpresaNoExatoWeb, ExatoFeaturesStore.CriarEmpresaNoExatoWeb)
			.AddOfficePolicy(OfficeEditarEmpresaNoExatoWeb, ExatoFeaturesStore.EditarEmpresaNoExatoWeb);

		return options;
    }
}
