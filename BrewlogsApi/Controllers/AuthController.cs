using System.Data;
using AutoMapper;
using BrewlogsApi.Data;
using BrewlogsApi.Dtos;
using BrewlogsApi.Helpers;
using BrewlogsApi.Model;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrewlogsApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly DbContext _dapper;
    private readonly SqlHelper _sqlHelper;
    private readonly AuthHelper _authHelper;
    private readonly IMapper _mapper;

    public AuthController(IConfiguration config)
    {
        _dapper = new DbContext(config);
        _sqlHelper = new SqlHelper(config);
        _authHelper = new AuthHelper(config);
        _mapper = new Mapper(new MapperConfiguration(cfg => { cfg.CreateMap<AccountForRegistrationDto, Account>(); }));
    }

    [AllowAnonymous]
    [HttpPost("/Register")]
    public IActionResult Register(AccountForRegistrationDto account)
    {
        if (account.Password != account.PasswordConfirm) return BadRequest("Passwords do not match!");

        string sqlCheckUserExists = "SELECT Email FROM BrewData.Auth WHERE Email = '" +
                                    account.Email + "'";

        IEnumerable<string> existingUsers = _dapper.LoadData<string>(sqlCheckUserExists);

        if (existingUsers.Any()) return Conflict("User with this email already exists!");

        UserForLoginDto userForSetPassword = new()
        {
            Email = account.Email,
            Password = account.Password
        };

        if (!_authHelper.SetPassword(userForSetPassword)) return BadRequest("Failed to register user.");

        Account userComplete = _mapper.Map<Account>(account);
        userComplete.Active = true;

        if (!_sqlHelper.UpsertUser(userComplete)) return StatusCode(500, "Failed to add user.");

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("/Login")]
    public IActionResult Login(UserForLoginDto loginUser)
    {
        const string sqlForHashAndSalt = """
                                         EXEC BrewData.spLoginConfirmation_Get
                                                         @Email = @EmailParam
                                         """;

        DynamicParameters sqlParameters = new();

        sqlParameters.Add("@EmailParam", loginUser.Email, DbType.String);

        AccountForConfirmationDto userForConfirmation = _dapper
            .LoadDataSingleWithParameters<AccountForConfirmationDto>(sqlForHashAndSalt, sqlParameters);

        byte[] passwordHash = _authHelper.GetPasswordHash(loginUser.Password, userForConfirmation.PasswordSalt);

        if (passwordHash.Where((t, index) => t != userForConfirmation.PasswordHash[index]).Any())
        {
            return StatusCode(401, "Incorrect password!");
        }

        string userIdSql = """
                           
                                           SELECT UserId FROM BrewData.Users WHERE Email = '
                           """ +
                           loginUser.Email + "'";

        int userId = _dapper.LoadDataSingle<int>(userIdSql);

        return Ok(new Dictionary<string, string>
        {
            { "token", _authHelper.CreateToken(userId) }
        });
    }

    [HttpPut("PasswordReset")]
    public IActionResult ResetPassword(UserForPasswordResetDto userForPasswordReset)
    {
        string? userId = User.FindFirst("userId")?.Value;
        
        if (userId != null && _authHelper.ResetPassword(userForPasswordReset, userId))
        {
            return Ok();
        }

        return BadRequest("Failed to reset password");
    }

    [HttpGet("RefreshToken")]
    public IActionResult RefreshToken()
    {
        string? userIdClaim = User.FindFirst("userId")?.Value;
        if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
        {
            return Unauthorized(new { error = "Authentication failed", message = "There was an error validating the user." });
        }
        
        string newToken = _authHelper.CreateToken(userId);
        return Ok(new { token = newToken });
    }
}