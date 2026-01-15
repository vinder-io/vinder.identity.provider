namespace Vinder.Identity.Application.Payloads.User;

public sealed record InspectPrincipalParameters :
    IRequest<Result<PrincipalDetailsScheme>>;