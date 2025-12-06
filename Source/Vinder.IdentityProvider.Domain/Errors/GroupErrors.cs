namespace Vinder.IdentityProvider.Domain.Errors;

public static class GroupErrors
{
    public static readonly Error GroupAlreadyExists = new(
        Code: "#VINDER-IDP-ERR-GRP-409",
        Description: "The group with the specified name already exists. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error GroupAlreadyHasPermission = new(
        Code: "#VINDER-IDP-ERR-GRP-415",
        Description: "The group already has the specified permission assigned. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error GroupDoesNotExist = new(
        Code: "#VINDER-IDP-ERR-GRP-404",
        Description: "The group with the specified ID does not exist. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error PermissionNotAssigned = new(
        Code: "#VINDER-IDP-ERR-GRP-416",
        Description: "The group does not have the specified permission assigned. See https://bit.ly/errors-reference for more details."
    );
}