using Microsoft.IdentityModel.Tokens;

namespace Vinder.IdentityProvider.Infrastructure.Security;

public sealed class JwtSecurityTokenService(ISettings settings, ITokenRepository repository) : ISecurityTokenService
{
    private readonly TimeSpan _accessTokenDuration = TimeSpan.FromMinutes(15);
    private readonly TimeSpan _refreshTokenDuration = TimeSpan.FromDays(7);

    public Task<Result<SecurityToken>> GenerateAccessTokenAsync(User user, CancellationToken cancellation = default)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new ClaimsBuilder()
            .WithSubject(user.Id.ToString())
            .WithUsername(user.Username)
            .WithPermissions(user.Permissions)
            .Build();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Security.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claimsIdentity = new ClaimsIdentity(claims);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
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

        return Task.FromResult(Result<SecurityToken>.Success(securityToken));
    }

    public Task<Result<SecurityToken>> GenerateRefreshTokenAsync(User user, CancellationToken cancellation = default)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new ClaimsBuilder()
            .WithSubject(user.Id.ToString())
            .WithUsername(user.Username)
            .WithPermissions(user.Permissions)
            .Build();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Security.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claimsIdentity = new ClaimsIdentity(claims);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.Add(_refreshTokenDuration)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        var securityToken = new SecurityToken
        {
            Type = TokenType.Refresh,
            Value = tokenString,
            ExpiresAt = tokenDescriptor.Expires.Value
        };

        return Task.FromResult(Result<SecurityToken>.Success(securityToken));
    }

    public Task<Result> ValidateTokenAsync(SecurityToken token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Security.SecretKey));

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = securityKey,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            tokenHandler.ValidateToken(token.Value, validationParameters, out _);
            return Task.FromResult(Result.Success());
        }
        catch (SecurityTokenExpiredException)
        {
            return Task.FromResult(Result.Failure(AuthenticationErrors.TokenExpired));
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            return Task.FromResult(Result.Failure(AuthenticationErrors.InvalidSignature));
        }
        catch (ArgumentException)
        {
            return Task.FromResult(Result.Failure(AuthenticationErrors.InvalidTokenFormat));
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

        existingToken.Revoked = true;
        existingToken.IsDeleted = true;

        await repository.UpdateAsync(existingToken, cancellation);

        return Result.Success();
    }

    public Task<Result> ValidateAccessTokenAsync(SecurityToken token, CancellationToken cancellation = default)
        => ValidateTokenAsync(token);
    public Task<Result> ValidateRefreshTokenAsync(SecurityToken token, CancellationToken cancellation = default)
        => ValidateTokenAsync(token);
}