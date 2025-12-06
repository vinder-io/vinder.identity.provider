namespace Vinder.IdentityProvider.Application.Providers;

public interface IPrincipalProvider
{
    public User? Principal { get; }

    public void SetPrincipal(User user);
    public User GetCurrentPrincipal();
}
