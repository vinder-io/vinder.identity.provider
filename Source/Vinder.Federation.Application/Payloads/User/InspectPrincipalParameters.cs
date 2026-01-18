namespace Vinder.Federation.Application.Payloads.User;

public sealed record InspectPrincipalParameters :
    IMessage<Result<PrincipalDetailsScheme>>;