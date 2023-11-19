namespace brewlogsMinimalApi.Helpers;

public class EmailRegistrationParams
{
    public EmailRegistrationParams(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; }
    public string Password { get; }
}