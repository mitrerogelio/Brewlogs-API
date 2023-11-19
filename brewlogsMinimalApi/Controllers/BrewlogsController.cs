using System.Security.Claims;
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBrewlog(int id)
    {
        var brewlog = await _dbContext.Brewlogs.FindAsync(id);
        if (brewlog == null)
        {
            return NotFound();
        }

        return Ok(brewlog);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBrewlog(Brewlog brewlog)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("Unable to determine User ID. Something may be wrong with the request.");
        }
        
        brewlog.Author = userId;
        _dbContext.Brewlogs.Add(brewlog);
        await _dbContext.SaveChangesAsync();
        return Created($"/api/brewlogs/{brewlog.Id}", brewlog);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBrewlog(int id, Brewlog updatedBrewlog)
    {
        var existingBrewlog = await _dbContext.Brewlogs.FindAsync(id);
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBrewlog(int id)
    {
        var brewlog = await _dbContext.Brewlogs.FindAsync(id);
        if (brewlog == null)
        {
            return NotFound();
        }

        _dbContext.Brewlogs.Remove(brewlog);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}