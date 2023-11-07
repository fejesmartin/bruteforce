using Codecool.BruteForce.Passwords.Model;

namespace Codecool.BruteForce.Passwords.Generator;

public class PasswordGenerator : IPasswordGenerator
{
    private static readonly Random Random = new();
    private readonly AsciiTableRange[] _characterSets;

    public PasswordGenerator(params AsciiTableRange[] characterSets)
    {
        _characterSets = characterSets;
    }

    public string Generate(int length)
    {
        if (length <= 0 || _characterSets.Length == 0)
        {
            return string.Empty;
        }

        string password = string.Empty;
        
        for (int i = 0; i < length; i++)
        {
            var randomCharSet = GetRandomCharacterSet();
            var randomChar = GetRandomCharacter(randomCharSet);
            password += randomChar;
        }

        return password;
    }

    private AsciiTableRange GetRandomCharacterSet()
    {
        int randomIndex = Random.Next(_characterSets.Length);
        return _characterSets[randomIndex];
    }

    private static char GetRandomCharacter(AsciiTableRange characterSet)
    {
        int randomValue = Random.Next(characterSet.Start, characterSet.End + 1);
        return (char)randomValue;
    }
}
