using System.Reflection;

namespace Syki.Shared.Auth;

public static class FeaturesStore
{
    public static readonly SykiFeature ViewUsers = new(0, 0, "Ver usuários.");
    public static readonly SykiFeature CreateUser = new(0, 1, "Criar usuário.");
    public static readonly SykiFeature ViewRoles = new(0, 2, "Ver roles.");
    public static readonly SykiFeature CreateRole = new(0, 3, "Criar role.");
    public static readonly SykiFeature UpdateRole = new(0, 4, "Editar role.");
    public static readonly SykiFeature ViewFeatures = new(0, 5, "Ver features.");
    public static readonly SykiFeature ViewPolicies = new(0, 6, "Ver policies.");
    public static readonly SykiFeature ViewAuditTrails = new(0, 7, "Ver audit trails.");
    public static readonly SykiFeature ViewAuditTrailDetails = new(0, 8, "Ver detalhes de um audit trail.");

    public static readonly SykiFeature ViewDomainEvents = new(1, 9, "Ver eventos de domínio.");
    public static readonly SykiFeature ViewDomainEventDetails = new(1, 10, "Ver detalhes de um evento de domínio.");
    public static readonly SykiFeature ViewCommands = new(1, 11, "Ver comandos.");
    public static readonly SykiFeature ViewCommandDetails = new(1, 12, "Ver detalhes de um comando.");
    public static readonly SykiFeature ReprocessCommand = new(1, 13, "Reprocessar um comando.");
    public static readonly SykiFeature ViewCommandBatches = new(1, 14, "Ver lotes de comandos.");
    public static readonly SykiFeature ViewCommandBatchDetails = new(1, 15, "Ver detalhes de um lote de comandos.");

    public static readonly SykiFeature ViewInstitutions = new(2, 16, "Ver instituições de ensino.");
    public static readonly SykiFeature CreateInstitution = new(2, 17, "Criar instituição de ensino.");
    public static readonly SykiFeature ViewInstitutionDetails = new(2, 18, "Ver detalhes de uma instituição de ensino.");

    public static readonly List<FeatureGroup> Groups = [];
    public static readonly List<SykiFeature> Features = [];
    static FeaturesStore()
    {
        Groups =
        [
            new(0, "Users"),
            new(1, "Daemon"),
            new(2, "Instituições"),
        ];

        Features = typeof(FeaturesStore)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(f => f.FieldType == typeof(SykiFeature))
            .Select(f => (SykiFeature)f.GetValue(null)!)
            .OrderBy(f => f.GroupId)
                .ThenBy(f => f.Id)
            .ToList();

        if (Features.Any(x => Groups.FirstOrDefault(g => g.Id == x.GroupId) == null)) throw new Exception("Invalid group id!");

        if (!Features.Select(x => x.Id).IsAllDistinct()) throw new Exception("Duplicated feature ids!");

        if (!Features.Select(x => x.Name).IsAllDistinct()) throw new Exception("Duplicated feature names!");

        if (Features.Count - 1 != Features.OrderByDescending(x => x.Id).First().Id) throw new Exception("Wrong ids!");
    }
}
