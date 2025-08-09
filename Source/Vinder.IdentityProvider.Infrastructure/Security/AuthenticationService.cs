namespace Vinder.IdentityProvider.Infrastructure.Security;

public sealed class AuthenticationService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ISecurityTokenService tokenService
) : IAuthenticationService
{
    public async Task<Result<AuthenticationResult>> AuthenticateAsync(AuthenticationCredentials credentials, CancellationToken cancellation = default)
    {
        var filters = new UserFiltersBuilder()
            .WithUsername(credentials.Username)
            .Build();

        var users = await userRepository.GetUsersAsync(filters, cancellation: cancellation);
        var user = users.FirstOrDefault();

        if (user is null)
        {
            return Result<AuthenticationResult>.Failure(AuthenticationErrors.UserNotFound);
        }

        if (!await passwordHasher.VerifyPasswordAsync(credentials.Password, user.PasswordHash))
        {
            return Result<AuthenticationResult>.Failure(AuthenticationErrors.InvalidCredentials);
        }

        var accessTokenResult = await tokenService.GenerateAccessTokenAsync(user, cancellation);
        if (!accessTokenResult.IsSuccess)
        {
            return Result<AuthenticationResult>.Failure(accessTokenResult.Error);
        }

        var refreshTokenResult = await tokenService.GenerateRefreshTokenAsync(user, cancellation);
        if (!refreshTokenResult.IsSuccess)
        {
            return Result<AuthenticationResult>.Failure(refreshTokenResult.Error);
        }

        if (accessTokenResult.Data is null || refreshTokenResult.Data is null)
        {
            return Result<AuthenticationResult>.Failure(AuthenticationErrors.InvalidTokenFormat);
        }

        var result = new AuthenticationResult
        {
            AccessToken = accessTokenResult.Data.Value,
            RefreshToken = refreshTokenResult.Data.Value
        };

        return Result<AuthenticationResult>.Success(result);
    }
}