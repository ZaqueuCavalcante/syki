using System.Reflection;

namespace Exato.Shared.Auth;

public static class ExatoFeaturesStore
{
    public static readonly ExatoFeature ViewExatoUsers = new(0, 0, "Ver usuários do schema exato.");
    public static readonly ExatoFeature CreateExatoUser = new(0, 1, "Criar usuário no schema exato.");
    public static readonly ExatoFeature ViewExatoRoles = new(0, 2, "Ver roles do sistema.");
    public static readonly ExatoFeature CreateExatoRole = new(0, 33, "Criar role no sistema.");
    public static readonly ExatoFeature UpdateExatoRole = new(0, 34, "Editar role no sistema.");
    public static readonly ExatoFeature ViewExatoFeatures = new(0, 3, "Ver features do sistema.");
    public static readonly ExatoFeature ViewExatoPolicies = new(0, 4, "Ver policies do sistema.");
    public static readonly ExatoFeature ViewAuditTrails = new(0, 12, "Ver audit trails.");
    public static readonly ExatoFeature ViewAuditTrailDetails = new(0, 13, "Ver detalhes de um audit trail.");

    public static readonly ExatoFeature ViewDomainEvents = new(1, 5, "Ver eventos de domínio.");
    public static readonly ExatoFeature ViewDomainEventDetails = new(1, 6, "Ver detalhes de um evento de domínio.");
    public static readonly ExatoFeature ViewCommands = new(1, 7, "Ver comandos.");
    public static readonly ExatoFeature ViewCommandDetails = new(1, 8, "Ver detalhes de um comando.");
    public static readonly ExatoFeature ReprocessCommand = new(1, 9, "Reprocessar um comando.");
    public static readonly ExatoFeature ViewCommandBatches = new(1, 10, "Ver lotes de comandos.");
    public static readonly ExatoFeature ViewCommandBatchDetails = new(1, 11, "Ver detalhes de um lote de comandos.");

    public static readonly ExatoFeature VerEmpresas = new(2, 14, "Ver empresas.");
    public static readonly ExatoFeature CriarEmpresa = new(2, 15, "Criar empresa.");
    public static readonly ExatoFeature VerDetalhesDeUmaEmpresa = new(2, 16, "Ver detalhes de uma empresa.");
    public static readonly ExatoFeature EditarCadastroDaEmpresa = new(2, 17, "Editar cadastro da empresa.");
    public static readonly ExatoFeature EditarConsultasDaEmpresa = new(2, 18, "Editar consultas da empresa.");
    public static readonly ExatoFeature EditarRelatoriosDaEmpresa = new(2, 19, "Editar relatórios da empresa.");
    public static readonly ExatoFeature EditarFaturamentoDaEmpresa = new(2, 20, "Editar faturamento da empresa.");
    public static readonly ExatoFeature EditarSaldoDaEmpresa = new(2, 21, "Editar saldo da empresa.");
    public static readonly ExatoFeature CriarTokenDeAcessoDaEmpresa = new(2, 22, "Criar token de acesso da empresa.");
    public static readonly ExatoFeature EditarTokenDeAcessoDaEmpresa = new(2, 23, "Editar token de acesso da empresa.");
    public static readonly ExatoFeature VincularUsuarioEmpresa = new(2, 24, "Vincular usuário-empresa.");
    public static readonly ExatoFeature DesvincularUsuarioEmpresa = new(2, 25, "Desvincular usuário-empresa.");
    public static readonly ExatoFeature CriarEmpresaNoExatoWeb = new(2, 32, "Criar empresa no Exato Web.");
    public static readonly ExatoFeature EditarEmpresaNoExatoWeb = new(2, 38, "Editar empresa no Exato Web.");

    public static readonly ExatoFeature VerUsuarios = new(3, 26, "Ver usuários.");
    public static readonly ExatoFeature CriarUsuario = new(3, 27, "Criar usuário.");
    public static readonly ExatoFeature ExcluirUsuario = new(3, 35, "Excluir usuário.");
    public static readonly ExatoFeature VerDetalhesDeUmUsuario = new(3, 28, "Ver detalhes de um usuário.");
    public static readonly ExatoFeature EditarCadastroDoUsuario = new(3, 29, "Editar cadastro do usuário.");
    public static readonly ExatoFeature EditarPermissoesDoUsuario = new(3, 30, "Editar permissões do usuário.");

    public static readonly ExatoFeature VerConsultas = new(4, 31, "Ver consultas.");
    public static readonly ExatoFeature VerDetalhesDeUmaConsulta = new(4, 37, "Ver detalhes de uma consulta.");

    public static readonly ExatoFeature ViewWebPayments = new(5, 36, "Ver pagamentos do Exato Web.");

    public static readonly List<FeatureGroup> Groups = [];
    public static readonly List<ExatoFeature> Features = [];
    static ExatoFeaturesStore()
    {
        Groups =
        [
            new(0, "Users"),
            new(1, "Workers"),
            new(2, "Empresas"),
            new(3, "Usuários"),
            new(4, "Consultas"),
            new(5, "Pagamentos"),
        ];

        Features = typeof(ExatoFeaturesStore)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(f => f.FieldType == typeof(ExatoFeature))
            .Select(f => (ExatoFeature)f.GetValue(null)!)
            .OrderBy(f => f.GroupId)
                .ThenBy(f => f.Id)
            .ToList();

        if (Features.Any(x => Groups.FirstOrDefault(g => g.Id == x.GroupId) == null)) throw new Exception("Invalid group id!");

        if (!Features.Select(x => x.Id).IsAllDistinct()) throw new Exception("Duplicated feature ids!");

        if (!Features.Select(x => x.Name).IsAllDistinct()) throw new Exception("Duplicated feature names!");

        if (Features.Count - 1 != Features.OrderByDescending(x => x.Id).First().Id) throw new Exception("Wrong ids!");
    }
}
