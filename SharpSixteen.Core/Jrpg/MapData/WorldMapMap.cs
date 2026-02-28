namespace SharpSixteen.Core.Jrpg.MapData;

/// <summary>Overworld / world map. Tile 2 = exit to Town (spawn 4).</summary>
public static class WorldMapMap
{
    private const string Line0 = "########################################";
    private const string Line1 = "#......................................#";
    private const string Line2 = "#......................................#";
    private const string Line3 = "#......................................#";
    private const string Line4 = "#......................................#";
    private const string Line5 = "#......................................#";
    private const string Line6 = "#.................P....................#";
    private const string Line7 = "#......................................#";
    private const string Line8 = "#......................................#";
    private const string Line9 = "#......................................#";
    private const string Line10 = "#.....................................2#";
    private const string Line11 = "#......................................#";
    private const string Line12 = "#.................3....................#";
    private const string Line13 = "########################################";

    public static readonly string Data =
        Line0 + "\n" + Line1 + "\n" + Line2 + "\n" + Line3 + "\n" + Line4 + "\n" + Line5 + "\n" +
        Line6 + "\n" + Line7 + "\n" + Line8 + "\n" + Line9 + "\n" + Line10 + "\n" + Line11 + "\n" +
        Line12 + "\n" + Line13;

    static WorldMapMap()
    {
        MapDataValidation.ValidateLineLengths(Data, MapDataValidation.WorldMapWidth, nameof(WorldMapMap));
    }
}
