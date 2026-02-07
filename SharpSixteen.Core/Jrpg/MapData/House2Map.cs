namespace SharpSixteen.Core.Jrpg.MapData;

/// <summary>House 2 interior; 7 = door back to Town (spawn 2).</summary>
public static class House2Map
{
    private const string Line0 = "############";
    private const string Line1 = "#..........#";
    private const string Line2 = "#..........#";
    private const string Line3 = "#....P.....#";
    private const string Line4 = "#..........#";
    private const string Line5 = "#.....7....#";
    private const string Line6 = "############";

    public static readonly string Data =
        Line0 + "\n" + Line1 + "\n" + Line2 + "\n" + Line3 + "\n" + Line4 + "\n" + Line5 + "\n" +
        Line6;

    static House2Map()
    {
        MapDataValidation.ValidateLineLengths(Data, MapDataValidation.HouseWidth, nameof(House2Map));
    }
}
