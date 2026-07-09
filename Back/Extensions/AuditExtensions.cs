using Audit.Core;

namespace Estud.Back.Extensions;

public static class AuditExtensions
{
    public const string UserId = nameof(UserId);
    public const string InstitutionId = nameof(InstitutionId);
    public const string ActivityId = nameof(ActivityId);
    public const string Operation = nameof(Operation);

    extension(AuditEvent evt)
    {
        public int UserId => int.Parse(evt.CustomFields[UserId].ToString() ?? "0");
        public int InstitutionId => int.Parse(evt.CustomFields[InstitutionId].ToString() ?? "0");
        public string ActivityId => evt.CustomFields[ActivityId]?.ToString() ?? Guid.NewGuid().ToString();
        public string Operation => evt.CustomFields[Operation]?.ToString() ?? "OPERATION_NOT_FOUND";
    }

    extension(AuditScope scope)
    {
        public void SetUserId(int userId) => scope.SetCustomField(UserId, userId);
        public void SetInstitutionId(int institutionId) => scope.SetCustomField(InstitutionId, institutionId);
        public void SetActivityId(string activityId) => scope.SetCustomField(ActivityId, activityId);
        public void SetOperation(string operation) => scope.SetCustomField(Operation, operation);
    }
}
