# Optional tile assets (OpenGameArt)

**For a full list of free galleries and how to use sprites + tiles, see [ART_STYLE_GUIDE.md](ART_STYLE_GUIDE.md).**

The JRPG overworld uses **map-specific tile sets** when present. If a set is missing, the game falls back to built-in `Tiles/` (BlockB0, BlockA0, Exit, House). All overworld tiles are **16×16** pixels.

## Quick reference – tile folders

| Folder         | Purpose   | Suggested source (see ART_STYLE_GUIDE) |
|----------------|-----------|----------------------------------------|
| `TilesTown/`   | Town map  | RPG Town Pixel Art (CC0)              |
| `TilesDungeon/`| Cave      | Dungeon tileset (CC0)                 |
| `TilesWorld/`   | World map | Worldmap/Overworld (CC-BY)            |

Required texture names and fallbacks are in [ART_STYLE_GUIDE.md](ART_STYLE_GUIDE.md). Add each PNG to `SharpSixteen.mgcb` with the same path (e.g. `TilesTown/Floor.png`).

## Custom / alternate sprites

Place **optional** overworld sprites in `Sprites/Jrpg/Custom/`:

- `Hero.png` – overworld player (16×16)
- `Npc.png` – NPCs (16×16)
- `EnemySlime.png` – enemy sprite (16×16)

If a file exists in `Custom/`, the game uses it instead of the default in `Sprites/Jrpg/`. Add them to the content pipeline under the same path.
