namespace Vinder.IdentityProvider.TestSuite.IntegrationTests.Endpoints;

public sealed class IdentityEndpointTests(IntegrationEnvironmentFixture factory) :
    IClassFixture<IntegrationEnvironmentFixture>
{
    private readonly Fixture _fixture = new();

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

    [Fact(DisplayName = "[e2e] - when valid refresh token is provided should return new access token")]
    public async Task WhenValidRefreshTokenProvided_ShouldReturnNewAccessToken()
    {
        /* arrange: authenticate user and get refresh token */
        var httpClient = factory.HttpClient.WithTenantHeader("master");
        var credentials = new AuthenticationCredentials
        {
            Username = "vinder.testing.user",
            Password = "vinder.testing.password"
        };

        var authenticationResponse = await httpClient.PostAsJsonAsync("api/v1/identity/authenticate", credentials);
        var authenticationResult = await authenticationResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

        /* act: use refresh token to get a new access token */
        var refreshRequest = new SessionTokenRenewal
        {
            RefreshToken = authenticationResult!.RefreshToken
        };

        var refreshResponse = await httpClient.PostAsJsonAsync("api/v1/identity/refresh-token", refreshRequest);
        var refreshResult = await refreshResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

        /* assert: response should be OK and new access token should be returned */
        Assert.NotNull(refreshResponse);
        Assert.NotNull(refreshResult);

        Assert.Equal(HttpStatusCode.OK, refreshResponse.StatusCode);

        /* assert: both access token and refresh token should be returned and not empty */
        Assert.NotEmpty(refreshResult.AccessToken);
        Assert.NotEmpty(refreshResult.RefreshToken);
    }

    [Fact(DisplayName = "[e2e] - when invalid refresh token is provided should return 401 #VINDER-IDP-ERR-AUT-405")]
    public async Task WhenInvalidRefreshTokenProvided_ShouldReturnBadRequest()
    {
        /* arrange: prepare an invalid refresh token */
        var httpClient = factory.HttpClient.WithTenantHeader("master");
        var refreshRequest = new SessionTokenRenewal
        {
            RefreshToken = "this-is-an-invalid-token"
        };

        /* act: call refresh-token endpoint */
        var refreshResponse = await httpClient.PostAsJsonAsync("api/v1/identity/refresh-token", refreshRequest);
        var error = await refreshResponse.Content.ReadFromJsonAsync<Error>();

        /* assert: response should be 401 and error should match ERR-AUT-410 */
        Assert.Equal(HttpStatusCode.Unauthorized, refreshResponse.StatusCode);

        Assert.NotNull(error);
        Assert.Equal("#VINDER-IDP-ERR-AUT-410", error.Code);
    }

    [Fact(DisplayName = "[e2e] - when refresh token is revoked should return 400 BadRequest #VINDER-IDP-ERR-AUT-405")]
    public async Task WhenRefreshTokenRevoked_ShouldReturnBadRequest()
    {
        /* arrange: authenticate user and get access & refresh tokens */
        var httpClient = factory.HttpClient.WithTenantHeader("master");
        var credentials = new AuthenticationCredentials
        {
            Username = "vinder.testing.user",
            Password = "vinder.testing.password"
        };

        var authenticationResponse = await httpClient.PostAsJsonAsync("api/v1/identity/authenticate", credentials);
        var authenticationResult = await authenticationResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

        Assert.NotNull(authenticationResult);
        Assert.NotEmpty(authenticationResult.AccessToken);
        Assert.NotEmpty(authenticationResult.RefreshToken);

        /* arrange: revoke the refresh token using ISecurityTokenService */
        var scope = factory.Services.CreateScope();
        var tokenService = scope.ServiceProvider.GetRequiredService<ISecurityTokenService>();

        var refreshToken = _fixture.Build<SecurityToken>()
            .With(token => token.Value, authenticationResult.RefreshToken)
            .With(token => token.Type, TokenType.Refresh)
            .Create();

        var revokeResult = await tokenService.RevokeRefreshTokenAsync(refreshToken);

        Assert.True(revokeResult.IsSuccess, "Refresh token should be revoked successfully");

        /* act: attempt to renew session with the revoked refresh token */
        var refreshRequest = _fixture.Build<SessionTokenRenewal>()
            .With(payload => payload.RefreshToken, authenticationResult.RefreshToken)
            .Create();

        var refreshResponse = await httpClient.PostAsJsonAsync("api/v1/identity/refresh-token", refreshRequest);
        var error = await refreshResponse.Content.ReadFromJsonAsync<Error>();

        /* assert: endpoint should reject revoked token */
        Assert.Equal(HttpStatusCode.BadRequest, refreshResponse.StatusCode);
        Assert.NotNull(error);

        Assert.Equal("#VINDER-IDP-ERR-AUT-405", error.Code);
    }
}
