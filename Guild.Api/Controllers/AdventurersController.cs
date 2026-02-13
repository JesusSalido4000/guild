using Guild.Api.Data;
using Guild.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Guild.Api.Controllers;

public sealed record CreateAdventurerRequest(string Name, int Age, AdventurerClass Class);

public sealed record KillAdventurerRequest(string Mission, string? Note);

[ApiController]
[Route("adventurers")]
public class AdventurersController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAlive()
    {
        var alive = GuildStore.Adventurers.Where(a => a.Status == AdventurerStatus.Alive).ToList();
        return Ok(alive);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateAdventurerRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest("Name is required.");
        if (req.Age < 10 || req.Age > 120) return BadRequest("Age must be between 10 and 120.");

        var a = new Adventurer
        {
            Name = req.Name.Trim(),
            Age = req.Age,
            Class = req.Class,
            Status = AdventurerStatus.Alive,
            Level = 1,
            Xp = 0,
            Renown = 0,
            Rank = AdventurerRank.E
        };

        GuildStore.Add(a);
        return Created($"/adventurers/{a.Id}", a);
    }

    [HttpPost("{id:guid}/kill")]
    public IActionResult Kill(Guid id, [FromBody] KillAdventurerRequest req)
    {
        var a = GuildStore.Get(id);
        if (a is null) return NotFound();

        if (a.Status == AdventurerStatus.Dead)
            return Conflict("Adventurer is already dead.");

        if (string.IsNullOrWhiteSpace(req.Mission))
            return BadRequest("Mission is required.");

        a.Status = AdventurerStatus.Dead;
        a.Death = new DeathInfo
        {
            DiedAt = DateTimeOffset.UtcNow,
            MissionName = req.Mission.Trim(),
            Note = string.IsNullOrWhiteSpace(req.Note) ? null : req.Note.Trim(),
            Epitaph = $"{a.Name} cayó cumpliendo su deber."
        };

        return Ok(a);
    }
}