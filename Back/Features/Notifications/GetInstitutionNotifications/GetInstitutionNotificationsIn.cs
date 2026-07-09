namespace Estud.Back.Features.Notifications.GetInstitutionNotifications;

public class GetInstitutionNotificationsIn : IApiDto<GetInstitutionNotificationsIn>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    public static IEnumerable<(string, GetInstitutionNotificationsIn)> GetExamples() =>
    [
        ("Exemplo", new GetInstitutionNotificationsIn() { }),
    ];
}
