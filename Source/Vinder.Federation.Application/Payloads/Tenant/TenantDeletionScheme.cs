namespace Vinder.Federation.Application.Payloads.Tenant;

public sealed record TenantDeletionScheme : IMessage<Result>
{
    public string TenantId { get; init; } = default!;
}