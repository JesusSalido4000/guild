namespace Guild.Api.Models;

public enum AdventurerStatus
{
    Alive = 1,
    Dead = 2
}

public sealed class Adventurer
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name { get; set; } = "";
    public string Class { get; set; } = "";
    public int Level { get; set; } = 1;

    public int Age { get; set; }
    public AdventurerStatus Status { get; set; } = AdventurerStatus.Alive;

    // Graveyard fields
    public DateTimeOffset? DiedAt { get; set; }
    public string? DiedOnMission { get; set; }
    public string? Epitaph { get; set; }
}