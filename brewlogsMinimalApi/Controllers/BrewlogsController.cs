using brewlogsMinimalApi.Data;
using brewlogsMinimalApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace brewlogsMinimalApi.Controllers;

[ApiController]
[Route("api/brewlogs")]
public class BrewlogsController : ControllerBase
{
    private readonly BrewlogsDbContext _dbContext;

    public BrewlogsController(BrewlogsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetBrewlogs()
    {
        var brewlogs = await _dbContext.Brewlogs.ToListAsync();
        return Ok(brewlogs);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBrewlog(Guid id)
    {
        Brewlog? brewlog = await _dbContext.Brewlogs.FindAsync(id);
        if (brewlog == null)
        {
            return NotFound();
        }

        return Ok(brewlog);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBrewlog(Brewlog brewlog)
    {
        _dbContext.Brewlogs.Add(brewlog);
        await _dbContext.SaveChangesAsync();
        return Created($"/api/brewlogs/{brewlog.Id}", brewlog);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBrewlog(Guid id, Brewlog updatedBrewlog)
    {
        Brewlog? existingBrewlog = await _dbContext.Brewlogs.FindAsync(id);
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

        await _dbContext.SaveChangesAsync();
        return Ok(existingBrewlog);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBrewlog(Guid id)
    {
        Brewlog? brewlog = await _dbContext.Brewlogs.FindAsync(id);
        if (brewlog == null)
        {
            return NotFound();
        }

        _dbContext.Brewlogs.Remove(brewlog);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}