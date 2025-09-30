namespace Vinder.IdentityProvider.Common.Constants;

public static class OpenIDEndpoints
{
    public const string Authorize = "api/v1/openid/authorize";
    public const string Token = "api/v1/openid/connect/token";
    public const string UserInfo = "api/v1/openid/user-info";
    public const string Jwks = ".well-known/jwks.json";
}