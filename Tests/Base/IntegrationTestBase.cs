using Syki.Database;
using NUnit.Framework;
using Syki.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Syki.Tests.Base;

public class ApiTestBase
{
    protected HttpClient _client = null!;
    protected SykiDbContext _context = null!;
    protected SykiWebApplicationFactory _factory = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new SykiWebApplicationFactory();
    }

    [SetUp]
    public void SetupBeforeEachTest()
    {
        using var scope = _factory.Services.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<SykiDbContext>();
        _client = _factory.CreateClient();

        var cnn = _context.Database.GetConnectionString()!;

        if (Env.IsTesting() && cnn.Contains("Database=syki-tests-db"))
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
    }
}
