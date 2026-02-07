namespace SharpSixteen.Core.Jrpg.MapData;

/// <summary>House 3 interior; 8 = door back to Town (spawn 3).</summary>
public static class House3Map
{
    private const string Line0 = "############";
    private const string Line1 = "#..........#";
    private const string Line2 = "#....P.....#";
    private const string Line3 = "#..........#";
    private const string Line4 = "#.....8....#";
    private const string Line5 = "############";

    public static readonly string Data =
        Line0 + "\n" + Line1 + "\n" + Line2 + "\n" + Line3 + "\n" + Line4 + "\n" + Line5;

    static House3Map()
    {
        MapDataValidation.ValidateLineLengths(Data, MapDataValidation.HouseWidth, nameof(House3Map));
    }
}
