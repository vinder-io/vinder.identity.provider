namespace Vinder.IdentityProvider.Application.Validators.Tenant;

public sealed class TenantUpdateValidator : AbstractValidator<TenantForUpdate>
{
    public TenantUpdateValidator()
    {
        RuleFor(tenant => tenant.Name)
            .NotEmpty()
            .WithMessage("Tenant name must not be empty.")
            .MinimumLength(3)
            .WithMessage("Tenant name must be at least 3 characters long.")
            .MaximumLength(100)
            .WithMessage("Tenant name must be at most 100 characters long.");

        RuleFor(tenant => tenant.Description)
            .MaximumLength(500)
            .WithMessage("Tenant description must be at most 500 characters long.")
            .When(tenant => !string.IsNullOrEmpty(tenant.Description));
    }
}