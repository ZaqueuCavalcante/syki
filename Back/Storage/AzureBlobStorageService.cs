using Azure.Storage.Blobs;

namespace Syki.Back.Storage;

public class AzureBlobStorageService(AzureBlobStorageSettings settings) : IStorageService
{
    public async Task UploadProfilePhoto(string name, Stream stream)
    {
        var client = new BlobContainerClient(settings.ConnectionString, "profile-photos");

        await client.UploadBlobAsync(name, stream);
    }
}
