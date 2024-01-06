using Bunit;
using NUnit.Framework;
using MudBlazor.Services;

namespace Syki.Tests.Base;

public abstract class BunitTestContext : TestContextWrapper
{
    [SetUp]
    public void Setup() => TestContext = new Bunit.TestContext();

    [TearDown]
    public void TearDown() => TestContext?.Dispose();

    public void AddMudServices()
    {
        Services.AddMudServices();
        TestContext!.JSInterop.SetupVoid("mudJsEvent.connect", _ => true);
        TestContext!.JSInterop.SetupVoid("mudPopover.initialize", _ => true);
        TestContext!.JSInterop.SetupVoid("mudElementRef.saveFocus", _ => true);
        TestContext!.JSInterop.SetupVoid("mudScrollManager.lockScroll", _ => true);
        TestContext!.JSInterop.SetupVoid("mudScrollManager.unlockScroll", _ => true);
    }
}
