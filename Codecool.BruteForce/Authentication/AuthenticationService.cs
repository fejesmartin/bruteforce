using Codecool.BruteForce.Users.Repository;

namespace Codecool.BruteForce.Authentication;

public class AuthenticationService: IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    
    public AuthenticationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public bool Authenticate(string userName, string password)
    {
        int userId;
        if (int.TryParse(userName[^1].ToString(), out userId))
        {
            var user = _userRepository.Get(userId);
            if (user != null && user.UserName == userName && user.Password == password)
            {
                return true;
            }
        }
        return false;
    }
}