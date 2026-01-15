namespace Vinder.Identity.Application.Handlers.Identity;

public sealed class SessionInvalidationHandler(ISecurityTokenService tokenService) :
    IRequestHandler<SessionInvalidationScheme, Result>
{
    public async Task<Result> Handle(SessionInvalidationScheme request, CancellationToken cancellationToken)
    {
        var refreshToken = TokenMapper.AsRefreshToken(request.RefreshToken);
        var validationResult = await tokenService.ValidateRefreshTokenAsync(refreshToken, cancellationToken);

        if (validationResult.IsFailure)
        {
            return Result.Failure(validationResult.Error);
        }

        var revokeResult = await tokenService.RevokeRefreshTokenAsync(refreshToken, cancellationToken);
        if (revokeResult.IsFailure)
        {
            return Result.Failure(revokeResult.Error);
        }

        return Result.Success();
    }
}