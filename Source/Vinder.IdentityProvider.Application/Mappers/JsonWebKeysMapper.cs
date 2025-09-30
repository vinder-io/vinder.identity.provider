namespace Vinder.IdentityProvider.Application.Mappers;

public static class JsonWebKeysMapper
{
    public static JsonWebKeyDetails AsJsonWebKeys(Secret secret)
    {
        var publicKey = RsaHelper.FromPublicKey(secret.PublicKey);
        var parameters = publicKey.ExportParameters(false);

        return new JsonWebKeyDetails
        {
            Identifier = secret.Id,
            Exponent = Base64UrlEncoder.Encode(parameters.Exponent!),
            Modulus = Base64UrlEncoder.Encode(parameters.Modulus!)
        };
    }

    public static JsonWebKeySet AsJsonWebKeySet(Secret secret)
    {
        return new JsonWebKeySet
        {
            Keys = [JsonWebKeysMapper.AsJsonWebKeys(secret)]
        };
    }
}