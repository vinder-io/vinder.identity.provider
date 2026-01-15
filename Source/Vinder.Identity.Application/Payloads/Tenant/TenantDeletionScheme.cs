namespace Vinder.Identity.Application.Payloads.Tenant;

public sealed record TenantDeletionScheme : IMessage<Result>
{
    public string TenantId { get; init; } = default!;
}