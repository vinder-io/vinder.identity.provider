namespace Vinder.Identity.Domain.Concepts;

public sealed record RedirectUri(string Address) :
    IValueObject<RedirectUri>
{
    public string Address { get; } = Address;
}