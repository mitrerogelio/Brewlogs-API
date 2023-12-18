using System.Data;
using brewlogsMinimalApi.Data;
using brewlogsMinimalApi.Dtos;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace brewlogsMinimalApi.Controllers;

[ApiController]
[Route("api/brewlogs")]
public class BrewlogsController : ControllerBase
{
    private readonly DbContext _dapper;

    public BrewlogsController(IConfiguration config)
    {
        _dapper = new DbContext(config);
    }

    [HttpGet("test")]
    public DateTime Test()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }


    [HttpPost]
    public IActionResult UpsertBrewlog(BrewlogDto brewlogDto)
    {
        int authorId = int.Parse(this.User.FindFirst("userId")?.Value ?? "");

        const string sql = """
                           EXEC BrewData.spBrewlogs_Upsert
                                               @Id=@IdParameter,
                                               @Author=@AuthorParameter,
                                               @CoffeeName=@CoffeeNameParameter,
                                               @Dose=@DoseParameter,
                                               @Grind=@GrindParameter,
                                               @BrewRatio=@BrewRatioParameter,
                                               @Roast=@RoastParameter,
                                               @BrewerUsed=@BrewerUsedParameter
                           """;

        DynamicParameters sqlParameters = new DynamicParameters();
        sqlParameters.Add("@IdParameter", brewlogDto.Id, DbType.Int32);
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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBrewlog(Guid id)
    {
        Brewlog? brewlog = await _repository.GetBrewlog(id);

        if (brewlog == null)
        {
            return NotFound();
        }

        return Ok(brewlog);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBrewlog(Guid id, Brewlog updatedBrewlog)
    {
        Brewlog? existingBrewlog = await _repository.GetBrewlog(id);
        if (existingBrewlog == null)
        {
            return NotFound();
        }

        existingBrewlog.CoffeeName = updatedBrewlog.CoffeeName;
        existingBrewlog.Dose = updatedBrewlog.Dose;
        existingBrewlog.Grind = updatedBrewlog.Grind;
        existingBrewlog.BrewRatio = updatedBrewlog.BrewRatio;
        existingBrewlog.Roast = updatedBrewlog.Roast;
        existingBrewlog.BrewerUsed = updatedBrewlog.BrewerUsed;

        await _repository.SaveChangesAsync();
        return Ok(existingBrewlog);
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