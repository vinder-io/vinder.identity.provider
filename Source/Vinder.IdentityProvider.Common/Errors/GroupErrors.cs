using Vinder.IdentityProvider.Common.Results;

namespace Vinder.IdentityProvider.Common.Errors;

public static class GroupErrors
{
    public static readonly Error GroupAlreadyExists = new(
        Code: "#VINDER-IDP-ERR-GRP-409",
        Description: "The group with the specified name already exists."
    );

    public static readonly Error GroupAlreadyHasPermission = new(
        Code: "#VINDER-IDP-ERR-GRP-415",
        Description: "The group already has the specified permission assigned."
    );

    public static readonly Error GroupDoesNotExist = new(
        Code: "#VINDER-IDP-ERR-GRP-404",
        Description: "The group with the specified ID does not exist."
    );
}