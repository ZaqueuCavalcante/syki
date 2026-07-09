using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Estud.Back.Extensions;

public static partial class SsoExtensions
{
    extension(string value)
    {
        public EstudError? ValidateSsoAuthority()
        {
            // Must be valid URI
            if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
                return InvalidSsoAuthority.I;

            // Must be HTTPS
            if (uri.Scheme != Uri.UriSchemeHttps)
                return SsoAuthorityMustBeHttps.I;

            // Block URLs with userinfo (SSRF bypass: https://evil.com@169.254.169.254/)
            if (uri.UserInfo.HasValue())
                return SsoAuthorityHasUserInfo.I;

            // Parse and validate the host
            return uri.Host.ValidateSsoHost();
        }

        public EstudError? ValidateSsoHost()
        {
            // Try to parse as IP address
            if (IPAddress.TryParse(value, out var ip))
                return ip.ValidateSsoIpAddress();

            // It's a hostname - check for dangerous hostnames
            var lowerHost = value.ToLowerInvariant();

            // Block localhost variants (allow in dev/testing)
            if (lowerHost is "localhost" or "localhost.localdomain")
                return EnvironmentExtensions.IsDevelopmentOrTesting() ? null : SsoAuthorityLocalhostNotAllowed.I;

            return null;
        }

        public string? NormalizeSsoDomain()
        {
            if (value.IsEmpty()) return null;

            var normalized = value.Trim().ToLowerInvariant();

            // Remove @ if present at start
            if (normalized.StartsWith('@'))
                normalized = normalized[1..];

            // Basic domain validation
            if (!SsoDomainRegex().IsMatch(normalized))
                return null;

            return normalized;
        }
    }

    extension(IPAddress ip)
    {
        public EstudError? ValidateSsoIpAddress()
        {
            var resolved = ip;
            var isDevOrTest = EnvironmentExtensions.IsDevelopmentOrTesting();

            // Handle IPv4-mapped IPv6 addresses (::ffff:127.0.0.1)
            if (resolved.IsIPv4MappedToIPv6)
                resolved = resolved.MapToIPv4();

            // IPv6 checks
            if (resolved.AddressFamily == AddressFamily.InterNetworkV6)
            {
                // Block IPv6 loopback (::1) — allow in dev/testing
                if (IPAddress.IPv6Loopback.Equals(resolved))
                    return isDevOrTest ? null : SsoAuthorityLoopbackNotAllowed.I;

                // Block IPv6 link-local (fe80::/10) — always blocked (cloud metadata risk)
                if (resolved.IsIPv6LinkLocal)
                    return SsoAuthorityLinkLocalNotAllowed.I;

                // Block IPv6 unique local addresses (fc00::/7 = fc00:: and fd00::) — allow in dev/testing
                var bytes = resolved.GetAddressBytes();
                if ((bytes[0] & 0xFE) == 0xFC) // fc00::/7
                    return isDevOrTest ? null : SsoAuthorityPrivateIpNotAllowed.I;

                return null;
            }

            // IPv4 checks
            var ipBytes = resolved.GetAddressBytes();

            // Block 0.0.0.0 — always blocked
            if (ipBytes[0] == 0 && ipBytes[1] == 0 && ipBytes[2] == 0 && ipBytes[3] == 0)
                return SsoAuthorityLoopbackNotAllowed.I;

            // Block entire loopback range 127.0.0.0/8 — allow in dev/testing
            if (ipBytes[0] == 127)
                return isDevOrTest ? null : SsoAuthorityLoopbackNotAllowed.I;

            // Block entire link-local range 169.254.0.0/16 — always blocked (cloud metadata risk)
            if (ipBytes[0] == 169 && ipBytes[1] == 254)
                return SsoAuthorityLinkLocalNotAllowed.I;

            // Block private IP ranges — allow in dev/testing
            if (IsPrivateSsoIpV4(ipBytes))
                return isDevOrTest ? null : SsoAuthorityPrivateIpNotAllowed.I;

            return null;
        }
    }

    private static bool IsPrivateSsoIpV4(byte[] bytes)
    {
        return bytes[0] switch
        {
            10 => true,                                      // 10.0.0.0/8
            172 => bytes[1] >= 16 && bytes[1] <= 31,         // 172.16.0.0/12
            192 => bytes[1] == 168,                          // 192.168.0.0/16
            _ => false
        };
    }

    [GeneratedRegex(@"^[a-z0-9]([a-z0-9-]*[a-z0-9])?(\.[a-z0-9]([a-z0-9-]*[a-z0-9])?)+$")]
    private static partial Regex SsoDomainRegex();
}
