namespace Vinder.Identity.WebApi.Pages;

public sealed class AuthorizePage(
    IDispatcher dispatcher,
    IUserCollection userCollection,
    ITenantCollection tenantCollection,
    ITokenCollection tokenCollection,
    ITenantProvider tenantProvider
) : PageModel
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

        var tenant = tenantProvider.GetCurrentTenant();
        var filters = new UserFiltersBuilder()
            .WithUsername(Credentials.Username)
            .WithTenantId(tenant.Id)
            .Build();

        var users = await userCollection.GetUsersAsync(filters);
        var user = users.FirstOrDefault();

        if (user is null)
        {
            ModelState.AddModelError(AuthenticationErrors.UserNotFound.Code, AuthenticationErrors.UserNotFound.Description);
            return Page();
        }

        var code = Guid.NewGuid().ToString("N").ToUpperInvariant();
        var token = new Domain.Aggregates.SecurityToken
        {
            UserId = user.Id,
            TenantId = tenant.Id,
            Type = TokenType.AuthorizationCode,
            Value = code,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
            Metadata = new Dictionary<string, string>
            {
                { "code.challenge", Parameters.CodeChallenge ?? string.Empty },
                { "code.challenge.method", Parameters.CodeChallengeMethod ?? string.Empty }
            }
        };

        await tokenCollection.InsertAsync(token);

        return Redirect($"{Parameters.RedirectUri}?code={code}&state={Parameters.State}");
    }
}
