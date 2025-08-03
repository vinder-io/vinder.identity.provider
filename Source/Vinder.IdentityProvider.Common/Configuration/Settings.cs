namespace Vinder.IdentityProvider.Common.Configuration;

public sealed class Settings
{
    public DatabaseSettings Database { get; set; } = default!;
    public SecuritySettings Security { get; set; } = default!;
}