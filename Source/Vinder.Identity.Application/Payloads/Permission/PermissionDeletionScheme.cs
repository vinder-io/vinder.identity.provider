namespace Vinder.Identity.Application.Payloads.Permission;

public sealed record PermissionDeletionScheme : IRequest<Result>
{
    public string PermissionId { get; init; } = default!;
}
