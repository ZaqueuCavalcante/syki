using Azure.Storage.Sas;
using Azure.Storage.Blobs;

namespace Syki.Back.Storage;

public class AzureBlobStorageService(AzureBlobStorageSettings settings) : IStorageService
{
    public async Task<string> CreatePreSignedUrlForUpload(StorageContainer container, string path)
    {
        var blobContainer = new BlobContainerClient(settings.ConnectionString, container.GetDescription());
        var blob = blobContainer.GetBlobClient(path);

        var permissions = BlobSasPermissions.Write | BlobSasPermissions.Create;
        var sasBuilder = new BlobSasBuilder(permissions, DateTimeOffset.UtcNow.AddMinutes(5))
        {
            Resource = "b",
            BlobName = blob.Name,
            BlobContainerName = blobContainer.Name,
        };

        Uri uri = blob.GenerateSasUri(sasBuilder);

        return await Task.FromResult(uri.ToString());
    }
}
