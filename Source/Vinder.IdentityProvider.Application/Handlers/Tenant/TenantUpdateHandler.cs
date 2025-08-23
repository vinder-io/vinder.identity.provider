namespace Vinder.IdentityProvider.Application.Handlers.Tenant;

public sealed class TenantUpdateHandler(ITenantRepository repository) :
    IRequestHandler<TenantForUpdate, Result<TenantDetails>>
{
    public async Task<Result<TenantDetails>> Handle(TenantForUpdate request, CancellationToken cancellationToken)
    {
        var filters = new TenantFiltersBuilder()
            .WithName(request.Name)
            .Build();

        var tenants = await repository.GetTenantsAsync(filters, cancellation: cancellationToken);
        var tenant = tenants.FirstOrDefault();

        if (tenant is null)
        {
            return Result<TenantDetails>.Failure(TenantErrors.TenantDoesNotExist);
        }

        tenant = TenantMapper.AsTenant(request, tenant);

        var updatedTenant = await repository.UpdateAsync(tenant, cancellation: cancellationToken);

        return Result<TenantDetails>.Success(TenantMapper.AsResponse(updatedTenant));
    }
}