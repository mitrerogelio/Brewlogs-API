using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace brewlogsMinimalApi.Data;

public class DbContext 
{
    private readonly IConfiguration _config;

    public DbContext(IConfiguration config)
    {
        _config = config;
    }
    
    public IEnumerable<T> LoadData<T>(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Query<T>(sql);
    }

    public T LoadDataSingle<T>(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.QuerySingle<T>(sql);
    }

    public bool ExecuteSql(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Execute(sql) > 0;
    }

    public bool ExecuteSqlWithParameters(string sql, DynamicParameters parameters)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Execute(sql, parameters) > 0;
    }
}