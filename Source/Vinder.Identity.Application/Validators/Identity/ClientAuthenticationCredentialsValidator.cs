namespace Vinder.Identity.Application.Validators.Identity;

public sealed class ClientAuthenticationCredentialsValidator : AbstractValidator<ClientAuthenticationCredentials>
{
    public ClientAuthenticationCredentialsValidator()
    {
        RuleFor(credential => credential.GrantType)
            .NotEmpty()
            .WithMessage("Grant type must not be empty.")
            .Equal("client_credentials")
            .WithMessage("Grant type must be 'client_credentials'.");

        RuleFor(credential => credential.ClientId)
            .NotEmpty()
            .WithMessage("Client ID must not be empty.")
            .MaximumLength(200)
            .WithMessage("Client ID must be at most 200 characters long.");

        RuleFor(credential => credential.ClientSecret)
            .NotEmpty()
            .WithMessage("Client secret must not be empty.")
            .MaximumLength(500)
            .WithMessage("Client secret must be at most 500 characters long.");
    }
}