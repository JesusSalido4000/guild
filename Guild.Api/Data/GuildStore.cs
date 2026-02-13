using Guild.Api.Models;

namespace Guild.Api.Data;

public static class GuildStore
{
    private static readonly List<Adventurer> _adventurers = new();

    public static IReadOnlyList<Adventurer> Adventurers => _adventurers;

    public static Adventurer Add(Adventurer a)
    {
        _adventurers.Add(a);
        return a;
    }

    public static Adventurer? Get(Guid id) => _adventurers.FirstOrDefault(x => x.Id == id);
}