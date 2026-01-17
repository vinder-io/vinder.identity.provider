namespace Vinder.Identity.Domain.Aggregates;

public enum TokenType
{
    Refresh,
    EmailVerification,
    AuthorizationCode,
    PasswordReset
}