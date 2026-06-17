using System.Collections.Concurrent;

namespace Syki.Back.Google;

public class FakeGoogleService : IGoogleService
{
    public static readonly ConcurrentDictionary<string, GoogleIdTokenPayload> Tokens = new();

    public Task<GoogleIdTokenPayload?> ValidateIdTokenAsync(string credential, string expectedAudience)
    {
        Tokens.TryGetValue(credential, out var payload);
        return Task.FromResult(payload);
    }

    public static void Reset()
    {
        Tokens.Clear();
    }
}
