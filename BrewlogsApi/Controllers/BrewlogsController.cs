using System.Data;
using BrewlogsApi.Data;
using BrewlogsApi.Dtos;
using BrewlogsApi.Model;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace BrewlogsApi.Controllers;

[ApiController]
[Route("api/brewlogs")]
public class BrewlogsController : ControllerBase
{
    private readonly DbContext _dapper;

    public BrewlogsController(IConfiguration config)
    {
        _dapper = new DbContext(config);
    }

    [HttpPost]
    public IActionResult UpsertBrewlog(BrewlogDto brewlogDto)
    {
        int authorId = int.Parse(User.FindFirst("userId")!.Value);

        string sql = """
                     EXEC BrewData.spBrewlogs_Upsert

                     """;

        DynamicParameters sqlParameters = new();
        if (brewlogDto.Id.HasValue)
        {
            sql += "@Id=@IdParameter,\n";
            sqlParameters.Add("@IdParameter", brewlogDto.Id.Value, DbType.Int32);
        }
        else
        {
            sqlParameters.Add("@Id", DBNull.Value, DbType.Int32);
        }

        sql += """
               @Author=@AuthorParameter,
               @CoffeeName=@CoffeeNameParameter,
               @Dose=@DoseParameter,
               @Grind=@GrindParameter,
               @BrewRatio=@BrewRatioParameter,
               @Roast=@RoastParameter,
               @BrewerUsed=@BrewerUsedParameter,
               @Rating=@RatingParameter
               """;
        sqlParameters.Add("@AuthorParameter", authorId, DbType.Int32);
        sqlParameters.Add("@CoffeeNameParameter", brewlogDto.CoffeeName, DbType.String);
        sqlParameters.Add("@DoseParameter", brewlogDto.Dose, DbType.Int32);
        sqlParameters.Add("@GrindParameter", brewlogDto.Grind, DbType.String);
        sqlParameters.Add("@BrewRatioParameter", brewlogDto.BrewRatio, DbType.Int32);
        sqlParameters.Add("@RoastParameter", brewlogDto.Roast, DbType.String);
        sqlParameters.Add("@BrewerUsedParameter", brewlogDto.BrewerUsed, DbType.String);
        sqlParameters.Add("@RatingParameter", brewlogDto.Rating, DbType.Int16);

        bool result = _dapper.ExecuteSqlWithParameters(sql, sqlParameters);

        return result ? Ok() : StatusCode(500);
    }

    [HttpGet("{logId:int}")]
    public ObjectResult GetLog(int logId)
    {
        try
        {
            int authorId = int.Parse(User.FindFirst("userId")?.Value ?? "");
            string sql = "EXEC BrewData.spBrewlogs_Get \n";
            const string stringParameters = """
                                            @BrewlogId=@BrewlogIdParameter,
                                            @Author=@AuthorParameter
                                            """;

            DynamicParameters sqlParameters = new();
            sqlParameters.Add("@BrewlogIdParameter", logId, DbType.Int32);
            sqlParameters.Add("@AuthorParameter", authorId, DbType.Int32);
            sql += stringParameters;

            IEnumerable<Brewlog> record = _dapper.LoadDataWithParameters<Brewlog>(sql, sqlParameters);

            if (!record.Any())
            {
                return NotFound("Brewlog not found.");
            }

            return Ok(record);
        }
        catch
        {
            return StatusCode(500, "Failed to get brewlog");
        }
    }

    [HttpGet]
    public ObjectResult QueryLogs(string? search = null)
    {
        try
        {
            int authorId = int.Parse(User.FindFirst("userId")?.Value ?? "");
            string sql = "EXEC BrewData.spBrewlogs_Get \n";
            const string stringParameters = """
                                            @Author=@AuthorParameter,
                                            @SearchValue=@SearchValueParameter
                                            """;

            DynamicParameters sqlParameters = new();
            sqlParameters.Add("@AuthorParameter", authorId, DbType.Int32);
            sqlParameters.Add("@SearchValueParameter", search, DbType.String);
            sql += stringParameters;

            IEnumerable<Brewlog> record = _dapper.LoadDataWithParameters<Brewlog>(sql, sqlParameters);

            if (!record.Any())
            {
                return NotFound("Brewlog not found.");
            }

            return Ok(record);
        }
        catch
        {
            return StatusCode(500, "Failed to get brewlog");
        }
    }

    /*
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBrewlog(Guid id)
    {
        Brewlog? brewlog = await _repository.GetBrewlog(id);
        if (brewlog == null)
        {
            return NotFound();
        }

        _repository.RemoveEntity(brewlog);
        await _repository.SaveChangesAsync();
        return NoContent();
    }
*/
}