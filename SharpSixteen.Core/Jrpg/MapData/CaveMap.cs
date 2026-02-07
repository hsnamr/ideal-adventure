namespace SharpSixteen.Core.Jrpg.MapData;

/// <summary>Doubled cave maze.</summary>
public static class CaveMap
{
    private const string Line0 = "################################################";
    private const string Line1 = "#..........................#...................#";
    private const string Line2 = "#.EEEE........EEEE.........#.EEEE........EEEE..#";
    private const string Line3 = "#.E..............E.........#.E..............E..#";
    private const string Line4 = "#.E....P.........E.........#.E..............E..#";
    private const string Line5 = "#.E................E.......#.E..............E..#";
    private const string Line6 = "#....EEEE........EEEE.......#.EEEE........EEEE.#";
    private const string Line7 = "#..........................#...................#";
    private const string Line8 = "######............##############################";
    private const string Line9 = "#......E........E........E........E............#";
    private const string Line10 = "#......E........E........E........E.......2....#";
    private const string Line11 = "#......EEEE.....EEEE.....EEEE.....EEEE.........#";
    private const string Line12 = "#..............................................#";
    private const string Line13 = "#..........................#...................#";
    private const string Line14 = "#.EEEE........EEEE.........#.EEEE........EEEE..#";
    private const string Line15 = "#.E..............E.........#.E..............E..#";
    private const string Line16 = "#.E..............E.........#.E..............E..#";
    private const string Line17 = "#.E..............E.........#.E..............E..#";
    private const string Line18 = "#.EEEE........EEEE.........#.EEEE........EEEE..#";
    private const string Line19 = "#..........................#...................#";
    private const string Line20 = "#..............................................#";
    private const string Line21 = "#......E........E........E........E............#";
    private const string Line22 = "#......E........E........E........E............#";
    private const string Line23 = "#......EEEE.....EEEE.....EEEE.....EEEE.........#";
    private const string Line24 = "#..............................................#";
    private const string Line25 = "################################################";

    public static readonly string Data =
        Line0 + "\n" + Line1 + "\n" + Line2 + "\n" + Line3 + "\n" + Line4 + "\n" + Line5 + "\n" +
        Line6 + "\n" + Line7 + "\n" + Line8 + "\n" + Line9 + "\n" + Line10 + "\n" + Line11 + "\n" +
        Line12 + "\n" + Line13 + "\n" + Line14 + "\n" + Line15 + "\n" + Line16 + "\n" + Line17 + "\n" +
        Line18 + "\n" + Line19 + "\n" + Line20 + "\n" + Line21 + "\n" + Line22 + "\n" + Line23 + "\n" +
        Line24 + "\n" + Line25;

    static CaveMap()
    {
        MapDataValidation.ValidateLineLengths(Data, MapDataValidation.CaveWidth, nameof(CaveMap));
    }
}
