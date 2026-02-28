using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#nullable enable

namespace SharpSixteen.Core.Jrpg;

/// <summary>
/// A single overworld map (town or cave): tile grid, player spawns, doors.
/// Map file format: one character per tile. . = floor, # = wall, P = player spawn,
/// 1 = door to Cave (spawn 0), 2 = door to Town (spawn 0), E = encounter tile.
/// </summary>
public class JrpgMap
{
    private JrpgTile[,] _tiles = null!;
    private readonly List<Point> _spawns = new();
    private readonly List<Point> _npcs = new();
    private readonly ContentManager _content;
    private readonly string _tileSetPrefix;
    private Texture2D? _caveTileset;

    private const int CaveTilesPerRow = 23;
    // Buch tileset: not a strict grid; try row 3 (y=48) for floor/wall/exit.
    private static Rectangle CaveTileRect(int index) =>
        new((index % CaveTilesPerRow) * JrpgTile.Width, (index / CaveTilesPerRow) * JrpgTile.Height, JrpgTile.Width, JrpgTile.Height);
    private static int CaveFloorIndex => 69;
    private static int CaveWallIndex => 70;
    private static int CaveExitIndex => 71;

    public int Width => _tiles.GetLength(0);
    public int Height => _tiles.GetLength(1);
    public MapId Id { get; }
    public IReadOnlyList<Point> Spawns => _spawns;
    public IReadOnlyList<Point> NpcPositions => _npcs;

    /// <param name="tileSetPrefix">Content path prefix. If null, uses MapTileSetConfig for the given id (Town/Cave/WorldMap tilesets).</param>
    public JrpgMap(ContentManager content, MapId id, Stream mapStream, string? tileSetPrefix = null)
    {
        _content = content;
        Id = id;
        _tileSetPrefix = tileSetPrefix ?? MapTileSetConfig.GetPrefix(id);
        LoadFromStream(mapStream);
    }

    private void LoadFromStream(Stream stream)
    {
        var lines = new List<string>();
        using (var reader = new StreamReader(stream))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
                lines.Add(line);
        }

        if (lines.Count == 0) throw new InvalidOperationException("Empty map file.");
        if (Id == MapId.Cave)
        {
            try
            {
                var tex = _content.Load<Texture2D>("TilesDungeon/dungeon_tiles_0");
                if (tex != null && tex.Width >= 48 && tex.Height >= 32)
                    _caveTileset = tex;
                else
                    _caveTileset = null;
            }
            catch { _caveTileset = null; }
            _caveTileset = null;
        }
        int width = lines[0].Length;
        int height = lines.Count;
        _tiles = new JrpgTile[width, height];

