namespace SharpSixteen.Core.Jrpg.MapData;

/// <summary>Outdoor field. Tile 2 = exit to Town (spawn 0), tile 3 = exit to Dungeon (spawn 0).</summary>
public static class FieldMap
{
    private const int W = 40;
    private const string Line0 = "########################################";
    private const string Line1 = "#......................................#";
    private const string Line2 = "#......................................#";
    private const string Line3 = "#......................................#";
    private const string Line4 = "#............E.....E...................#";
    private const string Line5 = "#......................................#";
    private const string Line6 = "#.................P....................#";
    private const string Line7 = "#......................................#";
    private const string Line8 = "#............E.....E...................#";
    private const string Line9 = "#......................................#";
    private const string Line10 = "#.....................................2#";
    private const string Line11 = "#......................................#";
    private const string Line12 = "#..............3.......................#";
    private const string Line13 = "#......................................#";
    private const string Line14 = "########################################";

    public static readonly string Data =
        Line0 + "\n" + Line1 + "\n" + Line2 + "\n" + Line3 + "\n" + Line4 + "\n" + Line5 + "\n" +
        Line6 + "\n" + Line7 + "\n" + Line8 + "\n" + Line9 + "\n" + Line10 + "\n" + Line11 + "\n" +
        Line12 + "\n" + Line13 + "\n" + Line14;

    static FieldMap()
    {
        MapDataValidation.ValidateLineLengths(Data, W, nameof(FieldMap));
    }
}
