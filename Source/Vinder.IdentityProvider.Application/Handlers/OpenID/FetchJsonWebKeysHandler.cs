namespace Vinder.IdentityProvider.Application.Handlers.OpenID;

public sealed class FetchJsonWebKeysHandler(ISecretRepository repository) :
    IRequestHandler<FetchJsonWebKeysRequest, Result<JsonWebKeySet>>
{
    public async Task<Result<JsonWebKeySet>> Handle(
        FetchJsonWebKeysRequest request, CancellationToken cancellationToken)
    {
        var secret = await repository.GetSecretAsync(cancellation: cancellationToken);
        var jwks = JsonWebKeysMapper.AsJsonWebKeySet(secret);

        return Result<JsonWebKeySet>.Success(jwks);
    }
}