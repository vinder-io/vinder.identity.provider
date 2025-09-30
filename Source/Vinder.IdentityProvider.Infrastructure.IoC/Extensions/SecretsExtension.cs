namespace Vinder.IdentityProvider.Infrastructure.IoC.Extensions;

public static class SecretsExtension
{
    public static void AddInitialSecrets(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var secretRepository = serviceProvider.GetRequiredService<ISecretRepository>();

        var secret = secretRepository.GetSecretAsync().GetAwaiter().GetResult();
        if (secret is null)
        {
            using var rsa = RSA.Create(2048);
            secret = new Secret
            {
                PrivateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey()),
                PublicKey  = Convert.ToBase64String(rsa.ExportRSAPublicKey())
            };

            secretRepository.InsertAsync(secret).GetAwaiter().GetResult();
        }
    }
}
