using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using brewlogsMinimalApi.Data;
using brewlogsMinimalApi.Dtos;
using Dapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace brewlogsMinimalApi.Helpers;

public class AuthHelper
{
    private readonly IConfiguration _config;
    private readonly DbContext _dapper;

    public AuthHelper(IConfiguration config)
    {
        _dapper = new DbContext(config);
        _config = config;
    }

    public byte[] GetPasswordHash(string password, byte[] passwordSalt)
    {
        string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value +
                                        Convert.ToBase64String(passwordSalt);

        return KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 1000000,
            numBytesRequested: 256 / 8
        );
    }


    public string CreateToken(int userId)
    {
        Claim[] claims =
        {
            new("userId", userId.ToString())
        };

        string? tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;

        SymmetricSecurityKey tokenKey = new(
            Encoding.UTF8.GetBytes(
                tokenKeyString ?? ""
            )
        );

        SigningCredentials credentials = new(
            tokenKey,
            SecurityAlgorithms.HmacSha512Signature
        );

        SecurityTokenDescriptor descriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            Expires = DateTime.Now.AddDays(1)
        };

        JwtSecurityTokenHandler tokenHandler = new();

        SecurityToken token = tokenHandler.CreateToken(descriptor);

        return tokenHandler.WriteToken(token);
    }

    public bool SetPassword(UserForLoginDto userForSetPassword)
    {
        byte[] passwordSalt = new byte[128 / 8];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(passwordSalt);
        }

        byte[] passwordHash = GetPasswordHash(userForSetPassword.Password, passwordSalt);

        Console.WriteLine("0x" + BitConverter.ToString(passwordHash).Replace("-", ""));
        Console.WriteLine(Encoding.UTF8.GetString(passwordHash, 0, passwordHash.Length));
        Console.WriteLine(Encoding.UTF8.GetString(passwordHash));
        Console.WriteLine(Convert.ToBase64String(passwordHash));

        const string sqlAddAuth = """
                                  EXEC BrewData.spRegistration_Upsert
                                                  @Email = @EmailParam,
                                                  @PasswordHash = @PasswordHashParam,
                                                  @PasswordSalt = @PasswordSaltParam
                                  """;

        DynamicParameters sqlParameters = new();

        sqlParameters.Add("@EmailParam", userForSetPassword.Email, DbType.String);
        sqlParameters.Add("@PasswordHashParam", passwordHash, DbType.Binary);
        sqlParameters.Add("@PasswordSaltParam", passwordSalt, DbType.Binary);

        return _dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters);
    }
}