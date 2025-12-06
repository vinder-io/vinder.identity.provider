namespace Vinder.IdentityProvider.Domain.Errors;

public static class ScopeErrors
{
    public static readonly Error ScopeAlreadyExists = new(
        Code: "#VINDER-IDP-ERROR-8D128",
        Description: "The scope with the specified name already exists. See https://bit.ly/errors-reference for more details."
    );

    public static readonly Error ScopeDoesNotExists = new(
        Code: "#VINDER-IDP-ERROR-903F9",
        Description: "The scope with the specified name does not exist. See https://bit.ly/errors-reference for more details."
    );
}