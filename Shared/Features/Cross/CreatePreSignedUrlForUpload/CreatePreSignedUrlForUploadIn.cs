namespace Syki.Shared;

public class CreatePreSignedUrlForUploadIn
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public StorageContainer Container { get; set; }
}
