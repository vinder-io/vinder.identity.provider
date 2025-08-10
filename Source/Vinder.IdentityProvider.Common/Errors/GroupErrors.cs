using Vinder.IdentityProvider.Common.Results;

namespace Vinder.IdentityProvider.Common.Errors;

public static class GroupErrors
{
    public static readonly Error GroupAlreadyExists = new(
        Code: "#VINDER-IDP-ERR-GRP-409",
        Description: "The group with the specified name already exists."
    );
}