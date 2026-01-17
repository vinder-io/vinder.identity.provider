namespace Vinder.Identity.Application.Validators.Identity;

public sealed class ClientAuthenticationCredentialsValidator : AbstractValidator<ClientAuthenticationCredentials>
{
    public ClientAuthenticationCredentialsValidator()
    {
        RuleFor(credential => credential.GrantType)
            .NotEmpty()
            .WithMessage("grant type must not be empty.")
            .Must(grant => grant == "client_credentials" || grant == "authorization_code")
            .WithMessage("grant type must be either 'client_credentials' or 'authorization_code'.");

        When(credential => credential.GrantType == "client_credentials", () =>
        {
            RuleFor(credential => credential.ClientId)
            .NotEmpty()
            .WithMessage("client identifier must not be empty.")
            .MaximumLength(200)
            .WithMessage("client identifier must be at most 200 characters long.");

            RuleFor(credential => credential.ClientSecret)
            .NotEmpty()
            .WithMessage("client secret must not be empty.")
            .MaximumLength(500)
            .WithMessage("client secret must be at most 500 characters long.");
        });

        When(credential => credential.GrantType == "authorization_code", () =>
        {
            RuleFor(credential => credential.Code)
                .NotEmpty()
                .WithMessage("code must not be empty.");

            RuleFor(credential => credential.CodeVerifier)
                .NotEmpty()
                .WithMessage("code verifier must not be empty.")
                .MinimumLength(43)
                .WithMessage("code verifier must be at least 43 characters.")
                .MaximumLength(128)
                .WithMessage("code verifier must be at most 128 characters.")
                .Matches("^[a-zA-Z0-9_-]+$")
                .WithMessage("code verifier must be base64url encoded.");
        });
    }
}