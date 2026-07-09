using System.Runtime.CompilerServices;

namespace Estud.Back.Settings;

public abstract class SettingsBase
{
    protected void RequireNonEmpty(string value, [CallerArgumentExpression(nameof(value))] string prop = "")
    {
        if (value.IsEmpty()) throw new InvalidOperationException($"{GetType().Name}.{prop} is required!");
    }

    protected void RequirePositive(int value, [CallerArgumentExpression(nameof(value))] string prop = "")
    {
        if (value <= 0) throw new InvalidOperationException($"{GetType().Name}.{prop} is required!");
    }
}
