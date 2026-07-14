using System.Net.Http.Json;
using Estud.Back.Features.Institutions.GetInstitutionConfig;
using Estud.Back.Features.Institutions.SetupInstitutionConfig;

namespace Estud.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<GetInstitutionConfigOut, ErrorOut>> GetInstitutionConfig()
    {
        var response = await http.GetAsync("/institutions/config");
        return await response.Resolve<GetInstitutionConfigOut>();
    }

    public async Task<OneOf<SetupInstitutionConfigOut, ErrorOut>> SetupInstitutionConfig(
        decimal noteLimit = 7.00M,
        decimal frequencyLimit = 70.00M
    ) {
        var data = new SetupInstitutionConfigIn { NoteLimit = noteLimit, FrequencyLimit = frequencyLimit };
        var response = await http.PostAsJsonAsync("/institutions/config", data);
        return await response.Resolve<SetupInstitutionConfigOut>();
    }
}
