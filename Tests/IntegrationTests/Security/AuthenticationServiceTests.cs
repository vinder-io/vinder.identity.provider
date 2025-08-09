using System.Security.Cryptography;

namespace Vinder.IdentityProvider.TestSuite.IntegrationTests.Security;

public sealed class AuthenticationServiceTests : IClassFixture<MongoDatabaseFixture>, IAsyncLifetime
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ISecurityTokenService _tokenService;
    private readonly ISettings _settings;
    private readonly IMongoDatabase _database;
    private readonly MongoDatabaseFixture _mongoFixture;
    private readonly AuthenticationService _authenticationService;
    private readonly Fixture _fixture = new();

    public AuthenticationServiceTests(MongoDatabaseFixture mongoFixture)
    {
        _mongoFixture = mongoFixture;
        _database = mongoFixture.Database;

        _userRepository = new UserRepository(_database);
        _passwordHasher = new PasswordHasher();

        var tokenRepository = new TokenRepository(_database);

        var keyBytes = new byte[32];
        var randomNumberGenerator = RandomNumberGenerator.Create();

        randomNumberGenerator.GetBytes(keyBytes);

        var secretKey = Convert.ToBase64String(keyBytes);
        var securitySettings = new SecuritySettings { SecretKey = secretKey };

        _settings = new Settings { Security = securitySettings };

        _tokenService = new JwtSecurityTokenService(_settings, tokenRepository);
        _authenticationService = new AuthenticationService(_userRepository, _passwordHasher, _tokenService);
    }

    [Fact(DisplayName = "[security] - when valid credentials are provided, then returns access and refresh tokens")]
    public async Task WhenValidCredentialsAreProvided_ThenReturnsAccessAndRefreshTokens()
    {
        /* arrange: create a user with a known password and insert into repository */
        var plainPassword = "My$ecureP@ssw0rd";
        var user = _fixture.Build<User>()
            .With(user => user.PasswordHash, await _passwordHasher.HashPasswordAsync(plainPassword))
            .With(user => user.IsDeleted, false)
            .Create();

        await _userRepository.InsertAsync(user);

        /* act: authenticate with valid credentials */
        var credentials = new AuthenticationCredentials
        {
            Username = user.Username,
            Password = plainPassword
        };

        var result = await _authenticationService.AuthenticateAsync(credentials);

        /* assert: authentication should be successful and tokens should be returned */
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);

        Assert.NotNull(result.Data.AccessToken);
        Assert.NotNull(result.Data.RefreshToken);
    }

    [Fact(DisplayName = "[security] - when invalid password is provided, then returns error (#VINDER-IDP-ERR-AUT-401)")]
    public async Task AuthenticateAsync_InvalidPassword_ReturnsInvalidCredentialsError()
    {
        /* arrange: create a user with a known password */
        var plainPassword = "My$ecureP@ssw0rd";
        var user = _fixture.Build<User>()
            .With(user => user.PasswordHash, await _passwordHasher.HashPasswordAsync(plainPassword))
            .With(user => user.IsDeleted, false)
            .Create();

        await _userRepository.InsertAsync(user);

        var credentials = new AuthenticationCredentials
        {
            Username = user.Username,
            Password = "wrong-password"
        };

        /* act: attempt to authenticate with wrong password */
        var result = await _authenticationService.AuthenticateAsync(credentials);

        /* assert: it should fail with invalid credentials error */
        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.InvalidCredentials, result.Error);
    }

    [Fact(DisplayName = "[security] - when user does not exist, then returns user not found error (#VINDER-IDP-ERR-AUT-404)")]
    public async Task WhenUserDoesNotExist_ThenReturnsUserNotFoundError()
    {
        /* arrange: create credentials with a username that does not exist */
        var credentials = new AuthenticationCredentials
        {
            Username = "nonexistentuser",
            Password = "any-password"
        };

        /* act: attempt to authenticate with non-existent user */
        var result = await _authenticationService.AuthenticateAsync(credentials);

        /* assert: it should fail with user not found error */
        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.UserNotFound, result.Error);
    }

    public async Task InitializeAsync() => await _mongoFixture.CleanDatabaseAsync();
    public async Task DisposeAsync() => await Task.CompletedTask;
}
