using System.Collections.Generic;

namespace SharpSixteen.Core.Database;

/// <summary>Item definition (consumable, key, etc.).</summary>
public class ItemRecord
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Kind { get; set; } = "Consumable"; // Consumable, Key, Equipment
    public string Effect { get; set; } = ""; // HealHp, HealMp, etc.
    public int Value { get; set; }
}

/// <summary>Items database.</summary>
public static class Items
{
    private static readonly List<ItemRecord> BuiltIn = new()
    {
        new ItemRecord { Id = "Potion", Name = "Potion", Kind = "Consumable", Effect = "HealHp", Value = 20 },
        new ItemRecord { Id = "Ether", Name = "Ether", Kind = "Consumable", Effect = "HealMp", Value = 15 },
        new ItemRecord { Id = "Key", Name = "Dungeon Key", Kind = "Key", Effect = "OpenDoor", Value = 0 },
        new ItemRecord { Id = "Antidote", Name = "Antidote", Kind = "Consumable", Effect = "CurePoison", Value = 10 },
        new ItemRecord { Id = "PhoenixDown", Name = "Phoenix Down", Kind = "Consumable", Effect = "Revive", Value = 50 },
    };

    private static List<ItemRecord> _all = new(BuiltIn);
    private static Dictionary<string, ItemRecord>? _byId;

    public static IReadOnlyList<ItemRecord> All => _all;

    public static void AddRangeFromJson(IEnumerable<ItemRecord> records)
    {
        if (records == null) return;
        foreach (var r in records) _all.Add(r);
        _byId = null;
    }

    public static ItemRecord? Get(string id)
    {
        _byId ??= new Dictionary<string, ItemRecord>();
        if (_byId.Count == 0)
            foreach (var i in _all) _byId[i.Id] = i;
        return _byId.TryGetValue(id ?? "", out var r) ? r : null;
    }
}
