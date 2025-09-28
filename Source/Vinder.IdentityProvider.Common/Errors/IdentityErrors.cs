namespace Vinder.IdentityProvider.Common.Errors;

public static class IdentityErrors
{
    public static readonly Error UserAlreadyExists = new(
        Code: "#VINDER-IDP-ERR-IDN-409",
        Description: "The user with the specified username already exists. See https://bit.ly/errors-reference for more details."
    );
}
