namespace Vinder.Identity.Domain.Errors;

public static class PermissionErrors
{
    public static readonly Error PermissionAlreadyExists = new(
        Code: "#VINDER-IDP-ERR-PRM-409",
        Description: "The permission with the specified name already exists. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error PermissionDoesNotExist = new(
        Code: "#VINDER-IDP-ERR-PRM-404",
        Description: "The permission with the specified ID does not exist. See https://bit.ly/errors-reference for more details."
    );
}
