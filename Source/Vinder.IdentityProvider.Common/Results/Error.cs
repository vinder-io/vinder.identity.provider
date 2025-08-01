namespace Vinder.IdentityProvider.Common.Results;

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error Unknown = new("Unknown", "An unknown error has occurred.");
}
