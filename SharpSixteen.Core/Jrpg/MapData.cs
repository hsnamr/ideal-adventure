using System.IO;

namespace SharpSixteen.Core.Jrpg;

/// <summary>
/// Embedded map data for Town and Cave (avoids file deployment).
/// Format: . = floor, # = wall, P = player spawn, 1 = door to Cave, 2 = door to Town, E = encounter.
/// </summary>
public static class MapData
{
    /// <summary>Small town: a few buildings, one door (1) to cave.</summary>
    public static readonly string Town =
        "##############\n" +
        "#............#\n" +
        "#....P.......#\n" +
        "#............#\n" +
        "#.......1....#\n" +
        "#............#\n" +
        "##############";

    /// <summary>Small cave: walls, encounter tiles (E), door (2) back to town.</summary>
    public static readonly string Cave =
        "##############\n" +
        "#EEE.....EEE.#\n" +
        "#E........E.#\n" +
        "#....P......#\n" +
        "#...........#\n" +
        "#.....2.....#\n" +
        "#EEE.....EEE.#\n" +
        "##############";

    public static Stream GetMapStream(MapId id)
    {
        string text = id == MapId.Town ? Town : Cave;
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(text);
        writer.Flush();
        ms.Position = 0;
        return ms;
    }
}
