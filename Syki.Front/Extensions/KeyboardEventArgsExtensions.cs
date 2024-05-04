using Microsoft.AspNetCore.Components.Web;

namespace Syki.Front.Extensions;

public static class KeyboardEventArgsExtensions
{
    public static bool IsEnter(this KeyboardEventArgs args)
    {
        return args.Key == "Enter";
    }
}
