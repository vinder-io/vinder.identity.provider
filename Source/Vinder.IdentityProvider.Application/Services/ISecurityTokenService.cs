namespace Vinder.IdentityProvider.Application.Services;

public interface ISecurityTokenService
{
    public Task<string> GenerateAccessTokenAsync(User user, CancellationToken cancellation = default);
    public Task<string> GenerateRefreshTokenAsync(User user, CancellationToken cancellation = default);

    public Task<bool> ValidateAccessTokenAsync(string token, CancellationToken cancellation = default);
    public Task<bool> ValidateRefreshTokenAsync(string token, CancellationToken cancellation = default);

    public Task RevokeRefreshTokenAsync(string token, CancellationToken cancellation = default);
}