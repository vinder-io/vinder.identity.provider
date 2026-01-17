namespace Vinder.Identity.Application.Validators.Authorization;

public sealed class AuthorizationParametersValidator : AbstractValidator<AuthorizationParameters>
{
    public AuthorizationParametersValidator()
    {
        RuleFor(parameters => parameters.ResponseType)
            .NotNull()
            .NotEmpty()
            .Equal("code")
            .WithMessage("response type must be 'code'.");

        RuleFor(parameters => parameters.ClientId)
            .NotNull()
            .NotEmpty();

        RuleFor(parameters => parameters.RedirectUri)
            .NotNull()
            .NotEmpty()
            .Must(uri => uri is not null && Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("redirect uri must be a valid absolute uri.");

        RuleFor(parameters => parameters.CodeChallenge)
            .NotNull()
            .NotEmpty();

        RuleFor(parameters => parameters.CodeChallengeMethod)
            .NotNull()
            .NotEmpty()
            .Must(method => method is not null && method == "S256")
            .WithMessage("code challenge method must be 'S256'.");
    }
}
