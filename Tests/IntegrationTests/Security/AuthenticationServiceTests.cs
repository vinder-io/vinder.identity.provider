using System.Security.Cryptography;

namespace Vinder.IdentityProvider.TestSuite.IntegrationTests.Security;

public sealed class AuthenticationServiceTests :
    IClassFixture<MongoDatabaseFixture>, IAsyncLifetime
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;
    private readonly ISecurityTokenService _tokenService;
    private readonly IMongoDatabase _database;
    private readonly MongoDatabaseFixture _mongoFixture;
    private readonly AuthenticationService _authenticationService;
    private readonly Fixture _fixture = new();
    private readonly RSA _rsa = RSA.Create(2048);

    private readonly Mock<ITenantProvider> _tenantProvider = new();
    private readonly Mock<IHostInformationProvider> _hostProvider = new();
    private readonly Mock<ISecretRepository> _secretRepository = new();
    private readonly Mock<IGroupRepository> _groupRepository = new();

    public AuthenticationServiceTests(MongoDatabaseFixture mongoFixture)
    {
        _mongoFixture = mongoFixture;
        _database = mongoFixture.Database;

        _userRepository = new UserRepository(_database, _tenantProvider.Object);
        _passwordHasher = new PasswordHasher();

        var tokenRepository = new TokenRepository(_database, _tenantProvider.Object);
        var secret = new Secret
        {
            PrivateKey = Convert.ToBase64String(_rsa.ExportRSAPrivateKey()),
            PublicKey  = Convert.ToBase64String(_rsa.ExportRSAPublicKey())
        };

        var tenant = _fixture.Create<Tenant>();

        _secretRepository
            .Setup(repository => repository.GetSecretAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(secret);

        _groupRepository
            .Setup(repository => repository.GetGroupsAsync(It.IsAny<GroupFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([  ]);

        _hostProvider.Setup(provider => provider.Address)
            .Returns(new Uri("http://localhost:5078"));

        _tenantProvider.Setup(provider => provider.GetCurrentTenant())
            .Returns(tenant);

        _tokenService = new JwtSecurityTokenService(
            secretRepository: _secretRepository.Object,
            repository: tokenRepository,
            tenantProvider: _tenantProvider.Object,
            groupRepository: _groupRepository.Object,
            host: _hostProvider.Object
        );

        _authenticationService = new AuthenticationService(
            userRepository: _userRepository,
            passwordHasher: _passwordHasher,
            tokenService: _tokenService
        );
    }

    [Fact(DisplayName = "[security] - when valid credentials are provided, then returns access and refresh tokens")]
    public async Task WhenValidCredentialsAreProvided_ThenReturnsAccessAndRefreshTokens()
    {
        /* arrange: create a user with a known password and insert into repository */
        var tenant = _fixture.Create<Tenant>();

        _tenantProvider.Setup(provider => provider.GetCurrentTenant())
            .Returns(tenant);

        var plainPassword = "My$ecureP@ssw0rd";
        var user = _fixture.Build<User>()
            .With(user => user.PasswordHash, await _passwordHasher.HashPasswordAsync(plainPassword))
            .With(user => user.TenantId, tenant.Id)
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
        var tenant = _fixture.Create<Tenant>();

        _tenantProvider.Setup(provider => provider.GetCurrentTenant())
            .Returns(tenant);

        var plainPassword = "My$ecureP@ssw0rd";
        var user = _fixture.Build<User>()
            .With(user => user.PasswordHash, await _passwordHasher.HashPasswordAsync(plainPassword))
            .With(user => user.IsDeleted, false)
            .With(user => user.TenantId, tenant.Id)
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
        var tenant = _fixture.Create<Tenant>();

        _tenantProvider.Setup(provider => provider.GetCurrentTenant())
            .Returns(tenant);

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
        Assert.Equal(AuthenticationErrors.InvalidCredentials, result.Error);
    }

    public async Task InitializeAsync() => await _mongoFixture.CleanDatabaseAsync();
    public async Task DisposeAsync() => await Task.CompletedTask;
}
