using System.Collections.Generic;

namespace SharpSixteen.Core.Jrpg;

/// <summary>
/// Tile set configuration per map for OpenGameArt-style tiles.
/// Town: RPG Town pack. Cave: Dungeon tileset. WorldMap: Worldmap/Overworld tileset.
/// Falls back to built-in Tiles/ when map-specific assets are not present.
/// </summary>
public static class MapTileSetConfig
{
    /// <summary>Content path prefix and texture names for each map type.</summary>
    public static string GetPrefix(MapId id)
    {
        return id switch
        {
            MapId.Town => "TilesTown/",
            MapId.Cave => "TilesDungeon/",
            MapId.Field => "TilesWorld/",
            MapId.Dungeon => "TilesDungeon/",
            MapId.WorldMap => "TilesWorld/",
            _ => "Tiles/"
        };
    }

    /// <summary>Floor/walkable tile name (e.g. road, grass, dungeon floor).</summary>
    public static string GetFloor(MapId id)
    {
        return id switch
        {
            MapId.Town => "Floor",
            MapId.Cave => "Floor",
            MapId.Field => "Grass",
            MapId.Dungeon => "Floor",
            MapId.WorldMap => "Grass",
            _ => "BlockB0"
        };
    }

    /// <summary>Wall/impassable tile name.</summary>
    public static string GetWall(MapId id)
    {
        return id switch
        {
            MapId.Town => "Wall",
            MapId.Cave => "Wall",
            MapId.Field => "Water",
            MapId.Dungeon => "Wall",
            MapId.WorldMap => "Water",
            _ => "BlockA0"
        };
    }

    /// <summary>Generic door/exit tile name.</summary>
    public static string GetDoor(MapId id)
    {
        return id switch
        {
            MapId.Town => "Door",
            MapId.Cave => "Exit",
            MapId.Field => "Exit",
            MapId.Dungeon => "Exit",
            MapId.WorldMap => "Exit",
            _ => "Exit"
        };
    }

    /// <summary>Cave entrance in town (water well). Used for tile '1' in town only.</summary>
    public static string GetWell(MapId id) => id == MapId.Town ? "Well" : "Exit";

    /// <summary>House/building tile name (town only).</summary>
    public static string GetHouse(MapId id) => id == MapId.Town ? "House" : "BlockA0";

    /// <summary>Fallback when map-specific texture fails to load.</summary>
    public static string GetFallbackFloor(MapId id) => "BlockB0";

    public static string GetFallbackWall(MapId id) => "BlockA0";

    public static string GetFallbackDoor(MapId id) => "Exit";
}
