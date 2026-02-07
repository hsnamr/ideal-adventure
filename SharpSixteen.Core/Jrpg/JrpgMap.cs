using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SharpSixteen.Core.Jrpg;

/// <summary>
/// A single overworld map (town or cave): tile grid, player spawns, doors.
/// Map file format: one character per tile. . = floor, # = wall, P = player spawn,
/// 1 = door to Cave (spawn 0), 2 = door to Town (spawn 0), E = encounter tile.
/// </summary>
public class JrpgMap
{
    private JrpgTile[,] _tiles;
    private readonly List<Point> _spawns = new();
    private readonly ContentManager _content;
    private readonly string _tileSetPrefix;

    public int Width => _tiles.GetLength(0);
    public int Height => _tiles.GetLength(1);
    public MapId Id { get; }
    public IReadOnlyList<Point> Spawns => _spawns;

    /// <param name="tileSetPrefix">Content path prefix. Use "Tiles/" and floor=BlockB0, wall=BlockA0, door=Exit for built-in tiles.</param>
    public JrpgMap(ContentManager content, MapId id, Stream mapStream, string tileSetPrefix = "Tiles/")
    {
        _content = content;
        Id = id;
        _tileSetPrefix = tileSetPrefix;
        LoadFromStream(mapStream);
    }

    private void LoadFromStream(Stream stream)
    {
        var lines = new List<string>();
        using (var reader = new StreamReader(stream))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
                lines.Add(line);
        }

        if (lines.Count == 0) throw new InvalidOperationException("Empty map file.");
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
                tile.Texture = LoadTexture("BlockB0");
                break;
            case '#':
                tile.Kind = JrpgTileKind.Wall;
                tile.Texture = LoadTexture("BlockA0");
                break;
            case 'P':
                tile.Kind = JrpgTileKind.Floor;
                tile.Texture = LoadTexture("BlockB0");
                _spawns.Add(new Point(x, y));
                break;
            case '1':
                tile.Kind = JrpgTileKind.Door;
                tile.Texture = LoadTexture("Exit");
                tile.DoorTarget = MapId.Cave;
                tile.DoorSpawnIndex = 0;
                break;
            case '2':
                tile.Kind = JrpgTileKind.Door;
                tile.Texture = LoadTexture("Exit");
                tile.DoorTarget = MapId.Town;
                tile.DoorSpawnIndex = 0;
                break;
            case 'E':
                tile.Kind = JrpgTileKind.Encounter;
                tile.Texture = LoadTexture("BlockB0");
                break;
            default:
                tile.Kind = JrpgTileKind.Floor;
                tile.Texture = LoadTexture("BlockB0");
                break;
        }
        return tile;
    }

    private Texture2D LoadTexture(string name)
    {
        try
        {
            return _content.Load<Texture2D>(_tileSetPrefix + name);
        }
        catch
        {
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
                    batch.Draw(t.Texture, dest, Color.White);
                }
            }
    }
}
