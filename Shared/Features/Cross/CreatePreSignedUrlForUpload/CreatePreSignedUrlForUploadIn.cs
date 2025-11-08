namespace Syki.Shared;

public class CreatePreSignedUrlForUploadIn : IApiDto<CreatePreSignedUrlForUploadIn>
{
    public string FileName { get; set; }
    public StorageContainer Container { get; set; }

    public static IEnumerable<(string, CreatePreSignedUrlForUploadIn)> GetExamples() =>
    [
        ("Exemplo", new() { FileName = "Profile.png", Container = StorageContainer.ProfilePhotos }),
    ];
}
