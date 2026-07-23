using System.Collections.Concurrent;

namespace Estud.Back.Google;

public class FakeGoogleService : IGoogleService
{
    public static readonly ConcurrentDictionary<string, GoogleIdTokenPayload> Tokens = new();

    public Task<GoogleIdTokenPayload?> ValidateIdTokenAsync(string? credential, string expectedAudience)
    {
        Tokens.TryGetValue(credential, out var payload);
        return Task.FromResult(payload);
    }

    /// <summary>
    /// Seeds a fake Google id_token into <see cref="Tokens"/> so that a One Tap
    /// login using <paramref name="credential"/> resolves to the given payload.
    /// Returns the payload's Subject (Google's stable user_id / "sub").
    /// </summary>
    public static string SeedGoogleToken(
        string credential,
        string email,
        string? name = null,
        bool emailVerified = true,
        string? subject = null)
    {
        subject ??= Guid.NewGuid().ToString();

        Tokens[credential] = new GoogleIdTokenPayload
        {
            Name = name,
            Email = email,
            Subject = subject,
            EmailVerified = emailVerified,
        };

        return subject;
    }
}
