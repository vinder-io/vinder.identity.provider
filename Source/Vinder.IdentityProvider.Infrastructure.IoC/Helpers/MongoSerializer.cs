namespace Vinder.IdentityProvider.Infrastructure.IoC.Helpers;

[ExcludeFromCodeCoverage]
public static class MongoSerializer
{
    private static bool _registered;

    public static void Register()
    {
        if (_registered)
        {
            return;
        }

        _registered = true;

        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
    }
}
