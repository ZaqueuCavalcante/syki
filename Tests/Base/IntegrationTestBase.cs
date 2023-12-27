using Npgsql;
using Respawn;
using System.Net;
using Syki.Shared;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Domain;
using System.Data.Common;
using Syki.Back.Database;
using Syki.Back.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Base;

public class IntegrationTestBase
{
    protected HttpClient _client = null!;
    protected SykiDbContext _ctx = null!;
    protected Respawner _respawner = null!;
    protected DbConnection _dbConnection = null!;
    protected SykiWebApplicationFactory _factory = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _factory = new SykiWebApplicationFactory();

        using var scope = _factory.Services.CreateScope();
        _ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        if (Env.IsTesting())
        {
            await _ctx.Database.EnsureDeletedAsync();
            await _ctx.Database.EnsureCreatedAsync();
        }

        _dbConnection = new NpgsqlConnection(_ctx.Database.GetConnectionString()!);
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions()
        {
            SchemasToInclude = [ "syki" ],
            TablesToIgnore = [ new ("roles") ],
            DbAdapter = DbAdapter.Postgres,
        });
    }

    [SetUp]
    public async Task SetUp()
    {
        _client = _factory.CreateClient();
        await _respawner.ResetAsync(_dbConnection);

        await AddAdmUser();
    }

    [TearDown]
    public async Task TearDown()
    {
        _client.Dispose();
        await _ctx.DisposeAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        _client.Dispose();
        await _ctx.DisposeAsync();
        await _factory.DisposeAsync();
        await _dbConnection.DisposeAsync();
    }

    private async Task AddAdmUser()
    {
        using var scope = _factory.Services.CreateScope();

        _ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();
        _ctx.Add(new Faculdade { Id = Guid.Empty, Nome = "Syki" });
        await _ctx.SaveChangesAsync();

        var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<SykiUser>>();

        var user = new SykiUser
        {
            Name = "Syki Adm",
            UserName = "adm@syki.com",
            Email = "adm@syki.com",
            FaculdadeId = Guid.Empty,
        };

        await _userManager.CreateAsync(user, "Adm@123");

        await _userManager.AddToRoleAsync(user, Adm);
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

    protected async Task<T> PutAsync<T>(string path, object obj)
    {
        var response = await _client.PutAsync(path, obj.ToStringContent());
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return await response.DeserializeTo<T>();
    }

    protected async Task<T> GetAsync<T>(string path)
    {
        var response = await _client.GetAsync(path);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return await response.DeserializeTo<T>();
    }

    protected async Task<string> Login(string email, string password)
    {
        var data = new LoginIn { Email = email, Password = password };

        var response = await _client.PostAsync("/users/login", data.ToStringContent());

        var token = await response.DeserializeTo<LoginOut>();

        _client.DefaultRequestHeaders.Remove("Authorization");
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.AccessToken}");

        return token.AccessToken;
    }

    protected async Task RegisterUser(UserIn body)
    {
        var response = await _client.PostAsync("/users", body.ToStringContent());
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    protected async Task<FaculdadeOut> CreateFaculdade(string nome)
    {
        await Login("adm@syki.com", "Adm@123");

        var body = new FaculdadeIn { Nome = nome };

        var response = await _client.PostAsync("/faculdades", body.ToStringContent());

        return await response.DeserializeTo<FaculdadeOut>();
    }
}
