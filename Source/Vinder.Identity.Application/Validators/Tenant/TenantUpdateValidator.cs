namespace Vinder.Identity.Application.Validators.Tenant;

public sealed class TenantUpdateValidator : AbstractValidator<TenantUpdateScheme>
{
    public TenantUpdateValidator()
    {
        RuleFor(tenant => tenant.Name)
            .NotEmpty()
            .WithMessage("tenant name must not be empty.")
            .MinimumLength(3)
            .WithMessage("tenant name must be at least 3 characters long.")
            .MaximumLength(100)
            .WithMessage("tenant name must be at most 100 characters long.");

        RuleFor(tenant => tenant.Description)
            .MaximumLength(250)
            .WithMessage("tenant description must be at most 250 characters long.")
            .When(tenant => !string.IsNullOrEmpty(tenant.Description));

        RuleFor(tenant => tenant.AllowedRedirectUris)
            .Must(uris => uris.All(uri => !string.IsNullOrWhiteSpace(uri) && Uri.IsWellFormedUriString(uri, UriKind.Absolute)))
            .WithMessage("all redirect URIs must be valid absolute URIs.")
            .When(tenant => tenant.AllowedRedirectUris is not null && tenant.AllowedRedirectUris.Count > 0);

        RuleFor(tenant => tenant.AllowedRedirectUris)
            .Must(uris => uris.Distinct().Count() == uris.Count)
            .WithMessage("redirect URIs must be unique.")
            .When(tenant => tenant.AllowedRedirectUris is not null && tenant.AllowedRedirectUris.Count > 0);
    }
}