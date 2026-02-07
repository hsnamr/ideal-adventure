namespace SharpSixteen.Core.Jrpg.MapData;

/// <summary>House 1 interior; 6 = door back to Town (spawn 1).</summary>
public static class House1Map
{
    private const string Line0 = "############";
    private const string Line1 = "#..........#";
    private const string Line2 = "#..........#";
    private const string Line3 = "#....P.....#";
    private const string Line4 = "#..........#";
    private const string Line5 = "#..........#";
    private const string Line6 = "#.....6....#";
    private const string Line7 = "############";

    public static readonly string Data =
        Line0 + "\n" + Line1 + "\n" + Line2 + "\n" + Line3 + "\n" + Line4 + "\n" + Line5 + "\n" +
        Line6 + "\n" + Line7;

    static House1Map()
    {
        MapDataValidation.ValidateLineLengths(Data, MapDataValidation.HouseWidth, nameof(House1Map));
    }
}
