using System.Collections.Generic;

namespace SharpSixteen.Core.Database;

/// <summary>Enemy definition for battle (id, name, sprite, stats).</summary>
public class EnemyRecord
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string SpriteId { get; set; } = "";
    public int Hp { get; set; }
    public int Attack { get; set; }
    public int Experience { get; set; }
}

/// <summary>Enemy database. Load from database/enemies.json or use built-in list.</summary>
public static class Enemies
{
    private static readonly List<EnemyRecord> BuiltIn = new()
    {
        new EnemyRecord { Id = "Slime", Name = "Slime", SpriteId = "Slime", Hp = 15, Attack = 3, Experience = 5 },
        new EnemyRecord { Id = "Bat", Name = "Bat", SpriteId = "Bat", Hp = 10, Attack = 4, Experience = 6 },
        new EnemyRecord { Id = "Skeleton", Name = "Skeleton", SpriteId = "Skeleton", Hp = 22, Attack = 5, Experience = 10 },
        new EnemyRecord { Id = "Goblin", Name = "Goblin", SpriteId = "Goblin", Hp = 18, Attack = 4, Experience = 8 },
        new EnemyRecord { Id = "Orc", Name = "Orc", SpriteId = "Orc", Hp = 28, Attack = 6, Experience = 14 },
        new EnemyRecord { Id = "Ghost", Name = "Ghost", SpriteId = "Ghost", Hp = 12, Attack = 5, Experience = 12 },
    };

    private static List<EnemyRecord> _all = new(BuiltIn);
    private static Dictionary<string, EnemyRecord>? _byId;

    public static IReadOnlyList<EnemyRecord> All => _all;

    public static void AddRangeFromJson(IEnumerable<EnemyRecord> records)
    {
        if (records == null) return;
        foreach (var e in records) _all.Add(e);
        _byId = null;
    }

    public static EnemyRecord? Get(string id)
    {
        _byId ??= new Dictionary<string, EnemyRecord>();
        if (_byId.Count == 0)
            foreach (var e in _all) _byId[e.Id] = e;
        return _byId.TryGetValue(id ?? "Slime", out var r) ? r : _all.Count > 0 ? _all[0] : null;
    }
}
