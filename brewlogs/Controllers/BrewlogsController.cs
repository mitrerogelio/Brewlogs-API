using brewlogs.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace brewlogs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrewlogsController : ControllerBase {

    private readonly BrewlogsContext _context;

    public BrewlogsController(BrewlogsContext context) {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetLogs() {
        if (_context.Brewlogs == null) return Empty;
        var recentLogs = await _context.Brewlogs
            .OrderByDescending(log => log.CreatedAt)
            .Take(10)
            .ToListAsync();
        return Ok(recentLogs);

    }
}