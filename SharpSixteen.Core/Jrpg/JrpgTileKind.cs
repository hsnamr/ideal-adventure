namespace SharpSixteen.Core.Jrpg;

/// <summary>
/// Type of a single tile in a JRPG map (16-bit style).
/// </summary>
public enum JrpgTileKind
{
    /// <summary>Walkable floor.</summary>
    Floor,
    /// <summary>Impassable wall.</summary>
    Wall,
    /// <summary>Door/stairs to another map; TargetMapId and spawn index set in map data.</summary>
    Door,
    /// <summary>Tile that can trigger random battles (e.g. cave floor).</summary>
    Encounter
}
