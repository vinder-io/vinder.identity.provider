namespace Vinder.IdentityProvider.WebApi.Extensions;

[ExcludeFromCodeCoverage]
public static class AuthenticationExtension
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var secretRepository = serviceProvider.GetRequiredService<ISecretRepository>();

        var secret = secretRepository.GetSecretAsync().GetAwaiter().GetResult();
        var publicKey = RsaKeyHelper.FromPublicKey(secret.PublicKey);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = publicKey,
            ClockSkew = TimeSpan.Zero
        };

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = validationParameters;
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
        });

        return services;
    }
}
