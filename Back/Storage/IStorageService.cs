namespace Syki.Back.Storage;

public interface IStorageService
{
    Task UploadProfilePhoto(string name, Stream stream);
}
