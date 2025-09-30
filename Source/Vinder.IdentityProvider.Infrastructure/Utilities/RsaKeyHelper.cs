namespace Vinder.IdentityProvider.Infrastructure.Utilities;

public static class RsaKeyHelper
{
    public static RsaSecurityKey FromPrivateKey(string base64PrivateKey)
    {
        var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(base64PrivateKey), out _);
        return new RsaSecurityKey(rsa);
    }

    public static RsaSecurityKey FromPublicKey(string base64PublicKey)
    {
        var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromBase64String(base64PublicKey), out _);
        return new RsaSecurityKey(rsa);
    }
}