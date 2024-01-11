using System.Data;
using BrewlogsApi.Data;
using BrewlogsApi.Dtos;
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
        int authorId = int.Parse(this.User.FindFirst("userId")?.Value ?? "");

        string sql = """
                     EXEC BrewData.spBrewlogs_Upsert

                     """;

        DynamicParameters sqlParameters = new();
        if (brewlogDto.Id.HasValue)
        {
            sqlParameters.Add("@Id", brewlogDto.Id.Value, DbType.Int32);
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
               @BrewerUsed=@BrewerUsedParameter
               """;
        sqlParameters.Add("@AuthorParameter", authorId, DbType.Int32);
        sqlParameters.Add("@CoffeeNameParameter", brewlogDto.CoffeeName, DbType.String);
        sqlParameters.Add("@DoseParameter", brewlogDto.Dose, DbType.Int32);
        sqlParameters.Add("@GrindParameter", brewlogDto.Grind, DbType.String);
        sqlParameters.Add("@BrewRatioParameter", brewlogDto.BrewRatio, DbType.Int32);
        sqlParameters.Add("@RoastParameter", brewlogDto.Roast, DbType.String);
        sqlParameters.Add("@BrewerUsedParameter", brewlogDto.BrewerUsed, DbType.String);

        bool result = _dapper.ExecuteSqlWithParameters(sql, sqlParameters);

        return result ? Ok() : StatusCode(500);
    }

    [HttpGet("{id:int}")]
    public Task<IActionResult> GetBrewlog(int id)
    {
        int authorId = int.Parse(this.User.FindFirst("userId")?.Value ?? "");
        const string sql = "EXEC BrewData.spBrewlogs_Get" +
                           "@Id=@IdParameter" +
                           "@Author=@AuthorParameter";
        DynamicParameters sqlParameters = new();
        sqlParameters.Add("@IdParameter", id, DbType.Int32);
        sqlParameters.Add("@AuthorParameter", authorId, DbType.Int32);

        bool result = _dapper.ExecuteSqlWithParameters(sql, sqlParameters);

        return Task.FromResult<IActionResult>(result ? Ok() : StatusCode(500));
    }

    /*
    [HttpGet]
    public async Task<IActionResult> GetBrewlogs()
    {
        List<Brewlog>? brewlogs = await _repository.GetBrewlogs();
        if (brewlogs == null)
        {
            return NotFound();
        }
        return Ok(brewlogs);
    }

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