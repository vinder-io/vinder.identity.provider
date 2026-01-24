namespace Vinder.Federation.Domain.Aggregates;

public enum TokenType
{
    Refresh,
    EmailVerification,
    AuthorizationCode,
    PasswordReset
}
