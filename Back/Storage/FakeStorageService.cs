namespace Syki.Back.Storage;

public class FakeStorageService : IStorageService
{
    public List<string> Files = [];

    public async Task<string> CreatePreSignedUrlForUpload(StorageContainer container, string path)
    {
        var url = $"https://syki.storage.com/{container.GetDescription()}/{path}";
        await Task.Delay(0);
        Files.Add(url);
        return url;
    }
}
