using Syki.Shared.Auth;
using Syki.Back.Auth.Schemes;

namespace Syki.Back.Auth;

public static partial class Policies
{
    public static List<PolicyMetadata> Adm = [];

	public const string GetUsers = nameof(GetUsers);
	public const string CreateUser = nameof(CreateUser);

    public const string GetRoles = nameof(GetRoles);
    public const string CreateRole = nameof(CreateRole);
    public const string UpdateRole = nameof(UpdateRole);
    public const string GetFeatures = nameof(GetFeatures);
    public const string GetPolicies = nameof(GetPolicies);

	public const string GetCommands = nameof(GetCommands);
	public const string GetCommand = nameof(GetCommand);
	public const string ReprocessCommand = nameof(ReprocessCommand);

	public const string GetCommandBatches = nameof(GetCommandBatches);
	public const string GetCommandBatch = nameof(GetCommandBatch);
	public const string GetCommandBatchCommands = nameof(GetCommandBatchCommands);

	public const string GetAuditTrails = nameof(GetAuditTrails);
	public const string GetAuditTrail = nameof(GetAuditTrail);
	public const string GetAuditTrailOperations = nameof(GetAuditTrailOperations);

    public static AuthorizationBuilder AddAdmPolicy(this AuthorizationBuilder builder, string name, params SykiFeature[] features)
    {
        Adm.Add(new() { Name = name, Features = features.ToList() });

        var ids = features.Select(x => x.Id).ToList();

        return builder.AddPolicy(name, policy => policy
            .RequireAuthenticatedUser()
            .RequireAssertion(x => x.User.Features.Any(f => ids.Contains(f)))
            .AddAuthenticationSchemes(JwtBearerScheme.Name));
    }

    public static AuthorizationBuilder AddAdmPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddAdmPolicy(GetUsers, FeaturesStore.ViewUsers)
            .AddAdmPolicy(CreateUser, FeaturesStore.CreateUser);

        builder
            .AddAdmPolicy(GetRoles, FeaturesStore.ViewRoles)
            .AddAdmPolicy(CreateRole, FeaturesStore.CreateRole)
            .AddAdmPolicy(UpdateRole, FeaturesStore.UpdateRole)
            .AddAdmPolicy(GetFeatures, FeaturesStore.ViewFeatures)
            .AddAdmPolicy(GetPolicies, FeaturesStore.ViewPolicies);

        builder
            .AddAdmPolicy(GetCommands, FeaturesStore.ViewCommands)
            .AddAdmPolicy(GetCommand, FeaturesStore.ViewCommandDetails)
            .AddAdmPolicy(ReprocessCommand, FeaturesStore.ReprocessCommand)
            .AddAdmPolicy(GetCommandBatches, FeaturesStore.ViewCommandBatches)
            .AddAdmPolicy(GetCommandBatch, FeaturesStore.ViewCommandBatchDetails)
            .AddAdmPolicy(GetCommandBatchCommands, FeaturesStore.ViewCommandBatchDetails);

        builder
            .AddAdmPolicy(GetAuditTrails, FeaturesStore.ViewAuditTrails)
            .AddAdmPolicy(GetAuditTrail, FeaturesStore.ViewAuditTrailDetails)
            .AddAdmPolicy(GetAuditTrailOperations, FeaturesStore.ViewAuditTrails);

        return builder;
    }
}
