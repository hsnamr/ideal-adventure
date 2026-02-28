namespace SharpSixteen.Core.Save;

/// <summary>
/// Serializable game state for save/load. Map and position use string/enums for JSON.
/// </summary>
public class GameSaveData
{
    public string MapId { get; set; } = "Town";
    public int PlayerTileX { get; set; }
    public int PlayerTileY { get; set; }
    public int PlayerHp { get; set; } = 30;
    public int PlayerMaxHp { get; set; } = 30;
    public int PlayerMp { get; set; } = 20;
    public int PlayerMaxMp { get; set; } = 20;
    public double TimePlayedSeconds { get; set; }
    public string SaveTimestampUtc { get; set; } = "";
}
