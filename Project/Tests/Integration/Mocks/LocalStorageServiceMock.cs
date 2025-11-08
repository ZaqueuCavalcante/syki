using System.Text.Json;
using Microsoft.JSInterop;
using System.Text.Json.Serialization.Metadata;

namespace Exato.Tests.Integration.Mocks;

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

    public ValueTask<TValue?> GetItemAsync<TValue>(string key, JsonTypeInfo<TValue>? jsonTypeInfo)
    {
        if (_storage.TryGetValue(key, out var json))
        {
            var value = jsonTypeInfo != null
                ? JsonSerializer.Deserialize(json, jsonTypeInfo)
                : JsonSerializer.Deserialize<TValue>(json);
            return new ValueTask<TValue?>(value);
        }

        return new ValueTask<TValue?>((TValue?)default);
    }

    public ValueTask SetItemAsync<TValue>(string key, TValue value, JsonTypeInfo<TValue>? jsonTypeInfo)
    {
        var json = jsonTypeInfo != null
            ? JsonSerializer.Serialize(value, jsonTypeInfo)
            : JsonSerializer.Serialize(value);

        _storage[key] = json;
        return new ValueTask();
    }
}
