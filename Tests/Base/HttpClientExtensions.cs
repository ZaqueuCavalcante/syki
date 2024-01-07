using Syki.Shared;

namespace Syki.Tests.Base;

public static class HttpClientExtensions
{
    public static async Task RegisterUser(this HttpClient client, UserIn body)
    {
        await client.PostAsync("/users", body.ToStringContent());
    }

    public static async Task<string> Login(this HttpClient client, string email, string password)
    {
        var data = new LoginIn { Email = email, Password = password };

        var response = await client.PostAsync("/users/login", data.ToStringContent());

        var token = await response.DeserializeTo<LoginOut>();

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.AccessToken}");

        return token.AccessToken;
    }

    public static async Task<FaculdadeOut> CreateFaculdade(this HttpClient client, string nome)
    {
        await client.Login("adm@syki.com", "Adm@123");

        var body = new FaculdadeIn { Nome = nome };

        var response = await client.PostAsync("/faculdades", body.ToStringContent());

        return await response.DeserializeTo<FaculdadeOut>();
    }

    public static async Task PostAsync(this HttpClient client, string path, object obj)
    {
        var response = await client.PostAsync(path, obj.ToStringContent());
    }

    public static async Task<T> PostAsync<T>(this HttpClient client, string path, object obj)
    {
        var response = await client.PostAsync(path, obj.ToStringContent());
        return await response.DeserializeTo<T>();
    }

    public static async Task<T> PutAsync<T>(this HttpClient client, string path, object obj)
    {
        var response = await client.PutAsync(path, obj.ToStringContent());
        return await response.DeserializeTo<T>();
    }

    public static async Task<T> GetAsync<T>(this HttpClient client, string path)
    {
        var response = await client.GetAsync(path);
        return await response.DeserializeTo<T>();
    }

    public static async Task RegisterAndLogin(this HttpClient client, Guid faculdadeId, string role)
    {
        if (role != "Adm")
        {
            var user = UserIn.New(faculdadeId, role);
            await client.RegisterUser(user);
            await client.Login(user.Email, user.Password);
        }
    }
}
