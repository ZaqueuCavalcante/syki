using Syki.Shared;
using Syki.Front.Auth;
using Syki.Tests.Mock;
using Syki.Front.Services;
using Microsoft.JSInterop;
using System.Security.Claims;
using RichardSzalay.MockHttp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;
using Syki.Shared.Login;

namespace Syki.Tests.Services;

public class AuthServiceTests : BunitTestContext
{
    [Test]
    public async Task Should_login()
    {
        // Arrange
        Services.AddScoped<IAuthService, AuthService>();
        Services.AddScoped<ILocalStorageService, LocalStorageServiceMock>();
        Services.AddScoped<SykiAuthStateProvider>();
        Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<SykiAuthStateProvider>());

        var response = new LoginOut { AccessToken = AuthTestBase.JWTAcademico };
        var mock = Services.AddMockHttpClient();
        mock.When(HttpMethod.Post, "/login").RespondJson(response);

        var authService = Services.GetService<IAuthService>()!;

        // Act
        var result = await authService.Login("email", "password");

        // Assert
        result.AccessToken.Should().Be(AuthTestBase.JWTAcademico);
        var storage = Services.GetService<ILocalStorageService>()!;
        var token = await storage.GetItemAsync("AccessToken");
        token.Should().Be(AuthTestBase.JWTAcademico);
    }

    [Test]
    public async Task Should_create_user_on_login()
    {
        // Arrange
        Services.AddScoped<IAuthService, AuthService>();
        Services.AddScoped<ILocalStorageService, LocalStorageServiceMock>();
        Services.AddScoped<SykiAuthStateProvider>();
        Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<SykiAuthStateProvider>());

        var response = new LoginOut { AccessToken = AuthTestBase.JWTAcademico  };
        var mock = Services.AddMockHttpClient();
        mock.When(HttpMethod.Post, "/login").RespondJson(response);

        var authService = Services.GetService<IAuthService>()!;
        await authService.Login("email", "password");

        // Act
        var user = await authService.GetUser();

        // Assert
        user.FindFirstValue("role").Should().Be(Academico);
    }

    [Test]
    public async Task Should_login_with_mfa()
    {
        // Arrange
        Services.AddScoped<IAuthService, AuthService>();
        Services.AddScoped<ILocalStorageService, LocalStorageServiceMock>();
        Services.AddScoped<SykiAuthStateProvider>();
        Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<SykiAuthStateProvider>());

        var response = new LoginOut { AccessToken = AuthTestBase.JWTAcademico  };
        var mock = Services.AddMockHttpClient();
        mock.When(HttpMethod.Post, "/users/login-mfa").RespondJson(response);

        var authService = Services.GetService<IAuthService>()!;

        // Act
        var result = await authService.LoginMfa("159753");

        // Assert
        result.AccessToken.Should().Be(AuthTestBase.JWTAcademico);
        var storage = Services.GetService<ILocalStorageService>()!;
        var token = await storage.GetItemAsync("AccessToken");
        token.Should().Be(AuthTestBase.JWTAcademico);
    }

    [Test]
    public async Task Should_create_user_on_login_with_mfa()
    {
        // Arrange
        Services.AddScoped<IAuthService, AuthService>();
        Services.AddScoped<ILocalStorageService, LocalStorageServiceMock>();
        Services.AddScoped<SykiAuthStateProvider>();
        Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<SykiAuthStateProvider>());

        var response = new LoginOut { AccessToken = AuthTestBase.JWTAcademico  };
        var mock = Services.AddMockHttpClient();
        mock.When(HttpMethod.Post, "/users/login-mfa").RespondJson(response);

        var authService = Services.GetService<IAuthService>()!;
        await authService.LoginMfa("159753");

        // Act
        var user = await authService.GetUser();

        // Assert
        user.FindFirstValue("role").Should().Be(Academico);
    }

    [Test]
    public async Task Should_logout()
    {
        // Arrange
        Services.AddScoped<IAuthService, AuthService>();
        Services.AddScoped<ILocalStorageService, LocalStorageServiceMock>();
        Services.AddScoped<SykiAuthStateProvider>();
        Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<SykiAuthStateProvider>());

        var authService = Services.GetService<IAuthService>()!;

        // Act
        await authService.Logout();

        // Assert
        var storage = Services.GetService<ILocalStorageService>()!;
        var token = await storage.GetItemAsync("AccessToken");
        token.Should().BeNull();
    }
}
