using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BrewlogsApi.Data;

public class DbContext
{
    private readonly IConfiguration _config;

    public DbContext(IConfiguration config)
    {
        _config = config;
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

    public int ExecuteScalarSqlWithParameters(string sql, DynamicParameters parameters)
    {
        using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.ExecuteScalar<int>(sql, parameters);
    }

    public IEnumerable<T> LoadDataWithParameters<T>(string sql, DynamicParameters parameters)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Query<T>(sql, parameters);
    }

    public T LoadDataSingleWithParameters<T>(string sql, DynamicParameters parameters)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.QuerySingle<T>(sql, parameters);
    }
}