using System.Security.Cryptography;
using Vinder.IdentityProvider.Common.Configuration;
using Vinder.IdentityProvider.Common.Errors;
using Vinder.IdentityProvider.Infrastructure.Security;

namespace Vinder.IdentityProvider.TestSuite.IntegrationTests.Security;

public sealed class JwtSecurityTokenServiceTests : IClassFixture<MongoDatabaseFixture>, IAsyncLifetime
{
    private readonly JwtSecurityTokenService _jwtSecurityTokenService;
    private readonly ISettings _settings;
    private readonly ITokenRepository _tokenRepository;
    private readonly IMongoDatabase _database;
    private readonly MongoDatabaseFixture _mongoFixture;
    private readonly Fixture _fixture = new();

    public JwtSecurityTokenServiceTests(MongoDatabaseFixture fixture)
    {
        _mongoFixture = fixture;
        _database = fixture.Database;
        _tokenRepository = new TokenRepository(_database);

        var keyBytes = new byte[32];
        var randomNumberGenerator = RandomNumberGenerator.Create();

        randomNumberGenerator.GetBytes(keyBytes);

        var secretKey = Convert.ToBase64String(keyBytes);
        var securitySettings = new SecuritySettings { SecretKey = secretKey };

        _settings = new Settings { Security = securitySettings };
        _jwtSecurityTokenService = new JwtSecurityTokenService(_settings, _tokenRepository);
    }

    [Fact(DisplayName = "[infrastructure] - when generating an access token, then it must be valid and contain correct claims")]
    public async Task WhenGeneratingAccessToken_ThenItMustBeValidAndContainCorrectClaims()
    {
        /* arrange: create a user */
        var user = _fixture.Create<User>();

        /* act: generate an access token */
        var result = await _jwtSecurityTokenService.GenerateAccessTokenAsync(user);

        /* assert: token must be successful and valid */
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);

        var validationResult = await _jwtSecurityTokenService.ValidateTokenAsync(result.Data);

