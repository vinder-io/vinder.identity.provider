namespace Vinder.Identity.Application.Payloads.User;

public sealed record InspectPrincipalParameters :
    IMessage<Result<PrincipalDetailsScheme>>;