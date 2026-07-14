using Estud.Back.Domain.Institutions;

namespace Estud.Back.Features.Institutions.GetInstitutionConfig;

public static class GetInstitutionConfigMapper
{
    extension(InstitutionConfig config)
    {
        public GetInstitutionConfigOut ToGetInstitutionConfigOut()
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
