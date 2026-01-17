namespace Vinder.Identity.Domain.Policies;

public interface IRedirectUriPolicy
{
    Task<Result> EnsureRedirectUriIsAllowedAsync(
        Tenant tenant,
        RedirectUri redirectUri,
        CancellationToken cancellation = default
);
}