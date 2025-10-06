namespace Vinder.IdentityProvider.Application.Payloads.Tenant;

public sealed record TenantCreationScheme : IRequest<Result<TenantDetailsScheme>>
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; } = default!;
}