using Microsoft.JSInterop;

namespace Syki.Tests.Mock;

public class LocalStorageServiceMock : ILocalStorageService
{
    public ValueTask<double> Length => throw new NotImplementedException();

    private Dictionary<string, string> _storage = new();

    public ValueTask ClearAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask<string?> GetItemAsync(string key)
    {
        _storage.TryGetValue(key, out string? value);
        return new ValueTask<string?>(value);
    }

    public ValueTask<string?> KeyAsync(double index)
    {
        throw new NotImplementedException();
    }

    public ValueTask RemoveItemAsync(string key)
    {
        _storage.Remove(key);
        return new ValueTask();
    }

    public ValueTask SetItemAsync(string key, string value)
    {
        _storage.Add(key, value);
        return new ValueTask();
    }
}
