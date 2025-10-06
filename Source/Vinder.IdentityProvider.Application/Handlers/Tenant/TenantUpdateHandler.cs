namespace Vinder.IdentityProvider.Application.Handlers.Tenant;

public sealed class TenantUpdateHandler(ITenantRepository repository) :
    IRequestHandler<TenantUpdateScheme, Result<TenantDetailsScheme>>
{
    public async Task<Result<TenantDetailsScheme>> Handle(TenantUpdateScheme request, CancellationToken cancellationToken)
    {
        var filters = new TenantFiltersBuilder()
            .WithId(request.TenantId)
            .Build();

        var tenants = await repository.GetTenantsAsync(filters, cancellation: cancellationToken);
        var tenant = tenants.FirstOrDefault();

        if (tenant is null)
        {
            return Result<TenantDetailsScheme>.Failure(TenantErrors.TenantDoesNotExist);
        }

        tenant = TenantMapper.AsTenant(request, tenant);

        var updatedTenant = await repository.UpdateAsync(tenant, cancellation: cancellationToken);

        return Result<TenantDetailsScheme>.Success(TenantMapper.AsResponse(updatedTenant));
    }
}