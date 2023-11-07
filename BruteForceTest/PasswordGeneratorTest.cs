using Codecool.BruteForce.Passwords.Generator;
using Codecool.BruteForce.Passwords.Model;

namespace BruteForceTest;

public class BruteForceTests
{
    private static readonly AsciiTableRange LowercaseChars = new(97, 122);
    private static readonly AsciiTableRange UppercaseChars = new(65, 90);
    private static readonly AsciiTableRange Numbers = new(48, 57);
    private PasswordGenerator _passwordGenerator;
    [SetUp]
    public void Setup()
    { 
        _passwordGenerator = new PasswordGenerator(LowercaseChars,UppercaseChars,Numbers);
    }

    [Test]
    public void PasswordGeneratorCreatesValidPassword()
    {
        string password = _passwordGenerator.Generate(4);
        
        Assert.That(password.Length, Is.EqualTo(4));
    }
    [Test]
    public void PasswordGeneratorReturnEmptyStringWhenLengthIsZero()
    {
        string password = _passwordGenerator.Generate(0);
        
        Assert.That(password,Is.Empty);
    }
    
}