namespace Vinder.Identity.Application.Handlers.Tenant;

public sealed class TenantDeletionHandler(ITenantRepository repository) : IRequestHandler<TenantDeletionScheme, Result>
{
    public async Task<Result> Handle(TenantDeletionScheme request, CancellationToken cancellationToken)
    {
        var filters = new TenantFiltersBuilder()
            .WithIdentifier(request.TenantId)
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