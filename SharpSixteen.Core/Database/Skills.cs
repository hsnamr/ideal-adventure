using System.Collections.Generic;

namespace SharpSixteen.Core.Database;

/// <summary>Skill/spell for battle (magic menu).</summary>
public class SkillRecord
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public int MpCost { get; set; }
    public int Power { get; set; }
    public string Target { get; set; } = "Enemy"; // Enemy, Self, AllEnemies
}

/// <summary>Skills database. Used by battle Magic menu.</summary>
public static class Skills
{
    private static readonly List<SkillRecord> BuiltIn = new()
    {
        new SkillRecord { Id = "Fire", Name = "Fire", MpCost = 4, Power = 10, Target = "Enemy" },
        new SkillRecord { Id = "Ice", Name = "Ice", MpCost = 4, Power = 10, Target = "Enemy" },
        new SkillRecord { Id = "Heal", Name = "Heal", MpCost = 6, Power = 15, Target = "Self" },
        new SkillRecord { Id = "Thunder", Name = "Thunder", MpCost = 8, Power = 18, Target = "Enemy" },
        new SkillRecord { Id = "Cure", Name = "Cure", MpCost = 5, Power = 20, Target = "Self" },
        new SkillRecord { Id = "Blizzard", Name = "Blizzard", MpCost = 10, Power = 22, Target = "Enemy" },
    };

    private static List<SkillRecord> _all = new(BuiltIn);
    private static Dictionary<string, SkillRecord>? _byId;

    public static IReadOnlyList<SkillRecord> All => _all;

    public static void AddRangeFromJson(IEnumerable<SkillRecord> records)
    {
        if (records == null) return;
        foreach (var s in records) _all.Add(s);
        _byId = null;
    }

    public static SkillRecord? Get(string id)
    {
        _byId ??= new Dictionary<string, SkillRecord>();
        if (_byId.Count == 0)
            foreach (var s in _all) _byId[s.Id] = s;
        return _byId.TryGetValue(id ?? "", out var r) ? r : null;
    }
}
