namespace Vinder.IdentityProvider.Application.Payloads.User;

public sealed record UserDetails
{
    public string Id { get; init; } = default!;
    public string Username { get; init; } = default!;

    public IReadOnlyCollection<GroupDetails> Groups { get; init; } = [  ];
    public IReadOnlyCollection<PermissionDetails> Permissions { get; init; } = [  ];
}