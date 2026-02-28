using System;
using System.IO;
using System.Text.Json;
using SharpSixteen.Core.Jrpg;

namespace SharpSixteen.Core.Save;

/// <summary>
/// Persists and loads game save data. Uses ApplicationData/SharpSixteen/save.json (desktop)
/// or equivalent. Holds current session state for the JRPG overworld and battle.
/// </summary>
public class SaveManager
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    private readonly string _saveDirectory;
    private readonly string _saveFilePath;
    private GameSaveData _current = new();

    public SaveManager()
    {
        _saveDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SharpSixteen");
        _saveFilePath = Path.Combine(_saveDirectory, "save.json");
    }

    /// <summary>Current in-memory state (map, position, HP, time).</summary>
    public GameSaveData Current => _current;

    /// <summary>True if a save file exists and can be loaded.</summary>
    public bool HasSave()
    {
        return File.Exists(_saveFilePath);
    }

    /// <summary>Resets current state to new game (Town, spawn 0, full HP, 0 time).</summary>
    public void ResetToNewGame()
    {
        _current = new GameSaveData
        {
            MapId = nameof(MapId.Town),
            PlayerTileX = 4,
            PlayerTileY = 7,
            PlayerHp = 30,
            PlayerMaxHp = 30,
            PlayerMp = 20,
            PlayerMaxMp = 20,
            TimePlayedSeconds = 0,
            SaveTimestampUtc = ""
        };
    }

    /// <summary>Loads from disk into Current. Returns true if loaded, false if no file or error.</summary>
    public bool Load()
    {
        if (!HasSave()) return false;
        try
        {
            string json = File.ReadAllText(_saveFilePath);
            var data = JsonSerializer.Deserialize<GameSaveData>(json);
            if (data != null)
            {
                _current = data;
                return true;
            }
        }
        catch { }
        return false;
    }

    /// <summary>Writes Current to disk.</summary>
    public void Save()
    {
        _current.SaveTimestampUtc = DateTime.UtcNow.ToString("O");
        try
        {
            if (!Directory.Exists(_saveDirectory))
                Directory.CreateDirectory(_saveDirectory);
            string json = JsonSerializer.Serialize(_current, JsonOptions);
            File.WriteAllText(_saveFilePath, json);
        }
        catch { }
    }

    /// <summary>Updates current state from overworld (map and tile position).</summary>
    public void UpdatePosition(MapId mapId, int tileX, int tileY)
    {
        _current.MapId = mapId.ToString();
        _current.PlayerTileX = tileX;
        _current.PlayerTileY = tileY;
    }

    /// <summary>Updates current HP (e.g. after battle).</summary>
    public void UpdateHp(int hp, int maxHp)
    {
        _current.PlayerHp = Math.Clamp(hp, 0, maxHp);
        _current.PlayerMaxHp = Math.Max(1, maxHp);
    }

    /// <summary>Updates current MP (e.g. after battle).</summary>
    public void UpdateMp(int mp, int maxMp)
    {
        _current.PlayerMp = Math.Clamp(mp, 0, maxMp);
        _current.PlayerMaxMp = Math.Max(0, maxMp);
    }

    /// <summary>Adds elapsed time to time played.</summary>
    public void AddTimePlayed(double seconds)
    {
        _current.TimePlayedSeconds += seconds;
    }

    /// <summary>Parses Current.MapId to MapId enum.</summary>
    public static MapId ParseMapId(string mapId)
    {
        return Enum.TryParse<MapId>(mapId, out var id) ? id : MapId.Town;
    }
}
