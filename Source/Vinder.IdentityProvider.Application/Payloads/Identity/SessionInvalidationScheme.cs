namespace Vinder.IdentityProvider.Application.Payloads.Identity;

public sealed record SessionInvalidationScheme : IRequest<Result>
{
    public string RefreshToken { get; init; } = default!;
}