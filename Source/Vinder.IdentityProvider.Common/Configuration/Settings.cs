namespace Vinder.IdentityProvider.Common.Configuration;

public sealed class Settings : ISettings
{
    public Administration Administration { get; set; } = default!;
    public DatabaseSettings Database { get; set; } = default!;
    public SecuritySettings Security { get; set; } = default!;
}