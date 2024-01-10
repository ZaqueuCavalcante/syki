using Syki.Shared;

namespace Syki.Tests.Base;

public static class HttpClientExtensions
{
    public static async Task<UserOut> RegisterUser(this HttpClient client, UserIn body)
    {
        var response = await client.PostAsync("/users", body.ToStringContent());
        return await response.DeserializeTo<UserOut>();
    }

    public static void RemoveAuthToken(this HttpClient client)
    {
        client.DefaultRequestHeaders.Remove("Authorization");
    }

    public static void AddAuthToken(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }

    public static async Task<string> Login(this HttpClient client, string email, string password)
    {
        var data = new LoginIn { Email = email, Password = password };

        var response = await client.PostAsync<LoginOut>("/users/login", data);

        client.RemoveAuthToken();
        client.AddAuthToken(response.AccessToken);

        return response.AccessToken;
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
        await client.PostAsync(path, obj.ToStringContent());
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

    public static async Task<UserIn> RegisterAndLogin(this HttpClient client, Guid faculdadeId, string role)
    {
        if (role != "Adm")
        {
            var user = UserIn.New(faculdadeId, role);
            await client.RegisterUser(user);
            await client.Login(user.Email, user.Password);
            return user;
        }

        return null!;
    }
}
