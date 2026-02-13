using Guild.Api.Data;
using Guild.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Guild.Api.Controllers;

public sealed record CreateAdventurerRequest(string Name, string Class, int Level, int Age);
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
        if (string.IsNullOrWhiteSpace(req.Class)) return BadRequest("Class is required.");
        if (req.Level < 1 || req.Level > 100) return BadRequest("Level must be between 1 and 100.");
        if (req.Age < 10 || req.Age > 120) return BadRequest("Age must be between 10 and 120.");

        var a = new Adventurer
        {
            Name = req.Name.Trim(),
            Class = req.Class.Trim(),
            Level = req.Level,
            Age = req.Age,
            Status = AdventurerStatus.Alive
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
        a.DiedAt = DateTimeOffset.UtcNow;
        a.DiedOnMission = req.Mission.Trim();

        // Comentario simple (luego lo hacemos más “lore”)
        a.Epitaph = string.IsNullOrWhiteSpace(req.Note)
            ? $"{a.Name} cayó cumpliendo su deber."
            : req.Note.Trim();

        return Ok(a);
    }
}