        Assert.True(validationResult.IsSuccess);
    }

    [Fact(DisplayName = "[infrastructure] - when generating a refresh token, then it must be valid and contain correct claims and be persisted")]
    public async Task WhenGeneratingRefreshToken_ThenItMustBeValidAndContainCorrectClaimsAndBePersisted()
    {
        /* arrange: create a user */
        var user = _fixture.Create<User>();

        /* act: generate a refresh token */
        var result = await _jwtSecurityTokenService.GenerateRefreshTokenAsync(user);

        /* assert: token must be successful and valid */
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);

        var validationResult = await _jwtSecurityTokenService.ValidateTokenAsync(result.Data);

        Assert.True(validationResult.IsSuccess);

        /* assert: refresh token must be persisted in the database */
        var filters = new TokenFiltersBuilder()
            .WithValue(result.Data.Value)
            .Build();

        var tokens = await _tokenRepository.GetTokensAsync(filters);

        Assert.Single(tokens);
        Assert.Equal(result.Data.Value, tokens.First().Value);
    }

    [Fact(DisplayName = "[infrastructure] - when validating an expired token, then it must return token expired error")]
    public async Task WhenValidatingExpiredToken_ThenItMustReturnTokenExpiredError()
    {
        /* arrange: create an expired token */
        var expiredToken = new SecurityToken
        {
            Value = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyLCJleHAiOjE1MTYyMzkwMjJ9.N-y_y_y_y_y_y_y_y_y_y_y_y_y_y_y_y_y_y_y_y_y_y_y_y_y_y_y_y_y_y_y_y", // This is a dummy expired token
            ExpiresAt = DateTime.UtcNow.AddMinutes(-1)
        };

        /* act: validate the expired token */
        var result = await _jwtSecurityTokenService.ValidateTokenAsync(expiredToken);

        /* assert: result must be a failure with TokenExpired error */
        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.TokenExpired, result.Error);
    }

    [Fact(DisplayName = "[infrastructure] - when validating a token with invalid format, then it must return invalid token format error")]
    public async Task WhenValidatingTokenWithInvalidFormat_ThenItMustReturnInvalidTokenFormatError()
    {
        /* arrange: create a token with invalid format */
        var invalidFormatToken = new SecurityToken
        {
            Value = "invalid-token-format",
            ExpiresAt = DateTime.UtcNow.AddMinutes(15)
        };

        /* act: validate the token with invalid format */
        var result = await _jwtSecurityTokenService.ValidateTokenAsync(invalidFormatToken);

        /* assert: result must be a failure with InvalidTokenFormat error */
        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.InvalidTokenFormat, result.Error);
    }

    [Fact(DisplayName = "[infrastructure] - when revoking a refresh token, then it must be marked as revoked and deleted in the database")]
    public async Task WhenRevokingRefreshToken_ThenItMustBeMarkedAsRevokedAndDeletedInTheDatabase()
    {
        /* arrange: create and insert a refresh token */
        var user = _fixture.Create<User>();
        var refreshTokenResult = await _jwtSecurityTokenService.GenerateRefreshTokenAsync(user);

        Assert.True(refreshTokenResult.IsSuccess);
        Assert.NotNull(refreshTokenResult.Data);

        var refreshToken = refreshTokenResult.Data;

        /* act: revoke the refresh token */
        var revokeResult = await _jwtSecurityTokenService.RevokeRefreshTokenAsync(refreshToken);

        /* assert: revoke must be successful */
        Assert.True(revokeResult.IsSuccess);

        /* assert: refresh token must be marked as revoked and deleted in the database */
        var filters = new TokenFiltersBuilder()
            .WithValue(refreshToken.Value)
            .WithIsDeleted(true)
            .Build();

        var tokens = await _tokenRepository.GetTokensAsync(filters);

        Assert.Single(tokens);

        Assert.True(tokens.First().Revoked);
        Assert.True(tokens.First().IsDeleted);
    }

    [Fact(DisplayName = "[infrastructure] - when revoking a non-existent refresh token, then it must return invalid refresh token error")]
    public async Task WhenRevokingNonExistentRefreshToken_ThenItMustReturnInvalidRefreshTokenError()
    {
        /* arrange: create a non-existent refresh token */
        var nonExistentToken = _fixture.Create<SecurityToken>();

        /* act: revoke the non-existent refresh token */
        var revokeResult = await _jwtSecurityTokenService.RevokeRefreshTokenAsync(nonExistentToken);

        /* assert: revoke must be a failure with InvalidRefreshToken error */
        Assert.True(revokeResult.IsFailure);
        Assert.Equal(AuthenticationErrors.InvalidRefreshToken, revokeResult.Error);
    }

    [Fact(DisplayName = "[infrastructure] - ValidateAccessTokenAsync should call ValidateTokenAsync")]
    public async Task ValidateAccessTokenAsync_ShouldCallValidateTokenAsync()
    {
        /* arrange: create a valid token */
        var user = _fixture.Create<User>();
        var accessTokenResult = await _jwtSecurityTokenService.GenerateAccessTokenAsync(user);

        Assert.True(accessTokenResult.IsSuccess);
        Assert.NotNull(accessTokenResult.Data);

        var accessToken = accessTokenResult.Data;

        /* act: validate access token */
        var result = await _jwtSecurityTokenService.ValidateAccessTokenAsync(accessToken);

        /* assert: result must be successful */
        Assert.True(result.IsSuccess);
    }

    [Fact(DisplayName = "[infrastructure] - ValidateRefreshTokenAsync should call ValidateTokenAsync")]
    public async Task ValidateRefreshTokenAsync_ShouldCallValidateTokenAsync()
    {
        /* arrange: create a valid token */
        var user = _fixture.Create<User>();

        var refreshTokenResult = await _jwtSecurityTokenService.GenerateRefreshTokenAsync(user);
        var refreshToken = refreshTokenResult.Data;

        /* act: validate refresh token */
        var result = await _jwtSecurityTokenService.ValidateRefreshTokenAsync(refreshToken!);

        /* assert: result must be successful */
        Assert.True(result.IsSuccess);
    }

    public async Task DisposeAsync() => await Task.CompletedTask;
    public async Task InitializeAsync()
    {
        await _mongoFixture.CleanDatabaseAsync();
    }
}