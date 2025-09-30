namespace Vinder.IdentityProvider.Domain.Entities;

public sealed class Secret : Entity
{
    public string PrivateKey { get; set; } = default!;
    public string PublicKey { get; set; } = default!;
}