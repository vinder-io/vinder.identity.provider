namespace Vinder.IdentityProvider.Application.Payloads.User;

public sealed record InspectPrincipalParameters :
    IRequest<Result<PrincipalDetailsScheme>>;