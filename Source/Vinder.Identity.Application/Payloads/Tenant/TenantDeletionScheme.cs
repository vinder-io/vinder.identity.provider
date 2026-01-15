namespace Vinder.Identity.Application.Payloads.Tenant;

public sealed record TenantDeletionScheme : IRequest<Result>
{
    public string TenantId { get; init; } = default!;
}