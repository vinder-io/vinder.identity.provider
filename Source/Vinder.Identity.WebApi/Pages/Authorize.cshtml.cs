namespace Vinder.Identity.WebApi.Pages;

public sealed class AuthorizePage(IDispatcher dispatcher, ITenantCollection tenantCollection, ITenantProvider tenantProvider) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public AuthorizationParameters Parameters { get; set; } = new();

    [BindProperty]
    public AuthenticationCredentials Credentials { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var filters = new TenantFiltersBuilder()
            .WithClientId(Parameters.ClientId)
            .Build();

        var tenants = await tenantCollection.GetTenantsAsync(filters);
        var tenant = tenants.FirstOrDefault();

        if (tenant is null)
        {
            ModelState.AddModelError(TenantErrors.TenantDoesNotExist.Code, TenantErrors.TenantDoesNotExist.Description);
            return Page();
        }

        tenantProvider.SetTenant(tenant);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var result = await dispatcher.DispatchAsync(Credentials);
        if (result.IsFailure)
        {
            ModelState.AddModelError(AuthenticationErrors.InvalidCredentials.Code, result.Error.Description);
            return Page();
        }

        return Redirect($"{Parameters.RedirectUri}?code=CODE&state={Parameters.State}");
    }
}
