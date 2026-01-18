namespace Vinder.Federation.TestSuite.Extensions;

public static class HttpClientExtensions
{
    public static HttpClient WithTenantHeader(this HttpClient client, string tenant)
    {
        if (client.DefaultRequestHeaders.Contains("x-tenant"))
        {
            client.DefaultRequestHeaders.Remove("x-tenant");
        }

        client.DefaultRequestHeaders.Add("x-tenant", tenant);

        return client;
    }

    public static HttpClient WithAuthorization(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return client;
    }
}
