using Estud.Back.Domain.Institutions;

namespace Estud.Back.Features.Institutions.SetupInstitutionConfig;

public static class SetupInstitutionConfigMapper
{
    extension(InstitutionConfig config)
    {
        public SetupInstitutionConfigOut ToSetupInstitutionConfigOut()
        {
            return new()
            {
                Id = config.Id,
                NoteLimit = config.NoteLimit,
                FrequencyLimit = config.FrequencyLimit,
            };
        }
    }
}
