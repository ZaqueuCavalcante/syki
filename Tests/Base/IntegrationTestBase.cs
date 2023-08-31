using Syki.Domain;
using Syki.Database;
using NUnit.Framework;
using Syki.Extensions;
using Syki.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using System.Net;

namespace Syki.Tests.Base;

public class ApiTestBase
{
    protected HttpClient _client = null!;
    protected SykiDbContext _ctx = null!;
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
        _ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();
        _client = _factory.CreateClient();

        var cnn = _ctx.Database.GetConnectionString()!;

        if (Env.IsTesting() && cnn.Contains("Database=syki-tests-db"))
        {
            _ctx.Database.EnsureDeleted();
            _ctx.Database.EnsureCreated();
        }
    }

    protected async Task PostAsync(string path, object obj)
    {
        var response = await _client.PostAsync(path, obj.ToStringContent());
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    protected async Task<T> PostAsync<T>(string path, object obj)
    {
        var response = await _client.PostAsync(path, obj.ToStringContent());
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return await response.DeserializeTo<T>();
    }

    protected async Task<T> GetAsync<T>(string path)
    {
        var response = await _client.GetAsync(path);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return await response.DeserializeTo<T>();
    }

    protected async Task<string> Login(string email)
    {
        var data = new LoginIn { Email = email };

        var response = await _client.PostAsync("/users/login", data.ToStringContent());

        var token = await response.DeserializeTo<LoginOut>();

        _client.DefaultRequestHeaders.Remove("Authorization");
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.AccessToken}");

        return token.AccessToken;
    }

    protected async Task<FaculdadeOut> CreateFaculdade(string nome)
    {
        await Login("adm@syki.com");

        var body = new FaculdadeIn { Nome = nome };

        var response = await _client.PostAsync("/faculdades", body.ToStringContent());

        return await response.DeserializeTo<FaculdadeOut>();
    }
}
