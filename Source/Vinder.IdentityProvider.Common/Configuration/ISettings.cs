namespace Vinder.IdentityProvider.Common.Configuration;

public interface ISettings
{
    public DatabaseSettings Database { get; }
    public SecuritySettings Security { get; }
}