using Codecool.BruteForce.Passwords.Model;

namespace Codecool.BruteForce.Passwords.Breaker;

public class PasswordBreaker : IPasswordBreaker
{
    private  readonly AsciiTableRange LowercaseChars = new(97, 122);
    private  readonly AsciiTableRange UppercaseChars = new(65, 90);
    private  readonly AsciiTableRange Numbers = new(48, 57);
  
    private static IEnumerable<string> GetAllPossibleCombos(IEnumerable<IEnumerable<string>> strings)
    {
        IEnumerable<string> combos = new[] { "" };

        combos = strings
            .Aggregate(combos, (current, inner) => current.SelectMany(c => inner, (c, i) => c + i));

        return combos;
    }

    public IEnumerable<string> GetCombinations(int passwordLength)
    {
        if (passwordLength <= 0)
        {
            throw new ArgumentException("Password length must be greater than 0.");
        }

        Console.WriteLine($"Generating combinations for password length: {passwordLength}");

        List<string> allChars = new List<string>();

        for (int i = Numbers.Start; i <= Numbers.End; i++)
        {
            allChars.Add(((char)i).ToString());
        }

        for (int i = LowercaseChars.Start; i <= LowercaseChars.End; i++)
        {
            allChars.Add(((char)i).ToString());
        }

        for (int i = UppercaseChars.Start; i <= UppercaseChars.End; i++)
        {
            allChars.Add(((char)i).ToString());
        }

        Console.WriteLine($"Total characters: {allChars.Count}");

        IEnumerable<string> combos = new[] { "" };

        for (int i = 0; i < passwordLength; i++)
        {
            combos = combos.SelectMany(c => allChars, (c, j) => c + j);
        }

        Console.WriteLine($"Total combinations: {combos.Count()}");

        return combos;
    }


}
