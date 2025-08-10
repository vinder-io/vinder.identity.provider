namespace Vinder.IdentityProvider.Common.Configuration;

public sealed class Administration
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Tenant { get; set; } = default!;
}