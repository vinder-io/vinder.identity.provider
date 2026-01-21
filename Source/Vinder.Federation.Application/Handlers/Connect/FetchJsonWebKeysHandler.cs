namespace Vinder.Federation.Application.Handlers.Connect;

public sealed class FetchJsonWebKeysHandler(ISecretCollection collection) :
    IMessageHandler<FetchJsonWebKeysParameters, Result<JsonWebKeySetScheme>>
{
    public async Task<Result<JsonWebKeySetScheme>> HandleAsync(
        FetchJsonWebKeysParameters parameters, CancellationToken cancellation = default)
    {
        var secret = await collection.GetSecretAsync(cancellation: cancellation);
        var jwks = JsonWebKeysMapper.AsJsonWebKeySetScheme(secret);

        return Result<JsonWebKeySetScheme>.Success(jwks);
    }
}
