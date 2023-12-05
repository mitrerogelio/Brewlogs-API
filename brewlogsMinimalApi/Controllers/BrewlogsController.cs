using AutoMapper;
using brewlogsMinimalApi.Data;
using brewlogsMinimalApi.Dtos;
using brewlogsMinimalApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace brewlogsMinimalApi.Controllers;

[ApiController]
[Route("api/brewlogs")]
public class BrewlogsController : ControllerBase
{
    private readonly IBrewlogRepository _repository;
    private readonly IMapper _mapper;

    public BrewlogsController(IBrewlogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetBrewlogs()
    {
        List<Brewlog> brewlogs = await _repository.GetBrewlogs();
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

    [HttpPost]
    public async Task<IActionResult> CreateBrewlog(BrewlogDto brewlogDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Brewlog brewlogEntity = _mapper.Map<Brewlog>(brewlogDto);
        _repository.AddEntity(brewlogEntity);

        try
        {
            await _repository.SaveChangesAsync();
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
}