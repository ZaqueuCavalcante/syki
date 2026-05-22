namespace Syki.Back.Features.Identity.CreateSsoConfiguration;

public class CreateSsoConfigurationOut : IApiDto<CreateSsoConfigurationOut>
{
    public Guid Id { get; set; }

    public static IEnumerable<(string, CreateSsoConfigurationOut)> GetExamples() =>
    [
        ("Exemplo", new CreateSsoConfigurationOut { Id = Guid.NewGuid() }),
    ];
}
