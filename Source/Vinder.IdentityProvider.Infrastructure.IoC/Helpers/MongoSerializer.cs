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

        if (!BsonClassMap.IsClassMapRegistered(typeof(Entity)))
        {
            BsonClassMap.RegisterClassMap<Entity>(mapper =>
            {
                var serializer = new GuidSerializer(GuidRepresentation.Standard);

                mapper.AutoMap();
                mapper.MapMember(entity => entity.Id)
                    .SetSerializer(serializer);
            });
        }
    }
}
