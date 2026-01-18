namespace Vinder.Federation.Domain.Errors;

public static class AuthenticationErrors
{
    public static readonly Error InvalidTokenFormat = new(
        Code: "#VINDER-IDP-ERR-AUT-410",
        Description: "The token format is invalid or the token is malformed. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error TokenExpired = new(
        Code: "#VINDER-IDP-ERR-AUT-411",
        Description: "The token has expired. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error InvalidSignature = new(
        Code: "#VINDER-IDP-ERR-AUT-412",
        Description: "The token signature is invalid. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error InvalidRefreshToken = new(
        Code: "#VINDER-IDP-ERR-AUT-405",
        Description: "The provided refresh token is invalid, expired, or has already been used. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error LogoutFailed = new(
        Code: "#VINDER-IDP-ERR-AUT-409",
        Description: "Logout failed: the refresh token is invalid, expired, or has already been used. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error InvalidCredentials = new(
        Code: "#VINDER-IDP-ERR-AUT-401",
        Description: "The provided credentials are invalid. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error ClientNotFound = new(
        Code: "#VINDER-IDP-ERR-AUT-408",
        Description: "The client was not found. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error InvalidClientCredentials = new(
        Code: "#VINDER-IDP-ERR-AUT-403",
        Description: "The provided client credentials are invalid. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error UserNotFound = new(
        Code: "#VINDER-IDP-ERR-AUT-404",
        Description: "The user was not found. See https://bit.ly/errors-reference for more details."
    );
}
