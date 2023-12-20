namespace BrewlogsApi.Dtos;

public class AccountForConfirmationDto
{
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
}