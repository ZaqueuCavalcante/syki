namespace Syki.Front.Features.Cross.Health;

public class HealthClient(HttpClient http) : ICrossClient
{
    public async Task Get()
    {
        await http.GetAsync("/health");
    }
}
