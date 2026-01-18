namespace Vinder.Federation.Application.Payloads.Identity;

public sealed record SessionTokenRenewalScheme : IMessage<Result<AuthenticationResult>>
{
    public string RefreshToken { get; init; } = default!;
}