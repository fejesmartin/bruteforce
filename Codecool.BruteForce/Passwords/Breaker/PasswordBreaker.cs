using Codecool.BruteForce.Passwords.Model;

namespace Codecool.BruteForce.Passwords.Breaker;

public class PasswordBreaker : IPasswordBreaker
{
    private  readonly AsciiTableRange LowercaseChars = new(97, 122);
    private  readonly AsciiTableRange UppercaseChars = new(65, 90);
    private  readonly AsciiTableRange Numbers = new(48, 57);
  
    public IEnumerable<string> GetCombinations(int passwordLength)
    {

        List<List<string>> allChars = new();
         
        if (passwordLength <= 0)
        {
            throw new ArgumentException("Password length must be greater than 0.");
        }

        List<string> numberList = new();
        for (int i = Numbers.Start; i <= Numbers.End; i++)
        {
            numberList.Add(((char)i).ToString());
        }
        List<string> lowerCaseList = new();
        for (int i = LowercaseChars.Start; i <= LowercaseChars.End; i++)
        {
            lowerCaseList.Add(((char)i).ToString());
        }
        List<string> upperCaseList = new();
        for (int i = UppercaseChars.Start; i <= UppercaseChars.End; i++)
        {
            upperCaseList.Add(((char)i).ToString());
        }

        allChars.Add(numberList);
        allChars.Add(lowerCaseList);
        allChars.Add(upperCaseList);
       

        return GetAllPossibleCombos(allChars);

    }

    private static IEnumerable<string> GetAllPossibleCombos(IEnumerable<IEnumerable<string>> strings)
    {
        IEnumerable<string> combos = new[] { "" };

        combos = strings
            .Aggregate(combos, (current, inner) => 
                current.SelectMany(c => inner, (c, i) => c + i));

        return combos;
    }  
}
