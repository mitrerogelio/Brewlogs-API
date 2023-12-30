namespace BrewlogsApi.Dtos;

public class UserForPasswordResetDto
{
    public required string Email { get; set; }
    public required string NewPassword {get; set;}
}