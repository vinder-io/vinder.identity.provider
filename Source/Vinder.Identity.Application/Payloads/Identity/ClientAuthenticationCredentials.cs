namespace Vinder.Identity.Application.Payloads.Identity;

public sealed record ClientAuthenticationCredentials : IRequest<Result<ClientAuthenticationResult>>
{
    public string GrantType { get; init; } = default!;
    public string ClientId { get; init; } = default!;
    public string ClientSecret { get; init; } = default!;
}