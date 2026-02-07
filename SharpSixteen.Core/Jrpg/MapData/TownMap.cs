namespace SharpSixteen.Core.Jrpg.MapData;

/// <summary>Town with houses and NPCs; doors 1 to cave, 3,4,5 to houses.</summary>
public static class TownMap
{
    private const string Line0 = "########################################";
    private const string Line1 = "#..............HHHH..............HHHH..#";
    private const string Line2 = "#..............HHHH....N.........HHHH..#";
    private const string Line3 = "#..............HHHH..............HHHH..#";
    private const string Line4 = "#................3..................4..#";
    private const string Line5 = "#..............HHHH..............HHHH..#";
    private const string Line6 = "#..............HHHH..............HHHH..#";
    private const string Line7 = "#....P.................................#";
    private const string Line8 = "#.................................1....#";
    private const string Line9 = "#..............HHHH..............HHHH..#";
    private const string Line10 = "#..............HHHH....N.........HHHH..#";
    private const string Line11 = "#..............HHHH..............HHHH..#";
    private const string Line12 = "#................5.....................#";
    private const string Line13 = "#..............HHHH..............HHHH..#";
    private const string Line14 = "#..............HHHH..............HHHH..#";
    private const string Line15 = "#..N............N..........N........N..#";
    private const string Line16 = "#..............HHHH..............HHHH..#";
    private const string Line17 = "#..............HHHH..............HHHH..#";
    private const string Line18 = "#..............HHHH..............HHHH..#";
    private const string Line19 = "#..............HHHH..............HHHH..#";
    private const string Line20 = "#..............HHHH..............HHHH..#";
    private const string Line21 = "########################################";

    public static readonly string Data =
        Line0 + "\n" + Line1 + "\n" + Line2 + "\n" + Line3 + "\n" + Line4 + "\n" + Line5 + "\n" +
        Line6 + "\n" + Line7 + "\n" + Line8 + "\n" + Line9 + "\n" + Line10 + "\n" + Line11 + "\n" +
        Line12 + "\n" + Line13 + "\n" + Line14 + "\n" + Line15 + "\n" + Line16 + "\n" + Line17 + "\n" +
        Line18 + "\n" + Line19 + "\n" + Line20 + "\n" + Line21;

    static TownMap()
    {
        MapDataValidation.ValidateLineLengths(Data, MapDataValidation.TownWidth, nameof(TownMap));
    }
}
