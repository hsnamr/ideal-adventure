using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SharpSixteen.Core.Database;

/// <summary>
/// Loads items, skills, events, and enemies from JSON files in a database folder.
/// Call once at startup (e.g. from game LoadContent). Files are optional.
/// </summary>
public static class DatabaseLoader
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    /// <summary>
    /// Tries to load all database JSON files from the given directory.
    /// Typical path: Path.Combine(AppContext.BaseDirectory, "Content", "database")
    /// or a "database" folder next to the executable.
    /// </summary>
    public static void LoadFromDirectory(string directory)
    {
        if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
            return;

        TryLoadFile(Path.Combine(directory, "items.json"), json =>
        {
            var list = JsonSerializer.Deserialize<List<ItemRecord>>(json, JsonOptions);
            if (list != null) Items.AddRangeFromJson(list);
        });

        TryLoadFile(Path.Combine(directory, "skills.json"), json =>
        {
            var list = JsonSerializer.Deserialize<List<SkillRecord>>(json, JsonOptions);
            if (list != null) Skills.AddRangeFromJson(list);
        });

        TryLoadFile(Path.Combine(directory, "events.json"), json =>
        {
            var list = JsonSerializer.Deserialize<List<EventRecord>>(json, JsonOptions);
            if (list != null) Events.AddRangeFromJson(list);
        });

        TryLoadFile(Path.Combine(directory, "enemies.json"), json =>
        {
            var list = JsonSerializer.Deserialize<List<EnemyRecord>>(json, JsonOptions);
            if (list != null) Enemies.AddRangeFromJson(list);
        });
    }

    private static void TryLoadFile(string path, Action<string> onLoaded)
    {
        try
        {
            if (!File.Exists(path)) return;
            string json = File.ReadAllText(path);
            if (!string.IsNullOrWhiteSpace(json))
                onLoaded(json);
        }
        catch
        {
            // Ignore missing or invalid files
        }
    }
}
