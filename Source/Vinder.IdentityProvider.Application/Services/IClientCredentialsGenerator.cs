namespace Vinder.IdentityProvider.Application.Services;

public interface IClientCredentialsGenerator
{
    Task<(string clientId, string clientSecret)> GenerateAsync(string tenantName);
}
