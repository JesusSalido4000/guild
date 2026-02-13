using Guild.Api.Data;
using Guild.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Guild.Api.Controllers;

[ApiController]
[Route("graveyard")]
public class GraveyardController : ControllerBase
{
    [HttpGet]
    public IActionResult GetDead()
    {
        var dead = GuildStore.Adventurers
            .Where(a => a.Status == AdventurerStatus.Dead)
            .OrderByDescending(a => a.Death!.DiedAt)
            .ToList();

        return Ok(dead);
    }
}