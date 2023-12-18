using System.Data;
using brewlogsMinimalApi.Data;
using brewlogsMinimalApi.Model;
using Dapper;

namespace brewlogsMinimalApi.Helpers;

public class SqlHelper
{
    private readonly DbContext _dapper;

    public SqlHelper(IConfiguration config)
    {
        _dapper = new DbContext(config);
    }

    public bool UpsertUser(Account user)
    {
        string sql = @"EXEC BrewData.spUser_Upsert
                @UserId = @UserIdParameter,
                @FirstName = @FirstNameParameter, 
                @LastName = @LastNameParameter, 
                @Email = @EmailParameter, 
                @Active = @ActiveParameter";

        DynamicParameters sqlParameters = new();

        sqlParameters.Add("@FirstNameParameter", user.FirstName, DbType.String);
        sqlParameters.Add("@LastNameParameter", user.LastName, DbType.String);
        sqlParameters.Add("@EmailParameter", user.Email, DbType.String);
        sqlParameters.Add("@ActiveParameter", user.Active, DbType.Boolean);
        sqlParameters.Add("@UserIdParameter", user.UserId, DbType.Int32);

        return _dapper.ExecuteSqlWithParameters(sql, sqlParameters);
    }
}