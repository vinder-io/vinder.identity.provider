using Vinder.IdentityProvider.Common.Results;

namespace Vinder.IdentityProvider.Common.Errors;

public static class UserErrors
{
    public static readonly Error UserDoesNotExist = new(
        Code: "#VINDER-IDP-ERR-USR-404",
        Description: "The specified user does not exist. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error UserAlreadyInGroup = new(
        Code: "#VINDER-IDP-ERR-USR-409",
        Description: "The user is already a member of the specified group. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error UserAlreadyHasPermission = new(
        Code: "#VINDER-IDP-ERR-USR-410",
        Description: "The user already has the specified permission assigned. See https://bit.ly/errors-reference for more details."
    );

}
