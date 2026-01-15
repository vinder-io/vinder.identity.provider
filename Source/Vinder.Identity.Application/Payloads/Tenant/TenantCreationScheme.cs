namespace Vinder.Identity.Application.Payloads.Tenant;

public sealed record TenantCreationScheme : IMessage<Result<TenantDetailsScheme>>
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; } = default!;
}