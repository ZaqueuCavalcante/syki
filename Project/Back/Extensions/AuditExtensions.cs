using Audit.Core;

namespace Exato.Back.Extensions;

public static class AuditExtensions
{
    public const string UserId = nameof(UserId);
    public const string OrganizationId = nameof(OrganizationId);
    public const string ActivityId = nameof(ActivityId);
    public const string Operation = nameof(Operation);

    extension(AuditEvent evt)
    {
        public Guid UserId => Guid.Parse(evt.CustomFields[UserId].ToString() ?? Guid.Empty.ToString());
        public int OrganizationId => int.Parse(evt.CustomFields[OrganizationId].ToString() ?? "0");
        public string ActivityId => evt.CustomFields[ActivityId]?.ToString() ?? Guid.NewGuid().ToString();
        public string Operation => evt.CustomFields[Operation]?.ToString() ?? "OPERATION_NOT_FOUND";
    }

    extension(AuditScope scope)
    {
        public void SetUserId(Guid userId) => scope.SetCustomField("UserId", userId);
        public void SetOrganizationId(int organizationId) => scope.SetCustomField(OrganizationId, organizationId);
        public void SetActivityId(string activityId) => scope.SetCustomField(ActivityId, activityId);
        public void SetOperation(string operation) => scope.SetCustomField(Operation, operation);
    }
}
