namespace Estud.Back.Shared;

public class CreatePreSignedUrlForUploadOut : IApiDto<CreatePreSignedUrlForUploadOut>
{
    public string Url { get; set; }

    public static IEnumerable<(string, CreatePreSignedUrlForUploadOut)> GetExamples() =>
    [
        ("Exemplo", new() { Url = $"https://estud.storage.com/{StorageContainer.ProfilePhotos.GetDescription()}/Profile.png" }),
    ];
}
