namespace Vinder.IdentityProvider.Application.Services;

public interface IClientCredentialsGenerator
{
    public Task<ClientCredentials> GenerateAsync(string tenantName, CancellationToken cancellation = default);
}
