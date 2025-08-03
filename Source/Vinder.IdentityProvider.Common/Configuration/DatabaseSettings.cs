namespace Vinder.IdentityProvider.Common.Configuration;

public sealed record DatabaseSettings
{
    public string ConnectionString { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;
}