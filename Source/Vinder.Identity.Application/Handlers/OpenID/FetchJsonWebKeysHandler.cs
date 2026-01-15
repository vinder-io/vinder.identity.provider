namespace Vinder.Identity.Application.Handlers.OpenID;

public sealed class FetchJsonWebKeysHandler(ISecretCollection collection) :
    IMessageHandler<FetchJsonWebKeysParameters, Result<JsonWebKeySetScheme>>
{
    public async Task<Result<JsonWebKeySetScheme>> HandleAsync(
        FetchJsonWebKeysParameters parameters, CancellationToken cancellation)
    {
        var secret = await collection.GetSecretAsync(cancellation: cancellation);
        var jwks = JsonWebKeysMapper.AsJsonWebKeySetScheme(secret);

        return Result<JsonWebKeySetScheme>.Success(jwks);
    }
}