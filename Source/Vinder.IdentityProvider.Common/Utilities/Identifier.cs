namespace Vinder.IdentityProvider.Common.Utilities;

public static class Identifier
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private static readonly RandomNumberGenerator _randomNumberGenerator = RandomNumberGenerator.Create();

    public static string Generate<TResource>(int prefixLetters = 3, int randomPartLength = 22)
    {
        var prefix = new string([.. typeof(TResource).Name
            .Take(prefixLetters)
            .Select(char.ToLowerInvariant)
        ]);

        return $"{prefix}_{RandomString(randomPartLength)}";
    }

    public static string Generate(string prefix, int randomPartLength = 22)
        => $"{prefix.ToLowerInvariant()}_{RandomString(randomPartLength)}";

    private static string RandomString(int length)
    {
        Span<byte> bytes = stackalloc byte[length];
        Span<char> chars = stackalloc char[length];

        _randomNumberGenerator.GetBytes(bytes);

        for (int index = 0; index < length; index++)
        {
            chars[index] = Alphabet[bytes[index] % Alphabet.Length];
        }

        return new string(chars);
    }
}
