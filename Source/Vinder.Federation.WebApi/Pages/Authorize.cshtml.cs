namespace Vinder.Federation.WebApi.Pages;

public sealed class AuthorizePage : PageModel
{
    private readonly IDispatcher _dispatcher;
    private readonly IUserCollection _userCollection;

    private readonly ITokenCollection _tokenCollection;
    private readonly ITenantCollection _tenantCollection;
    private readonly ITenantProvider _tenantProvider;

    #region constructors
    public AuthorizePage(
        IDispatcher dispatcher,
        IUserCollection userCollection,
        ITenantProvider tenantProvider,
        ITenantCollection tenantCollection,
        ITokenCollection tokenCollection)
    {
        _dispatcher = dispatcher;
        _userCollection = userCollection;
        _tenantCollection = tenantCollection;
        _tenantProvider = tenantProvider;
        _tokenCollection = tokenCollection;
    }
    #endregion

    [property: BindProperty(SupportsGet = true)]
    public AuthorizationParameters Parameters { get; set; } = new();

    [property: BindProperty]
    public AuthenticationCredentials Credentials { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var filters = TenantFilters.WithSpecifications()
            .WithClientId(Parameters.ClientId)
            .Build();

        var tenants = await _tenantCollection.GetTenantsAsync(filters);
        var tenant = tenants.FirstOrDefault();

        if (tenant is null)
        {
            ModelState.AddModelError(
                key: TenantErrors.TenantDoesNotExist.Code,
                errorMessage: TenantErrors.TenantDoesNotExist.Description
            );

            return Page();
        }

        var result = await _dispatcher.DispatchAsync(Credentials);
        if (result.IsFailure)
        {
            ModelState.AddModelError(
                key: result.Error.Code,
                errorMessage: result.Error.Description
            );

            return Page();
        }

        _tenantProvider.SetTenant(tenant);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var result = await _dispatcher.DispatchAsync(Credentials);
        if (result.IsFailure)
        {
            ModelState.AddModelError(result.Error.Code, result.Error.Description);
            return Page();
        }

        var tenant = _tenantProvider.GetCurrentTenant();
        var filters = UserFilters.WithSpecifications()
            .WithUsername(Credentials.Username)
            .WithTenantId(tenant.Id)
            .Build();

        var users = await _userCollection.GetUsersAsync(filters);
        var user = users.FirstOrDefault();

        if (user is null)
        {
            ModelState.AddModelError(AuthenticationErrors.UserNotFound.Code, AuthenticationErrors.UserNotFound.Description);
            return Page();
        }

        var code = Guid.NewGuid().ToString("N").ToUpperInvariant();
        var metadata = new Dictionary<string, string>
        {
            { "code.challenge", Parameters.CodeChallenge ?? string.Empty },
            { "code.challenge.method", Parameters.CodeChallengeMethod ?? string.Empty }
        };

        var token = new Domain.Aggregates.SecurityToken
        {
            UserId = user.Id,
            TenantId = tenant.Id,
            Metadata = metadata,
            Value = code,
            Type = TokenType.AuthorizationCode,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
        };

        await _tokenCollection.InsertAsync(token);

        return Redirect($"{Parameters.RedirectUri}?code={code}&state={Parameters.State}");
    }
}
