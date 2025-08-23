namespace Vinder.IdentityProvider.Application.Handlers.Tenant;

public sealed class TenantDeletionHandler(ITenantRepository repository) : IRequestHandler<TenantForDeletion, Result>
{
    public async Task<Result> Handle(TenantForDeletion request, CancellationToken cancellationToken)
    {
        var filters = new TenantFiltersBuilder()
            .WithId(request.TenantId)
            .Build();

        var tenants = await repository.GetTenantsAsync(filters, cancellation: cancellationToken);
        var tenant = tenants.FirstOrDefault();

        if (tenant is null)
        {
            return Result.Failure(TenantErrors.TenantDoesNotExist);
        }

        await repository.DeleteAsync(tenant, cancellation: cancellationToken);

        return Result.Success();
    }
}