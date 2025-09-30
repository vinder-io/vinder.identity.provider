namespace Vinder.IdentityProvider.Application.Utilities;

public static class RsaHelper
{
    public static RSA FromPrivateKey(string base64PrivateKey)
    {
        var rsa = RSA.Create();

        rsa.ImportRSAPrivateKey(Convert.FromBase64String(base64PrivateKey), out _);

        return rsa;
    }

    public static RSA FromPublicKey(string base64PublicKey)
    {
        var rsa = RSA.Create();

        rsa.ImportRSAPublicKey(Convert.FromBase64String(base64PublicKey), out _);

        return rsa;
    }
}
