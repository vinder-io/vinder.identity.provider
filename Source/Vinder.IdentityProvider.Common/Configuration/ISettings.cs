namespace Vinder.IdentityProvider.Common.Configuration;

public interface ISettings
{
    public Administration Administration { get; }
    public DatabaseSettings Database { get; }
}