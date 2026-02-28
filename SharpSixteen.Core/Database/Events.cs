using System.Collections.Generic;

namespace SharpSixteen.Core.Database;

/// <summary>Event definition (dialogue, trigger, script ref).</summary>
public class EventRecord
{
    public string Id { get; set; } = "";
    public string Trigger { get; set; } = ""; // MapId.Tile, Talk, StepOn
    public string Text { get; set; } = "";
    public string PortraitId { get; set; } = "";
    public string NextEventId { get; set; } = "";
}

/// <summary>Events database (dialogue, story).</summary>
public static class Events
{
    private static readonly List<EventRecord> BuiltIn = new()
    {
        new EventRecord { Id = "town_greeting", Trigger = "Talk", Text = "Welcome to town!", PortraitId = "" },
        new EventRecord { Id = "cave_warning", Trigger = "StepOn", Text = "The cave looks dark...", PortraitId = "" },
        new EventRecord { Id = "field_welcome", Trigger = "StepOn", Text = "Open fields. Watch out for monsters!", PortraitId = "" },
        new EventRecord { Id = "dungeon_door", Trigger = "Talk", Text = "The door is locked. You need a key.", PortraitId = "" },
    };

    private static List<EventRecord> _all = new(BuiltIn);
    private static Dictionary<string, EventRecord>? _byId;

    public static IReadOnlyList<EventRecord> All => _all;

    public static void AddRangeFromJson(IEnumerable<EventRecord> records)
    {
        if (records == null) return;
        foreach (var e in records) _all.Add(e);
        _byId = null;
    }

    public static EventRecord? Get(string id)
    {
        _byId ??= new Dictionary<string, EventRecord>();
        if (_byId.Count == 0)
            foreach (var e in _all) _byId[e.Id] = e;
        return _byId.TryGetValue(id ?? "", out var r) ? r : null;
    }
}
