using Syki.Shared.Auth;

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

    public static AuthorizationBuilder AddOfficePolicy(this AuthorizationBuilder builder, string name, params SykiFeature[] features)
    {
        Adm.Add(new() { Name = name, Features = features.ToList() });

        var ids = features.Select(x => x.Id).ToList();

        return builder.AddPolicy(name, policy => policy
            .RequireAuthenticatedUser()
            .RequireAssertion(x => x.User.Features.Any(f => ids.Contains(f)))
            .AddAuthenticationSchemes(AuthenticationConfigs.BearerScheme));
    }

    public static AuthorizationBuilder AddOfficePolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddOfficePolicy(GetUsers, FeaturesStore.ViewUsers)
            .AddOfficePolicy(CreateUser, FeaturesStore.CreateUser);

        builder
            .AddOfficePolicy(GetRoles, FeaturesStore.ViewRoles)
            .AddOfficePolicy(CreateRole, FeaturesStore.CreateRole)
            .AddOfficePolicy(UpdateRole, FeaturesStore.UpdateRole)
            .AddOfficePolicy(GetFeatures, FeaturesStore.ViewFeatures)
            .AddOfficePolicy(GetPolicies, FeaturesStore.ViewPolicies);

        builder
            .AddOfficePolicy(GetDomainEvents, FeaturesStore.ViewDomainEvents)
            .AddOfficePolicy(GetDomainEvent, FeaturesStore.ViewDomainEventDetails)
            .AddOfficePolicy(GetCommands, FeaturesStore.ViewCommands)
            .AddOfficePolicy(GetCommand, FeaturesStore.ViewCommandDetails)
            .AddOfficePolicy(ReprocessCommand, FeaturesStore.ReprocessCommand)
            .AddOfficePolicy(GetCommandBatches, FeaturesStore.ViewCommandBatches)
            .AddOfficePolicy(GetCommandBatch, FeaturesStore.ViewCommandBatchDetails)
            .AddOfficePolicy(GetCommandBatchCommands, FeaturesStore.ViewCommandBatchDetails);

        builder
            .AddOfficePolicy(GetAuditTrails, FeaturesStore.ViewAuditTrails)
            .AddOfficePolicy(GetAuditTrail, FeaturesStore.ViewAuditTrailDetails)
            .AddOfficePolicy(GetAuditTrailOperations, FeaturesStore.ViewAuditTrails);

        return builder;
    }
}
