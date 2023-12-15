using System.ComponentModel.DataAnnotations;

namespace brewlogsMinimalApi.Model;

public class Account
{
    [Key] public int UserId { get; init; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public bool Active { get; set; }
}