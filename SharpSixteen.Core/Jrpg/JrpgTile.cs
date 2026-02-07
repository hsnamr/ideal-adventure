using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SharpSixteen.Core.Jrpg;

/// <summary>
/// One tile in a JRPG map. 16x16 pixels (SNES-style).
/// </summary>
public struct JrpgTile
{
    public const int Width = 16;
    public const int Height = 16;
    public static readonly Vector2 Size = new(Width, Height);

    public Texture2D Texture { get; set; }
    public JrpgTileKind Kind { get; set; }
    /// <summary>When Kind is Door, which map to transition to.</summary>
    public MapId DoorTarget { get; set; }
    /// <summary>When Kind is Door, spawn index on target map (0-based).</summary>
    public int DoorSpawnIndex { get; set; }

    public bool IsWalkable => Kind == JrpgTileKind.Floor || Kind == JrpgTileKind.Door || Kind == JrpgTileKind.Encounter || Kind == JrpgTileKind.Npc;
}
