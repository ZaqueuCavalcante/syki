namespace Estud.Back.Features.Identity.UpdateSsoConfiguration;

public class UpdateSsoConfigurationOut : IApiDto<UpdateSsoConfigurationOut>
{
    public Guid Id { get; set; }

    public static IEnumerable<(string, UpdateSsoConfigurationOut)> GetExamples() =>
    [
        ("Exemplo", new UpdateSsoConfigurationOut { Id = Guid.NewGuid() }),
    ];
}
