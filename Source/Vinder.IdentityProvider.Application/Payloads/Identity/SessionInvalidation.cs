namespace Vinder.IdentityProvider.Application.Payloads.Identity;

public sealed record SessionInvalidation : IRequest<Result>
{
    public string RefreshToken { get; init; } = default!;
}