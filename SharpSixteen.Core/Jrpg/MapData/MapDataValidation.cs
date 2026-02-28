using System;

namespace SharpSixteen.Core.Jrpg.MapData;

/// <summary>
/// Validation for map data to avoid line length mismatches at runtime.
/// Each map file calls <see cref="ValidateLineLengths"/> from a static constructor
/// so invalid maps fail as soon as the type is loaded (before any map is used).
/// (C# does not treat string.Length as a constant expression, so compile-time checks
/// like <c>const int _ = 1 / (Line.Length == Width ? 1 : 0);</c> are not possible here.)
/// </summary>
public static class MapDataValidation
{
    /// <summary>Expected character width of every line in the town map.</summary>
    public const int TownWidth = 40;

    /// <summary>Expected character width of every line in the cave map.</summary>
    public const int CaveWidth = 48;

    /// <summary>Expected character width of every line in the world map.</summary>
    public const int WorldMapWidth = 40;

    /// <summary>Expected character width of every line in house interior maps.</summary>
    public const int HouseWidth = 12;

    /// <summary>
    /// Validates that every line in the map string has the expected length.
    /// Call from a map type's static constructor. Throws if any line length differs.
    /// </summary>
    /// <param name="mapData">Full map string (lines separated by \n).</param>
    /// <param name="expectedWidth">Expected character count per line.</param>
    /// <param name="mapName">Name of the map (for error message).</param>
    public static void ValidateLineLengths(string mapData, int expectedWidth, string mapName = "Map")
    {
        if (string.IsNullOrEmpty(mapData))
            throw new InvalidOperationException($"{mapName}: empty map data.");
        var lines = mapData.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            int len = lines[i].Length;
            if (len != expectedWidth)
                throw new InvalidOperationException(
                    $"{mapName} line {i + 1} length mismatch: expected {expectedWidth}, got {len}.");
        }
    }
}
