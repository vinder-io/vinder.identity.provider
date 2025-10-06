namespace Vinder.IdentityProvider.Application.Handlers.OpenID;

public sealed class FetchJsonWebKeysHandler(ISecretRepository repository) :
    IRequestHandler<FetchJsonWebKeysParameters, Result<JsonWebKeySetScheme>>
{
    public async Task<Result<JsonWebKeySetScheme>> Handle(
        FetchJsonWebKeysParameters request, CancellationToken cancellationToken)
    {
        var secret = await repository.GetSecretAsync(cancellation: cancellationToken);
        var jwks = JsonWebKeysMapper.AsJsonWebKeySetScheme(secret);

        return Result<JsonWebKeySetScheme>.Success(jwks);
    }
}