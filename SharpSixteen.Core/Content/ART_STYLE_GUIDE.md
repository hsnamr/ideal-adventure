# Art style guide – free sprites and tiles

Use this guide to improve the game’s look with art from **free online galleries**. All paths are under `Content/`; add any new PNGs to `SharpSixteen.mgcb` with the same path.

---

## Free art galleries

| Gallery | Best for | License | URL |
|--------|----------|---------|-----|
| **OpenGameArt.org** | Tiles, sprites, music, sound | Mixed (CC0, CC-BY, etc.) | https://opengameart.org |
| **itch.io (CC0 assets)** | Sprites, tiles, UI, full packs | CC0 | https://itch.io/game-assets/assets-cc0 |
| **Kenney** | Tiles, characters, UI, audio | CC0 | https://kenney.nl/assets |
| **Lospec** | Pixel art palettes, references | Varies | https://lospec.com |
| **Craftpix (free section)** | 2D packs (some free) | Check per pack | https://craftpix.net/freebies/ |

**Tips:** Filter by **CC0** when you want no attribution. For **CC-BY**, credit the author in your game (e.g. credits screen or README).

---

## 1. JRPG overworld (town, cave, world map)

Tiles are **16×16**. The game uses **map-specific folders** when present; otherwise it falls back to `Tiles/` (BlockB0, BlockA0, Exit, House).

### Town – `TilesTown/`

| File   | Use           | Suggested source |
|--------|----------------|------------------|
| Floor.png | Ground, roads | [RPG Town Pixel Art](https://opengameart.org/content/rpg-town-pixel-art-assets) (CC0) – crop 16×16 |
| Wall.png  | Impassable   | Same pack |
| Door.png  | Doors        | Same pack |
| Well.png  | Cave entrance (well) | Same pack or [dungeon tileset](https://opengameart.org/content/dungeon-tileset) |
| House.png | Buildings    | Same pack |

**Add to mgcb:** `TilesTown/Floor.png`, etc. (same as other tiles – TextureImporter, no resize).

### Cave – `TilesDungeon/`

| File    | Use          | Suggested source |
|---------|--------------|------------------|
| Floor.png | Dungeon floor | [Dungeon tileset](https://opengameart.org/content/dungeon-tileset) (Buch, CC0) – crop 16×16 from sheet |
| Wall.png  | Cave walls   | Same |
| Exit.png  | Exit to town | Same (e.g. stairs/ladder tile) |

The project already has `TilesDungeon/dungeon_tiles_0.png` (full sheet). You can instead add **cropped** `Floor.png`, `Wall.png`, `Exit.png` from that sheet so the cave uses them via the normal tile loader.

### World map – `TilesWorld/`

| File    | Use       | Suggested source |
|---------|-----------|-------------------|
| Grass.png | Walkable | [Worldmap/Overworld tileset](https://opengameart.org/content/worldmapoverworld-tileset) (CC-BY 3.0 – attribute) |
| Water.png | Impassable | Same |
| Exit.png  | Exit to town | Same |

---

## 2. JRPG sprites (overworld + battle)

**16×16** top-down character sprites. Paths:

| Content path             | Use        | Suggested source |
|--------------------------|------------|------------------|
| `Sprites/Jrpg/Hero.png`  | Player     | [16×16 RPG Hero](https://lpc.opengameart.org/content/16x16-rpg-hero), [OGA 16×16 JRPG](https://opengameart.org/content/oga-16x16-jrpg-sprites-tiles), [16×16 character set](https://opengameart.org/content/16x16-8-bit-rpg-character-set) |
| `Sprites/Jrpg/Npc.png`   | NPCs       | Same packs – use a different frame or variant |
| `Sprites/Jrpg/EnemySlime.png` | Battle/overworld enemy | [Dungeon tileset](https://opengameart.org/content/dungeon-tileset) (has small characters), or any 16×16 enemy sprite |

**How to use:** Download a sprite sheet, pick one 16×16 frame (e.g. idle down), save as `Hero.png` (or Npc/EnemySlime), put in `Content/Sprites/Jrpg/`, add to `SharpSixteen.mgcb` under the same path. The game loads these by name.

---

## 3. Platformer (levels, player, enemies, pickups)

Used in the platformer levels (separate from the JRPG overworld).

| Content path | Use | Suggested source |
|--------------|-----|------------------|
| `Sprites/Player/` (Idle, Run, Jump, etc.) | Player animation | [Kenney Platformer Pack](https://kenney.nl/assets/platformer-characters-1), itch.io CC0 platformer characters |
| `Sprites/MonsterA/`, `MonsterB/`, etc. | Enemies | Kenney, OpenGameArt “platformer enemy” |
| `Sprites/Gem.png` | Collectible | Kenney, OpenGameArt “gem” or “collectible” |
| `Backgrounds/Layer0_*`, etc. | Parallax layers | Kenney, OpenGameArt “background” |
| `Tiles/Platform.png`, `BlockA*`, `BlockB*` | Level tiles | Kenney, [OpenGameArt tiles](https://opengameart.org/art-search-advanced?keys=tiles+platformer) |

---

## 4. UI and overlays

| Content path | Use | Suggested source |
|--------------|-----|-------------------|
| `Sprites/gradient.png` | Menus / dialogs | Any soft gradient (or generate) |
| `Sprites/blank.png` | Fallback / panels | 1×1 white/transparent PNG |
| `Overlays/you_died.png`, etc. | Game over / win | Kenney UI, OpenGameArt “game over” |

---

## 5. Adding assets step by step

1. **Download** from one of the galleries above (prefer CC0 or CC-BY with attribution).
2. **Resize/crop** to match expected size (16×16 for JRPG tiles and overworld sprites).
3. **Place** in `Content/` under the path in the tables (e.g. `Content/TilesTown/Floor.png`, `Content/Sprites/Jrpg/Hero.png`).
4. **Register in MonoGame:** open `Content/SharpSixteen.mgcb`, add the PNG with the same path (e.g. `TilesTown/Floor.png`), use **TextureImporter** and **TextureProcessor** (same settings as existing tiles/sprites).
5. **Rebuild** the project so the new `.xnb` is generated.

If a texture is missing, the game falls back to built-in tiles or placeholder sprites where supported.

---

## 6. Quick links

- **OpenGameArt – 2D Art:** https://opengameart.org/art-search-advanced?field_art_type_tid%5B%5D=9  
- **itch.io – CC0 game assets:** https://itch.io/game-assets/assets-cc0  
- **Kenney assets:** https://kenney.nl/assets  
- **Town tiles (CC0):** https://opengameart.org/content/rpg-town-pixel-art-assets  
- **Dungeon tiles (CC0):** https://opengameart.org/content/dungeon-tileset  
- **World map tiles (CC-BY):** https://opengameart.org/content/worldmapoverworld-tileset  
- **16×16 JRPG characters:** https://opengameart.org/content/top-down-2d-jrpg-16x16-characters-art-collection  

Using these galleries and paths, you can replace or add art without changing code; the game already loads by path and uses fallbacks when files are missing.
