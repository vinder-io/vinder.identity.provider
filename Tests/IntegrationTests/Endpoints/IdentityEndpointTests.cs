namespace Vinder.IdentityProvider.TestSuite.IntegrationTests.Endpoints;

public sealed class IdentityEndpointTests(IntegrationEnvironmentFixture factory) :
    IClassFixture<IntegrationEnvironmentFixture>
{
    [Fact(DisplayName = "[e2e] - when POST identity/authenticate with valid credentials should return access token 'n refresh token")]
    public async Task WhenPostIdentityAuthenticateWithValidCredentials_ShouldReturnAccessTokenAndRefreshToken()
    {
        /* arrange: define credentials for an existing user */
        var httpClient = factory.HttpClient.WithTenantHeader("master");
        var credentials = new AuthenticationCredentials
        {
            Username = "vinder.testing.user",
            Password = "vinder.testing.password"
        };

        /* act: send POST request to authenticate endpoint */
        var response = await httpClient.PostAsJsonAsync("api/v1/identity/authenticate", credentials);
        var result = await response.Content.ReadFromJsonAsync<AuthenticationResult>();

        /* assert: response and result should not be null, and status code should be OK */
        Assert.NotNull(response);
        Assert.NotNull(result);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        /* assert: both access token and refresh token should be returned and not empty */
        Assert.NotEmpty(result.AccessToken);
        Assert.NotEmpty(result.RefreshToken);
    }
}
