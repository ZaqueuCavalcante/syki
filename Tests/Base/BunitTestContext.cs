using Bunit;
using NUnit.Framework;

namespace Syki.Tests.Base;

public abstract class BunitTestContext : TestContextWrapper
{
    [SetUp]
    public void Setup() => TestContext = new Bunit.TestContext();

    [TearDown]
    public void TearDown() => TestContext?.Dispose();
}
