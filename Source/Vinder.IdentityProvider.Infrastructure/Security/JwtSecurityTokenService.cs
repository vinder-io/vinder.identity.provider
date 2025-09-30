namespace Vinder.IdentityProvider.Infrastructure.Security;

public sealed class JwtSecurityTokenService(
    ISecretRepository secretRepository,
    ITokenRepository repository,
    IHostInformationProvider host
) : ISecurityTokenService
{
    private readonly TimeSpan _accessTokenDuration = TimeSpan.FromMinutes(15);
    private readonly TimeSpan _refreshTokenDuration = TimeSpan.FromDays(7);

    public async Task<Result<SecurityToken>> GenerateAccessTokenAsync(User user, CancellationToken cancellation = default)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new ClaimsBuilder()
            .WithSubject(user.Id.ToString())
            .WithUsername(user.Username)
            .WithPermissions(user.Permissions)
            .Build();

        var privateKey = await GetPrivateKeyAsync(cancellation);
        var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);

        var claimsIdentity = new ClaimsIdentity(claims);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Issuer = host.Address.ToString(),
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.Add(_accessTokenDuration),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        var securityToken = new SecurityToken
        {
            Value = tokenString,
            ExpiresAt = tokenDescriptor.Expires.Value
        };

        return Result<SecurityToken>.Success(securityToken);
    }

    public async Task<Result<SecurityToken>> GenerateAccessTokenAsync(Tenant tenant, CancellationToken cancellation = default)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new ClaimsBuilder()
            .WithTenantId(tenant.Id.ToString())
            .WithTenantName(tenant.Name)
            .WithClientId(tenant.ClientId)
            .WithPermissions(tenant.Permissions)
            .Build();

        var privateKey = await GetPrivateKeyAsync(cancellation);
        var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);

        var claimsIdentity = new ClaimsIdentity(claims);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = host.Address.ToString(),
            Audience = tenant.Name,
            Subject = claimsIdentity,
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.Add(_accessTokenDuration)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        var securityToken = new SecurityToken
        {
            Value = tokenString,
            ExpiresAt = tokenDescriptor.Expires.Value,
        };

        return Result<SecurityToken>.Success(securityToken);
    }

    public async Task<Result<SecurityToken>> GenerateRefreshTokenAsync(User user, CancellationToken cancellation = default)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new ClaimsBuilder()
            .WithSubject(user.Id.ToString())
            .WithUsername(user.Username)
            .WithPermissions(user.Permissions)
            .Build();

        var privateKey = await GetPrivateKeyAsync(cancellation);
        var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);

        var claimsIdentity = new ClaimsIdentity(claims);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Issuer = host.Address.ToString(),
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.Add(_refreshTokenDuration)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        var securityToken = new SecurityToken
        {
            Type = TokenType.Refresh,
            UserId = user.Id,
            TenantId = user.TenantId,
            Value = tokenString,
            ExpiresAt = tokenDescriptor.Expires.Value,
        };

        await repository.InsertAsync(securityToken, cancellation);

        return Result<SecurityToken>.Success(securityToken);
    }

    public async Task<Result> ValidateTokenAsync(SecurityToken token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var publicKey = await GetPublicKeyAsync();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = publicKey,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            tokenHandler.ValidateToken(token.Value, validationParameters, out _);
            return Result.Success();
        }
        catch (SecurityTokenExpiredException)
        {
            return Result.Failure(AuthenticationErrors.TokenExpired);
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            return Result.Failure(AuthenticationErrors.InvalidSignature);
        }
        catch (ArgumentException)
        {
            return Result.Failure(AuthenticationErrors.InvalidTokenFormat);
        }
    }

    public async Task<Result> RevokeRefreshTokenAsync(SecurityToken token, CancellationToken cancellation = default)
    {
        var filters = new TokenFiltersBuilder()
            .WithValue(token.Value)
            .WithType(TokenType.Refresh)
            .Build();

        var tokens = await repository.GetTokensAsync(filters, cancellation);
        var existingToken = tokens.FirstOrDefault();

        if (existingToken is null)
        {
            return Result.Failure(AuthenticationErrors.InvalidRefreshToken);
        }

        if (existingToken.Revoked)
        {
            return Result.Failure(AuthenticationErrors.LogoutFailed);
        }

        existingToken.Revoked = true;
        existingToken.IsDeleted = true;

        await repository.UpdateAsync(existingToken, cancellation);

        return Result.Success();
    }

    public Task<Result> ValidateAccessTokenAsync(SecurityToken token, CancellationToken cancellation = default)
        => ValidateTokenAsync(token);
    public Task<Result> ValidateRefreshTokenAsync(SecurityToken token, CancellationToken cancellation = default)
        => ValidateTokenAsync(token);

    private async Task<RsaSecurityKey> GetPrivateKeyAsync(CancellationToken cancellation = default)
    {
        var secret = await secretRepository.GetSecretAsync(cancellation);
        return RsaKeyHelper.FromPrivateKey(secret.PrivateKey);
    }

    private async Task<RsaSecurityKey> GetPublicKeyAsync(CancellationToken cancellation = default)
    {
        var secret = await secretRepository.GetSecretAsync(cancellation);
        return RsaKeyHelper.FromPublicKey(secret.PublicKey);
    }
}