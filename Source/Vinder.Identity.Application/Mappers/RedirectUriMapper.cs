namespace Vinder.Identity.Application.Mappers;

public static class RedirectUriMapper
{
    public static RedirectUri AsRedirectUri(this string uri) =>
        new(uri);
}