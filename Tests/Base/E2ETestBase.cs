using Syki.Back.Settings;
using Microsoft.Playwright;

namespace Syki.Tests.Base;

public class E2ETestBase : PageTest
{
    private readonly string _url = "http://localhost:5002";
    private readonly string _database = "UserID=postgres;Password=postgres;Host=localhost;Port=5432;Database=syki-db;Pooling=true;";

    protected async Task Goto(string path)
    {
        await Page.GotoAsync($"{_url}{path}");
    }

    protected async Task ClickOn(ILocator locator)
    {
        await Expect(locator).ToBeVisibleAsync();
        await locator.ClickAsync();
    }

    protected ILocator Link(string text)
    {
        return Page.GetByRole(AriaRole.Link, new() { Name = text });
    }

    protected ILocator Button(string text)
    {
        return Page.GetByRole(AriaRole.Button, new() { Name = text });
    }

    protected async Task<string> Fill(string text)
    {
        await Page.GetByRole(AriaRole.Textbox).FillAsync(text);
        return text;
    }

    protected async Task AssertVisibleText(string text)
    {
        await Expect(Page.GetByText(text)).ToBeVisibleAsync();
    }

    protected async Task AssertVisibleButton(string text)
    {
        var button = Page.GetByRole(AriaRole.Button, new() { Name = text });
        await Expect(button).ToBeVisibleAsync();
    }

    protected async Task AssertVisibleLink(string text)
    {
        var link = Page.GetByRole(AriaRole.Link, new() { Name = text });
        await Expect(link).ToBeVisibleAsync();
    }

    protected SykiDbContext GetDbContext()
    {
        var settings = new DatabaseSettings { ConnectionString = _database };
        return new SykiDbContext(new DbContextOptions<SykiDbContext>(), settings, null);
    }

    protected async Task<string> GetRegisterToken(string email)
    {
        using var ctx = GetDbContext();
        var register = await ctx.UserRegisters.FirstAsync(d => d.Email == email);
        return register.Id.ToString();
    }

    protected async Task<string> GetMfaCode(string email)
    {
        using var ctx = GetDbContext();
        var userId = await ctx.Users.Where(u => u.Email == email).Select(u => u.Id).FirstAsync();
        var mfaKey = await ctx.UserTokens.Where(t => t.UserId == userId).Select(t => t.Value).FirstAsync();
        return mfaKey!.GenerateTOTP();
    }
}
