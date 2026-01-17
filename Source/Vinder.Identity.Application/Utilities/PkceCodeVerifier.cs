namespace Vinder.Identity.Application.Utilities;

public static class PkceCodeVerifier
{
    public static bool Validate(string codeVerifier, string codeChallenge, string method)
    {
        if (string.IsNullOrWhiteSpace(codeVerifier) || string.IsNullOrWhiteSpace(codeChallenge))
            return false;

        return method switch
        {
            "S256" => ValidateS256(codeVerifier, codeChallenge),
            "plain" => codeVerifier == codeChallenge,

            _ => false
        };
    }

    private static bool ValidateS256(string codeVerifier, string codeChallenge)
    {
        var bytes = SHA256.HashData(System.Text.Encoding.ASCII.GetBytes(codeVerifier));
        var hashed = Base64UrlEncoder.Encode(bytes);

        return hashed == codeChallenge;
    }
}