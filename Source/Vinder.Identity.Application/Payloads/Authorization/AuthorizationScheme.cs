namespace Vinder.Identity.Application.Payloads.Authorization;

public sealed record AuthorizationScheme
{
    public string RedirectUri { get; init; } = default!;
    public string Code { get; init; } = default!;
    public string? State { get; init; } = default!;
}