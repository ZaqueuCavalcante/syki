namespace Syki.Back.Storage;

public interface IStorageService
{
    Task<string> CreatePreSignedUrlForUpload(StorageContainer container, string path);
}
