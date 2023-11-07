using Codecool.BruteForce.Passwords.Generator;

namespace Codecool.BruteForce.Users.Generator;

public class UserGenerator : IUserGenerator
{
    private readonly List<IPasswordGenerator> _passwordGenerators;
    private static Random random = new Random();

    private int _userCount;

    public UserGenerator(IEnumerable<IPasswordGenerator> passwordGenerators)
    {
        _passwordGenerators = passwordGenerators.ToList();
        _userCount = 0;
    }

    public IEnumerable<(string userName, string password)> Generate(int count, int maxPasswordLength)
    {
        var userProperties = new List<(string userName, string password)>();
        
        for (int i = 0; i < count; i++)
        {
            _userCount++;
            var userName = $"user{_userCount}";
            var passwordGenerator = GetRandomPasswordGenerator();
            var passwordLength = GetRandomPasswordLength(maxPasswordLength);
            var password = passwordGenerator.Generate(passwordLength);
            
            userProperties.Add((userName,password));
        }

        return userProperties;
    }

    private IPasswordGenerator GetRandomPasswordGenerator()
    {
        return _passwordGenerators[random.Next(_passwordGenerators.Count)];
    }

    private static int GetRandomPasswordLength(int maxPasswordLength)
    {
        return random.Next(1,maxPasswordLength + 1);
    }
}
