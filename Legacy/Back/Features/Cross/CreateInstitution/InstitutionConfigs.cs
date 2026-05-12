namespace Syki.Back.Features.Cross.CreateInstitution;

public class InstitutionConfigs
{
    public Guid InstitutionId { get; set; }
    public decimal NoteLimit { get; set; }
    public decimal FrequencyLimit { get; set; }

    public InstitutionConfigs(
        Guid institutionId,
        decimal noteLimit,
        decimal frequencyLimit
    ) {
        InstitutionId = institutionId;
        NoteLimit = noteLimit;
        FrequencyLimit = frequencyLimit;
    }
}
