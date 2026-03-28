namespace Auth.API.Repositories;

public interface ICognitoRepository<T>
{
    // Me de um exemplo COM GENERICS
    // Task<T> LoginAsync(string email, string password);

    Task<T> SignIn(string email, string password);
    Task ForgotPassword(string email);
    Task ConfirmForgotPassword(string email, string code, string password);
    Task SignUp(string email, string password);
    
}