namespace Syki.Back.Storage;

public class FakeStorageService : IStorageService
{
    public List<string> Files = [];

    public async Task UploadProfilePhoto(string name, Stream stream)
    {
        await Task.Delay(0);
        Files.Add(name);
    }
}
