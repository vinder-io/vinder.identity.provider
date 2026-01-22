namespace Vinder.Federation.Domain.Errors;

public sealed class AuthorizationErrors
{
    public static readonly Error RedirectUriNotAllowed = new(
        Code: "#VINDER-IDP-ERR-AUT-421",
        Description: "The specified redirect URI is not registered or allowed for this tenant. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error InvalidAuthorizationCode = new(
        Code: "#VINDER-IDP-ERR-AUT-406",
        Description: "The provided authorization code is invalid, expired, or has already been used. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error AuthorizationCodeExpired = new(
        Code: "#VINDER-IDP-ERR-AUT-409",
        Description: "The authorization code has expired. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error InvalidCodeVerifier = new(
        Code: "#VINDER-IDP-ERR-AUT-407",
        Description: "The provided code verifier does not match the code challenge. See https://bit.ly/errors-reference for more details."
    );
}
