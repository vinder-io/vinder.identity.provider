namespace Vinder.IdentityProvider.Application.Handlers.Tenant;

public sealed class TenantCreationHandler(
    ITenantRepository repository,
    IClientCredentialsGenerator credentialsGenerator
) : IRequestHandler<TenantForCreation, Result<TenantDetails>>
{
    public async Task<Result<TenantDetails>> Handle(TenantForCreation request, CancellationToken cancellationToken)
    {
        var (clientId, clientSecret) = await credentialsGenerator.GenerateAsync(request.Name);

        var tenant = TenantMapper.AsTenant(
            tenant: request,
            clientId: clientId,
            secretHash: clientSecret
        );

        await repository.InsertAsync(tenant, cancellationToken);

        return Result<TenantDetails>.Success(TenantMapper.AsResponse(tenant));
    }
}