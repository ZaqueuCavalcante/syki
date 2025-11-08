using Microsoft.JSInterop;

namespace Exato.Front.Services;

public class ClipboardService(IJSRuntime JsInterop)
{
    public async Task CopyToClipboard(string text)
	{
		await JsInterop.InvokeVoidAsync("navigator.clipboard.writeText", text);
	}
}
