namespace Estud.Back.Domain.Webhooks;

public static class WebhookCustomHeaders
{
    public const int MaxHeaders = 20;
    public const int MaxKeyLength = 100;
    public const int MaxValueLength = 1000;

    public static bool IsValid(Dictionary<string, string> headers)
    {
        if (headers == null) return true;

        if (headers.Count > MaxHeaders) return false;

        foreach (var (key, value) in headers)
        {
            if (key.IsEmpty() || key.Length > MaxKeyLength) return false;
            if (value.IsEmpty() || value.Length > MaxValueLength) return false;
        }

        return true;
    }
}
