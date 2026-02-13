namespace Guild.Api.Models;

public enum AdventurerStatus
{
    Alive = 1,
    Dead = 2
}

public enum AdventurerRank
{
    E = 1,
    D = 2,
    C = 3,
    B = 4,
    A = 5,
    S = 6
}

public enum AdventurerClass
{
    Knight = 1,
    Mage = 2,
    Ranger = 3,
    Cleric = 4,
    Rogue = 5,
    Monk = 6,
    Necromancer = 7,
    Engineer = 8
}

public sealed class DeathInfo
{
    public DateTimeOffset DiedAt { get; set; } = DateTimeOffset.UtcNow;
    public string MissionName { get; set; } = "";
    public string? Note { get; set; }
    public string? Epitaph { get; set; }
}

public sealed class Adventurer
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    public string Name { get; set; } = "";
    public int Age { get; set; }

    // Profesión
    public AdventurerClass Class { get; set; }

    // Poder técnico (XP -> Level)
    public int Level { get; set; } = 1;
    public int Xp { get; set; } = 0;

    // Renombre (Renown -> Rank)
    public AdventurerRank Rank { get; set; } = AdventurerRank.E;
    public int Renown { get; set; } = 0;

    public AdventurerStatus Status { get; set; } = AdventurerStatus.Alive;

    public List<MissionLogEntry> History { get; set; } = new();
    public DeathInfo? Death { get; set; }
}
