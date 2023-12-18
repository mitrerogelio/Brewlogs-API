using AutoMapper;
using brewlogsMinimalApi.Data;
using brewlogsMinimalApi.Dtos;
using brewlogsMinimalApi.Helpers;
using brewlogsMinimalApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace brewlogsMinimalApi.Controllers;

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
    [HttpPost("/register")]
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
}