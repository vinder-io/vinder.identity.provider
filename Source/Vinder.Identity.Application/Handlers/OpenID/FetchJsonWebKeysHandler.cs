namespace Vinder.Identity.Application.Handlers.OpenID;

public sealed class FetchJsonWebKeysHandler(ISecretCollection collection) :
    IRequestHandler<FetchJsonWebKeysParameters, Result<JsonWebKeySetScheme>>
{
    public async Task<Result<JsonWebKeySetScheme>> Handle(
        FetchJsonWebKeysParameters request, CancellationToken cancellationToken)
    {
        var secret = await collection.GetSecretAsync(cancellation: cancellationToken);
        var jwks = JsonWebKeysMapper.AsJsonWebKeySetScheme(secret);

        return Result<JsonWebKeySetScheme>.Success(jwks);
    }
}