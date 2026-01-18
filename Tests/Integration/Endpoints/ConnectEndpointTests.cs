namespace Vinder.Federation.TestSuite.Integration.Endpoints;

public sealed class ConnectEndpointTests(IntegrationEnvironmentFixture factory) :
    IClassFixture<IntegrationEnvironmentFixture>
{
    private readonly Fixture _fixture = new();

    [Fact(DisplayName = "[e2e] - when POST /openid/connect/token with valid tenant credentials should return access token")]
    public async Task WhenPostTokenWithValidTenantCredentials_ShouldReturnAccessToken()
    {
        /* arrange: authenticate user and get access token */
        var httpClient = factory.HttpClient.WithTenantHeader("master");
        var userCredentials = new AuthenticationCredentials
        {
            Username = "vinder.testing.user",
            Password = "vinder.testing.password"
        };

        var authenticationResponse = await httpClient.PostAsJsonAsync("api/v1/identity/authenticate", userCredentials);
        var authentication = await authenticationResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

        Assert.NotNull(authentication);
        Assert.NotEmpty(authentication.AccessToken);

        httpClient.WithAuthorization(authentication.AccessToken);

        /* arrange: create a tenant to use as client */
        var payload = _fixture.Build<TenantCreationScheme>()
            .With(tenant => tenant.Name, $"test-tenant-{Guid.NewGuid()}")
            .With(tenant => tenant.Description, $"test-description-{Guid.NewGuid()}")
            .Create();

        var tenantResponse = await httpClient.PostAsJsonAsync("api/v1/tenants", payload);
        var tenant = await tenantResponse.Content.ReadFromJsonAsync<TenantDetailsScheme>();

        Assert.NotNull(tenant);
        Assert.Equal(HttpStatusCode.Created, tenantResponse.StatusCode);

        /* arrange: prepare client credentials using tenant data */
        var credentials = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", tenant.ClientId },
            { "client_secret", tenant.ClientSecret }
        };

        var content = new FormUrlEncodedContent(credentials);
        var connectClient = factory.HttpClient;

        /* act: send POST request to token endpoint */
        var response = await connectClient.PostAsync("api/v1/protocol/open-id/connect/token", content);
        var grantedToken = await response.Content.ReadFromJsonAsync<ClientAuthenticationResult>();

        /* assert: response should be 200 OK */
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(grantedToken);
        Assert.False(string.IsNullOrWhiteSpace(grantedToken.AccessToken));
    }

    [Fact(DisplayName = "[e2e] - when POST /openid/connect/token with non-existent client should return 401 #VINDER-IDP-ERR-AUTH-402")]
    public async Task WhenPostTokenWithNonExistentClient_ShouldReturnUnauthorized()
    {
        /* arrange: prepare credentials with non-existent client */
        var httpClient = factory.HttpClient;
        var credentials = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", $"non-existent-client-{Guid.NewGuid()}" },
            { "client_secret", "some-secret" }
        };

        var content = new FormUrlEncodedContent(credentials);

        /* act: send POST request with non-existent client */
        var response = await httpClient.PostAsync("api/v1/protocol/open-id/connect/token", content);
        var error = await response.Content.ReadFromJsonAsync<Error>();

        /* assert: response should be 401 Unauthorized */
        Assert.NotNull(error);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal(AuthenticationErrors.ClientNotFound, error);
    }

    [Fact(DisplayName = "[e2e] - when POST /openid/connect/token with invalid client secret should return 401 #VINDER-IDP-ERR-AUTH-403")]
    public async Task WhenPostTokenWithInvalidClientSecret_ShouldReturnUnauthorized()
    {
        /* arrange: authenticate user and get access token */
        var httpClient = factory.HttpClient.WithTenantHeader("master");
        var userCredentials = new AuthenticationCredentials
        {
            Username = "vinder.testing.user",
            Password = "vinder.testing.password"
        };

        var authenticationResponse = await httpClient.PostAsJsonAsync("api/v1/identity/authenticate", userCredentials);
        var authentication = await authenticationResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

        Assert.NotNull(authentication);

        httpClient.WithAuthorization(authentication.AccessToken);

        /* arrange: create a tenant */
        var payload = _fixture.Build<TenantCreationScheme>()
            .With(tenant => tenant.Name, $"test-tenant-{Guid.NewGuid()}")
            .With(tenant => tenant.Description, $"test-description-{Guid.NewGuid()}.com")
            .Create();

        var httpResponse = await httpClient.PostAsJsonAsync("api/v1/tenants", payload);
        var tenant = await httpResponse.Content.ReadFromJsonAsync<TenantDetailsScheme>();

        Assert.NotNull(tenant);

        /* arrange: prepare credentials with wrong secret */
        var credentials = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", tenant.ClientId },
            { "client_secret", "wrong-secret" }
        };

        /* act: send POST request with invalid secret */

        var content = new FormUrlEncodedContent(credentials);
        var connectClient = factory.HttpClient;

        var response = await connectClient.PostAsync("api/v1/protocol/open-id/connect/token", content);
        var error = await response.Content.ReadFromJsonAsync<Error>();

        /* assert: response should be 401 Unauthorized */
        Assert.NotNull(error);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal(AuthenticationErrors.InvalidClientCredentials, error);
    }

    [Fact(DisplayName = "[e2e] - when POST /openid/connect/token with missing grant_type should return 400")]
    public async Task WhenPostTokenWithMissingGrantType_ShouldReturnBadRequest()
    {
        /* arrange: prepare credentials without grant_type */
        var httpClient = factory.HttpClient;
        var credentials = new Dictionary<string, string>
        {
            { "client_id", "test-client-id" },
            { "client_secret", "test-client-secret" }
        };

        /* act: send POST request without grant_type */

        var content = new FormUrlEncodedContent(credentials);
        var response = await httpClient.PostAsync("api/v1/protocol/open-id/connect/token", content);

        /* assert: response should be 400 Bad Request */
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(DisplayName = "[e2e] - when POST /openid/connect/token with missing client_id should return 400")]
    public async Task WhenPostTokenWithMissingClientId_ShouldReturnBadRequest()
    {
        /* arrange: prepare credentials without client_id */
        var httpClient = factory.HttpClient;
        var credentials = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_secret", "test-client-secret" }
        };

        /* act: send POST request without client_id */

        var content = new FormUrlEncodedContent(credentials);
        var response = await httpClient.PostAsync("api/v1/protocol/open-id/connect/token", content);

        /* assert: response should be 400 Bad Request */
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(DisplayName = "[e2e] - when POST /openid/connect/token with missing client_secret should return 400")]
    public async Task WhenPostTokenWithMissingClientSecret_ShouldReturnBadRequest()
    {
        /* arrange: prepare credentials without client_secret */
        var httpClient = factory.HttpClient;
        var credentials = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", "test-client-id" }
        };

        /* act: send POST request without client_secret */

        var content = new FormUrlEncodedContent(credentials);
        var response = await httpClient.PostAsync("api/v1/protocol/open-id/connect/token", content);

        /* assert: response should be 400 Bad Request */
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}