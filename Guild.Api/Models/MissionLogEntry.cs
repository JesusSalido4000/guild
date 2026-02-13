namespace Guild.Api.Models;

public enum MissionOutcome
{
    Success = 1,
    Failure = 2,
    Partial = 3
}

public sealed class MissionLogEntry
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string MissionName { get; set; } = "";
    public MissionOutcome Outcome { get; set; } = MissionOutcome.Success;

    public int XpGained { get; set; }
    public int RenownGained { get; set; }

    public DateTimeOffset CompletedAt { get; set; } = DateTimeOffset.UtcNow;
    public string? Note { get; set; }
}