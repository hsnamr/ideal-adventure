using System.IO;
using SharpSixteen.Core.Jrpg;

namespace SharpSixteen.Core.Jrpg.MapData;

/// <summary>
/// Provides map streams for each area. Format: . = floor, # = wall, H = house,
/// P = player spawn, 1 = door to Cave, 2 = door to Town (spawn 0),
/// 3,4,5 = door to House1,2,3 (also town spawn 1,2,3),
/// 6,7,8 = door to Town spawn 1,2,3 (from inside house),
/// 9 = door to WorldMap (town spawn 4 = return from world), E = encounter, N = NPC.
/// </summary>
public static class MapData
{
    public static Stream GetMapStream(MapId id)
    {
        string text = id switch
        {
            MapId.Town => TownMap.Data,
            MapId.Cave => CaveMap.Data,
            MapId.Field => FieldMap.Data,
            MapId.Dungeon => DungeonMap.Data,
            MapId.WorldMap => WorldMapMap.Data,
            MapId.House1 => House1Map.Data,
            MapId.House2 => House2Map.Data,
            MapId.House3 => House3Map.Data,
            _ => TownMap.Data
        };
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(text);
        writer.Flush();
        ms.Position = 0;
        return ms;
    }
}
