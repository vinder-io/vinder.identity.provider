using Vinder.IdentityProvider.Common.Results;

namespace Vinder.IdentityProvider.Common.Errors;

public static class TenantErrors
{
    public static readonly Error HttpContextUnavailable = new(
        Code: "#VINDER-IDP-ERR-TNT-500",
        Description: "No HTTP context available to retrieve tenant information."
    );

    public static readonly Error TenantDoesNotExist = new(
        Code: "#VINDER-IDP-ERR-TNT-404",
        Description: "The specified tenant does not exist."
    );

    public static readonly Error TenantHeaderMissing = new(
        Code: "#VINDER-IDP-ERR-TNT-400",
        Description: "Tenant header is missing from the HTTP request."
    );
}
