# SharpSixteen

A short 2D 16-bit style JRPG (in the vein of SNES Final Fantasy), built with MonoGame. Targets **Linux** (Desktop GL) and **Android**.

## Quick start

- **Run (Linux):** `dotnet run --project SharpSixteen.DesktopGL/SharpSixteen.DesktopGL.csproj`
- **Build (Android):** `dotnet build SharpSixteen.Android/SharpSixteen.Android.csproj` (requires Android workload)

## Game flow (~1 hour)

1. **Main menu** → Start Game → **Town** (small overworld).
2. **Town** → Move with **arrow keys** or **DPad**. Walk onto the **door** (tile with exit graphic) to enter the **Cave**.
3. **Cave** → Same movement. Some tiles can trigger **random encounters**. Use the door to return to Town.
4. **Battle** (turn-based) → **Attack**, **Defend**, or **Run**. Win to return to the map; lose to return to the main menu.

## Tech

- **Resolution:** 320×240 logical (SNES-like), scaled to window.
- **Maps:** Embedded in code (`SharpSixteen.Core/Jrpg/MapData.cs`). Format: `.` floor, `#` wall, `P` spawn, `1` door to Cave, `2` door to Town, `E` encounter tile.
- **Screens:** `WorldScreen` (town/cave), `BattleScreen` (turn-based), `MainMenuScreen`; scene transitions via existing `ScreenManager`.

## Project layout

- **SharpSixteen.Core** – Shared game logic, JRPG map/battle, content.
- **SharpSixteen.DesktopGL** – Linux/Windows/macOS (OpenGL).
- **SharpSixteen.Android** – Android (no iOS in solution).

## Optional: open assets

For 16×16 JRPG tiles and sprites you can add later (license-compatible):

- [OGA 16x16 JRPG Sprites & Tiles](https://opengameart.org/content/oga-16x16-jrpg-sprites-tiles) (CC0/collaborative).
- [Top Down 2D JRPG 16x16 Art Collection](https://opengameart.org/content/top-down-2d-jrpg-16x16-art-collection).

Current tiles use the template’s existing art (scaled to 16×16) as placeholders.
