namespace Exato.Back.Intelligence.Domain.Public;

public class DoccheckTokenSetting
{
    public int Id { get; set; }

    public Guid KeyId { get; set; }

    public string? WebhookUrl { get; set; }

    public string? RedirectUrl { get; set; }

    public int? CustomizationSettingsId { get; set; }

    public string? ActionIfMinor { get; set; }

    public bool? EnableJsPostMessage { get; set; }

    public string? ActionIfPep { get; set; }

    public string? ActionIfPreape { get; set; }

    public string? ActionIfListedOnSanctions { get; set; }

    public bool? WebhookIncludeDetailsAml { get; set; }

    public string? ActionIfCpfCancelled { get; set; }

    public bool? AllowGlobalFaceComparison { get; set; }

    public string? ActionIfBolsaFamilia { get; set; }

    public bool? RejectedValidationToManualReview { get; set; }

    public bool? UseOcrAwsFallback { get; set; }

    public string? ActionIfDetectedFraud { get; set; }

    public bool? RedirectToDigitalDoc { get; set; }

    public string? ActionIfAgeNotInEstimatedRange { get; set; }

    public bool? ReplayThrowError { get; set; }

    public int? MaxMatchLevelForRejection { get; set; }

    public int? MaxMatchLevelForManualReview { get; set; }

    public int? MinMatchLevel { get; set; }

    public int? StatusWhenMinMatchNotMet { get; set; }

    public string? ActionIfForeignDocument { get; set; }

    public string? ActionIfNotOfficialDocument { get; set; }

    public int? ValidationLimitPerMonth { get; set; }

    public int? ForceManualAfterNReadDoc { get; set; }

    public string? CustomDomain { get; set; }

    public string? CustomShortDomain { get; set; }

    public List<string>? CpfsCustomDomain { get; set; }

    public bool? UseExatoToken { get; set; }

    public int? NumberAttemptsMatch3d3d { get; set; }

    public string? ActionIfValidationNotInBrazil { get; set; }

    public string? ForceStatus { get; set; }

    public string? ForceStatusEnrollment { get; set; }

    public string? ForceStatusNotEnrollment { get; set; }

    public bool? UseAwsToMatch3d3d { get; set; }

    public float? AwsLivenessPercentToApprove { get; set; }

    public float? AwsLivenessPercentToReject { get; set; }

    public float? AwsMatch3d3dSimilarityPercentToApprove { get; set; }

    public float? AwsMatch3d3dSimilarityPercentToReject { get; set; }

    public bool? SilentFlow { get; set; }

    public string? ActionIfPreapeMatchCpf { get; set; }

    public string? ActionIfPreapeMatchProbability09 { get; set; }

    public string? ActionIfPreapeMatchProbability08OrLess { get; set; }

    public int? MinutesToExpireValidationLink { get; set; }

    public string? ActionIfLawsuitOccurrences { get; set; }

    public string? ActionIfGoogleNewsOccurrences { get; set; }

    public int? BaseAgeToConsiderMinor { get; set; }

    public string? OcrSupplier { get; set; }

    public bool? UseAiToDetectTextsFromDocument { get; set; }

    public DoccheckTokenSetting() { }

    public DoccheckTokenSetting(Guid keyId)
    {
        KeyId = keyId;
    }
}
