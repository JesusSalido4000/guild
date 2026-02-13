using Guild.Api.Models;

namespace Guild.Api.Domain;

public static class Progression
{
    // XP por nivel: simple, predecible (ajustable luego)
    public static int XpRequiredForNextLevel(int level)
        => 100 + (level * 25);

    public static void ApplyXp(Adventurer a, int xpGained)
    {
        if (xpGained <= 0) return;

        a.Xp += xpGained;

        while (a.Xp >= XpRequiredForNextLevel(a.Level))
        {
            a.Xp -= XpRequiredForNextLevel(a.Level);
            a.Level++;
        }
    }

    public static AdventurerRank RankFromRenown(int renown)
    {
        if (renown >= 5000) return AdventurerRank.S;
        if (renown >= 3000) return AdventurerRank.A;
        if (renown >= 1500) return AdventurerRank.B;
        if (renown >= 600)  return AdventurerRank.C;
        if (renown >= 200)  return AdventurerRank.D;
        return AdventurerRank.E;
    }

    public static void ApplyRenown(Adventurer a, int renownGained)
    {
        if (renownGained <= 0) return;

        a.Renown += renownGained;
        a.Rank = RankFromRenown(a.Renown);
    }
}