        for (int y = 0; y < height; y++)
        {
            if (lines[y].Length != width)
                throw new InvalidOperationException($"Map line {y + 1} length mismatch.");
            for (int x = 0; x < width; x++)
            {
                _tiles[x, y] = ParseTile(lines[y][x], x, y);
            }
        }
    }

    private JrpgTile ParseTile(char c, int x, int y)
    {
        var tile = new JrpgTile();
        switch (c)
        {
            case '.':
                tile.Kind = JrpgTileKind.Floor;
                if (_caveTileset != null) { tile.Texture = _caveTileset; tile.SourceRect = CaveTileRect(CaveFloorIndex); }
                else tile.Texture = LoadTexture(MapTileSetConfig.GetFloor(Id), MapTileSetConfig.GetFallbackFloor(Id));
                break;
            case '#':
                tile.Kind = JrpgTileKind.Wall;
                if (_caveTileset != null) { tile.Texture = _caveTileset; tile.SourceRect = CaveTileRect(CaveWallIndex); }
                else tile.Texture = LoadTexture(MapTileSetConfig.GetWall(Id), MapTileSetConfig.GetFallbackWall(Id));
                break;
            case 'P':
                tile.Kind = JrpgTileKind.Floor;
                if (_caveTileset != null) { tile.Texture = _caveTileset; tile.SourceRect = CaveTileRect(CaveFloorIndex); }
                else tile.Texture = LoadTexture(MapTileSetConfig.GetFloor(Id), MapTileSetConfig.GetFallbackFloor(Id));
                _spawns.Add(new Point(x, y));
                break;
            case '1':
                tile.Kind = JrpgTileKind.Door;
                tile.Texture = LoadTexture(MapTileSetConfig.GetWell(Id), MapTileSetConfig.GetFallbackDoor(Id));
                tile.DoorTarget = MapId.Cave;
                tile.DoorSpawnIndex = 0;
                break;
            case '2':
                tile.Kind = JrpgTileKind.Door;
                if (Id == MapId.Cave && _caveTileset != null) { tile.Texture = _caveTileset; tile.SourceRect = CaveTileRect(CaveExitIndex); }
                else tile.Texture = LoadTexture(MapTileSetConfig.GetDoor(Id), MapTileSetConfig.GetFallbackDoor(Id));
                tile.DoorTarget = Id == MapId.Dungeon ? MapId.Field : MapId.Town;
                tile.DoorSpawnIndex = Id == MapId.WorldMap ? 4 : 0;
                break;
            case '3':
                if (Id == MapId.WorldMap)
                {
                    tile.Kind = JrpgTileKind.Door;
                    tile.Texture = LoadTexture(MapTileSetConfig.GetDoor(Id), MapTileSetConfig.GetFallbackDoor(Id));
                    tile.DoorTarget = MapId.Field;
                    tile.DoorSpawnIndex = 0;
                }
                else if (Id == MapId.Field)
                {
                    tile.Kind = JrpgTileKind.Door;
                    tile.Texture = LoadTexture(MapTileSetConfig.GetDoor(Id), MapTileSetConfig.GetFallbackDoor(Id));
                    tile.DoorTarget = MapId.Dungeon;
                    tile.DoorSpawnIndex = 0;
                }
                else
                {
                    tile.Kind = JrpgTileKind.Door;
                    tile.Texture = LoadTexture(MapTileSetConfig.GetDoor(Id), MapTileSetConfig.GetFallbackDoor(Id));
                    tile.DoorTarget = MapId.House1;
                    tile.DoorSpawnIndex = 0;
                    _spawns.Add(new Point(x, y));
                }
                break;
            case '4':
                tile.Kind = JrpgTileKind.Door;
                tile.Texture = LoadTexture(MapTileSetConfig.GetDoor(Id), MapTileSetConfig.GetFallbackDoor(Id));
                tile.DoorTarget = MapId.House2;
                tile.DoorSpawnIndex = 0;
                _spawns.Add(new Point(x, y));
                break;
            case '5':
                tile.Kind = JrpgTileKind.Door;
                tile.Texture = LoadTexture(MapTileSetConfig.GetDoor(Id), MapTileSetConfig.GetFallbackDoor(Id));
                tile.DoorTarget = MapId.House3;
                tile.DoorSpawnIndex = 0;
                _spawns.Add(new Point(x, y));
                break;
            case '6':
                tile.Kind = JrpgTileKind.Door;
                tile.Texture = LoadTexture(MapTileSetConfig.GetDoor(Id), MapTileSetConfig.GetFallbackDoor(Id));
                tile.DoorTarget = MapId.Town;
                tile.DoorSpawnIndex = 1;
                break;
            case '7':
                tile.Kind = JrpgTileKind.Door;
                tile.Texture = LoadTexture(MapTileSetConfig.GetDoor(Id), MapTileSetConfig.GetFallbackDoor(Id));
                tile.DoorTarget = MapId.Town;
                tile.DoorSpawnIndex = 2;
                break;
            case '8':
                tile.Kind = JrpgTileKind.Door;
                tile.Texture = LoadTexture(MapTileSetConfig.GetDoor(Id), MapTileSetConfig.GetFallbackDoor(Id));
                tile.DoorTarget = MapId.Town;
                tile.DoorSpawnIndex = 3;
                break;
            case '9':
                tile.Kind = JrpgTileKind.Door;
                tile.Texture = LoadTexture(MapTileSetConfig.GetDoor(Id), MapTileSetConfig.GetFallbackDoor(Id));
                tile.DoorTarget = MapId.WorldMap;
                tile.DoorSpawnIndex = 0;
                _spawns.Add(new Point(x, y));
                break;
            case 'E':
                tile.Kind = JrpgTileKind.Encounter;
                if (_caveTileset != null) { tile.Texture = _caveTileset; tile.SourceRect = CaveTileRect(CaveFloorIndex); }
                else tile.Texture = LoadTexture(MapTileSetConfig.GetFloor(Id), MapTileSetConfig.GetFallbackFloor(Id));
                break;
            case 'H':
                tile.Kind = JrpgTileKind.House;
                tile.Texture = LoadTexture(MapTileSetConfig.GetHouse(Id), "House");
                break;
            case 'N':
                tile.Kind = JrpgTileKind.Npc;
                tile.Texture = LoadTexture(MapTileSetConfig.GetFloor(Id), MapTileSetConfig.GetFallbackFloor(Id));
                _npcs.Add(new Point(x, y));
                break;
            default:
                tile.Kind = JrpgTileKind.Floor;
                if (_caveTileset != null) { tile.Texture = _caveTileset; tile.SourceRect = CaveTileRect(CaveFloorIndex); }
                else tile.Texture = LoadTexture(MapTileSetConfig.GetFloor(Id), MapTileSetConfig.GetFallbackFloor(Id));
                break;
        }
        return tile;
    }

    private Texture2D? LoadTexture(string name, string? fallbackName = null)
    {
        try
        {
            return _content.Load<Texture2D>(_tileSetPrefix + name);
        }
        catch
        {
            if (fallbackName != null)
            {
                try
                {
                    return _content.Load<Texture2D>("Tiles/" + fallbackName);
                }
                catch { }
            }
            return null;
        }
    }

    public JrpgTile GetTile(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return new JrpgTile { Kind = JrpgTileKind.Wall };
        return _tiles[x, y];
    }

    public bool CanWalk(int x, int y)
    {
        return GetTile(x, y).IsWalkable;
    }

    public bool TryGetDoor(int x, int y, out MapId target, out int spawnIndex)
    {
        var t = GetTile(x, y);
        if (t.Kind == JrpgTileKind.Door)
        {
            target = t.DoorTarget;
            spawnIndex = t.DoorSpawnIndex;
            return true;
        }
        target = default;
        spawnIndex = 0;
        return false;
    }

    public bool IsEncounterTile(int x, int y) => GetTile(x, y).Kind == JrpgTileKind.Encounter;

    public Point GetSpawn(int index)
    {
        if (index < 0 || index >= _spawns.Count)
            return new Point(1, 1);
        return _spawns[index];
    }

    public void Draw(SpriteBatch batch, Vector2 cameraPosition, Vector2 viewSize)
    {
        int tx0 = (int)(cameraPosition.X / JrpgTile.Width);
        int ty0 = (int)(cameraPosition.Y / JrpgTile.Height);
        int tx1 = (int)((cameraPosition.X + viewSize.X) / JrpgTile.Width) + 1;
        int ty1 = (int)((cameraPosition.Y + viewSize.Y) / JrpgTile.Height) + 1;
        tx0 = Math.Clamp(tx0, 0, Width - 1);
        ty0 = Math.Clamp(ty0, 0, Height - 1);
        tx1 = Math.Clamp(tx1, 0, Width);
        ty1 = Math.Clamp(ty1, 0, Height);

        var dest = new Rectangle(0, 0, JrpgTile.Width, JrpgTile.Height);
        for (int y = ty0; y < ty1; y++)
            for (int x = tx0; x < tx1; x++)
            {
                var t = _tiles[x, y];
                if (t.Texture != null)
                {
                    dest.X = x * JrpgTile.Width;
                    dest.Y = y * JrpgTile.Height;
                    if (t.SourceRect is Rectangle src)
                        batch.Draw(t.Texture, dest, src, Color.White);
                    else
                        batch.Draw(t.Texture, dest, Color.White);
                }
            }
    }
}
