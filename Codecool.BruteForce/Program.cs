using System.Diagnostics;
using System.Text;
using Codecool.BruteForce.Authentication;
using Codecool.BruteForce.Passwords.Breaker;
using Codecool.BruteForce.Passwords.Generator;
using Codecool.BruteForce.Passwords.Model;
using Codecool.BruteForce.Users.Generator;
using Codecool.BruteForce.Users.Repository;

namespace Codecool.BruteForce;

internal static class Program
{
    private static readonly AsciiTableRange LowercaseChars = new(97, 122);
    private static readonly AsciiTableRange UppercaseChars = new(65, 90);
    private static readonly AsciiTableRange Numbers = new(48, 57);
    private static List<(string, string)> allUsers = new();

    public static void Main(string[] args)
    {
        string workDir = AppDomain.CurrentDomain.BaseDirectory;
        var dbFile = $"{workDir}\\Users.db";
       
        IUserRepository userRepository = new UserRepository(dbFile);
        userRepository.DeleteAll();

       
        
        var passwordGenerators = CreatePasswordGenerators();
        IUserGenerator userGenerator = new UserGenerator(passwordGenerators);
        int userCount = 10;
        int maxPwLength = 3;
        
        AddUsersToDb(userCount, maxPwLength, userGenerator, userRepository);
        foreach (var user in allUsers)
        {
            Console.WriteLine(user.Item1 + " " + user.Item2); 
        }
        Console.WriteLine($"Database initialized with {userCount} users; maximum password length: {maxPwLength}");

        IAuthenticationService authenticationService = new AuthenticationService(userRepository);
        BreakUsers(userCount, maxPwLength, authenticationService);

        Console.WriteLine($"Press any key to exit.");

        Console.ReadKey();
    }

    private static void AddUsersToDb(int count, int maxPwLength, IUserGenerator userGenerator,
        IUserRepository userRepository)
    {
        foreach (var user in userGenerator.Generate(count, maxPwLength))
        {
            userRepository.Add(user.userName,user.password);
            allUsers.Add((user.userName,user.password));
            
        }
        
    }
    

    private static IEnumerable<IPasswordGenerator> CreatePasswordGenerators()
    {
        var lowercasePwGen = new PasswordGenerator(LowercaseChars);
        var uppercasePwGen = new PasswordGenerator(LowercaseChars, UppercaseChars);
        IPasswordGenerator numbersPwGen = new PasswordGenerator(LowercaseChars, UppercaseChars, Numbers);

        return new List<IPasswordGenerator>
        {
            lowercasePwGen, uppercasePwGen, numbersPwGen
        };
    }

    private static void BreakUsers(int userCount, int maxPwLength, IAuthenticationService authenticationService)
    {
        var passwordBreaker = new PasswordBreaker();
        Console.WriteLine("Initiating password breaker...\n");

        for (int i = 1; i <= userCount; i++)
        {
            var user = $"user{i}";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine($"Trying to break {user} with all possible password combinations...\nThis might take a while..");
            

            bool userPasswordFound = false;
            
            for (int currentLength = 1; currentLength <= maxPwLength; currentLength++)
            {
                var pwCombinations = passwordBreaker.GetCombinations(currentLength);

                foreach (var pw in pwCombinations)
                {

                    if (authenticationService.Authenticate(allUsers[i - 1].Item1, pw))
                    {
                        Console.WriteLine($"Authentication successful for {user} with password: {pw}");

                        if (pw == allUsers[i - 1].Item2)
                        {
                            Console.WriteLine($"User: {user}, Password found: {pw}");
                            stopwatch.Stop();
                            TimeSpan elapsed = stopwatch.Elapsed;
                            Console.WriteLine($"Elapsed time: {elapsed}");
                            userPasswordFound = true;
                            break;
                        }
                    }
                }
                if (userPasswordFound)
                {
                    break;
                }
            }
        }
    }


}
