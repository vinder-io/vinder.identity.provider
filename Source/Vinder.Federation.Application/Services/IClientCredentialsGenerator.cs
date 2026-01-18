namespace Vinder.Federation.Application.Services;

public interface IClientCredentialsGenerator
{
    public Task<ClientCredentials> GenerateAsync(string tenantName, CancellationToken cancellation = default);
}
