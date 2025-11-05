namespace App.Services;

public class UserService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUsersRepository _usersRepository;


    public UserService(IUsersRepository usersRepository, IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
        _passwordHasher = passwordHasher;
    }
    // регистрация аккаунта
    public async Task Register(string userName, string email, string password)
    {
        var hashedPassword = _passwordHasher.Generate(password);

        var user = User.Create(Guid.NewGuid(), userName, hashedPassword, email);

        await _userRepository.Add(user);
    }

    // вход в аккаунт(логин)
    public async Task<string> Login(string email, string password)
    {
        var token = await usersService.Login();
        

        return;
    }
}