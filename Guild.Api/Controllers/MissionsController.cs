using Guild.Api.Data;
using Guild.Api.Domain;
using Guild.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Guild.Api.Controllers;

public sealed record CompleteMissionRequest(
    Guid AdventurerId,
    string MissionName,
    MissionOutcome Outcome,
    int XpGained,
    int RenownGained,
    string? Note
);

[ApiController]
[Route("missions")]
public class MissionsController : ControllerBase
{
    [HttpPost("complete")]
    public IActionResult Complete([FromBody] CompleteMissionRequest req)
    {
        if (req.AdventurerId == Guid.Empty) return BadRequest("AdventurerId is required.");
        if (string.IsNullOrWhiteSpace(req.MissionName)) return BadRequest("MissionName is required.");
        if (req.XpGained < 0) return BadRequest("XpGained cannot be negative.");
        if (req.RenownGained < 0) return BadRequest("RenownGained cannot be negative.");

        var a = GuildStore.Get(req.AdventurerId);
        if (a is null) return NotFound("Adventurer not found.");
        if (a.Status != AdventurerStatus.Alive) return Conflict("Only alive adventurers can complete missions.");

        var log = new MissionLogEntry
        {
            MissionName = req.MissionName.Trim(),
            Outcome = req.Outcome,
            XpGained = req.XpGained,
            RenownGained = req.RenownGained,
            CompletedAt = DateTimeOffset.UtcNow,
            Note = string.IsNullOrWhiteSpace(req.Note) ? null : req.Note.Trim()
        };

        a.History.Add(log);

        Progression.ApplyXp(a, req.XpGained);
        Progression.ApplyRenown(a, req.RenownGained);

        return Ok(a);
    }
}