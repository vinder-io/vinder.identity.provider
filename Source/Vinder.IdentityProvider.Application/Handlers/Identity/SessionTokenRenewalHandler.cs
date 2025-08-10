namespace Vinder.IdentityProvider.Application.Handlers.Identity;

public sealed class SessionTokenRenewalHandler(IUserRepository userRepository, ISecurityTokenService tokenService) :
    IRequestHandler<SessionTokenRenewal, Result<AuthenticationResult>>
{
    public async Task<Result<AuthenticationResult>> Handle(SessionTokenRenewal request, CancellationToken cancellationToken)
    {
        var refreshToken = new SecurityToken
        {
            Value = request.RefreshToken
        };

        var validationResult = await tokenService.ValidateRefreshTokenAsync(refreshToken, cancellationToken);
        if (validationResult.IsFailure)
        {
            return Result<AuthenticationResult>.Failure(validationResult.Error);
        }

        var revokeResult = await tokenService.RevokeRefreshTokenAsync(refreshToken, cancellationToken);
        if (revokeResult.IsFailure)
        {
            return Result<AuthenticationResult>.Failure(revokeResult.Error);
        }

        var userFilters = new UserFiltersBuilder()
            .WithSecurityToken(refreshToken.Value)
            .Build();

        var users = await userRepository.GetUsersAsync(userFilters, cancellation: cancellationToken);
        var user = users.FirstOrDefault();

        if (user is null)
        {
            return Result<AuthenticationResult>.Failure(AuthenticationErrors.UserNotFound);
        }

        var accessTokenResult = await tokenService.GenerateAccessTokenAsync(user, cancellationToken);
        if (accessTokenResult.IsFailure)
        {
            return Result<AuthenticationResult>.Failure(accessTokenResult.Error);
        }

        var refreshTokenResult = await tokenService.GenerateRefreshTokenAsync(user, cancellationToken);
        if (refreshTokenResult.IsFailure)
        {
            return Result<AuthenticationResult>.Failure(refreshTokenResult.Error);
        }

        var renewedAccessToken = accessTokenResult.Data!.Value;
        var renewedRefreshToken = refreshTokenResult.Data!.Value;

        var response = new AuthenticationResult
        {
            AccessToken = renewedAccessToken,
            RefreshToken = renewedRefreshToken
        };

        return Result<AuthenticationResult>.Success(response);
    }
}
