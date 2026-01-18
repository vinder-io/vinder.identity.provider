namespace Vinder.Federation.Application.Payloads.Identity;

public sealed record SessionInvalidationScheme : IMessage<Result>
{
    public string RefreshToken { get; init; } = default!;
}