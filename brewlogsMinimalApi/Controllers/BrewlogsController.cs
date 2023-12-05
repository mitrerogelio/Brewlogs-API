using AutoMapper;
using brewlogsMinimalApi.Data;
using brewlogsMinimalApi.Dtos;
using brewlogsMinimalApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace brewlogsMinimalApi.Controllers;

[ApiController]
[Route("api/brewlogs")]
public class BrewlogsController : ControllerBase
{
    private readonly BrewlogsDbContext _dbContext;
    private readonly IMapper _mapper;

    public BrewlogsController(BrewlogsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
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
    public async Task<IActionResult> CreateBrewlog(BrewlogDto brewlogDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Brewlog brewlogEntity = _mapper.Map<Brewlog>(brewlogDto);
        _dbContext.Brewlogs.Add(brewlogEntity);

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error: " + ex.Message);
        }

        return CreatedAtAction("GetBrewlog", new { id = brewlogEntity.Id }, brewlogEntity);
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