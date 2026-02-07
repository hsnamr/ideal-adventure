using System;
using SharpSixteen.Core.Inputs;
using SharpSixteen.Core.Jrpg;
using SharpSixteen.Core.Jrpg.MapData;
using SharpSixteen.ScreenManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SharpSixteen.Screens;

/// <summary>
/// One overworld map (town or cave). Top-down 16-bit style, 4-direction movement,
/// doors trigger scene transition, encounter tiles can start battles.
/// </summary>
public class WorldScreen : GameScreen
{
    private JrpgMap _map = null!;
    private MapId _currentMapId;
    private readonly int _spawnIndex;
    private Vector2 _playerPixel;      // position in pixels
    private Point _playerTile;        // current tile for doors/collision
    private Texture2D _playerTexture;
    private Texture2D _npcTexture;
    private ContentManager _content = null!;
    private float _moveCooldown;       // grid-snap movement delay
    private const float MoveDelay = 0.15f;
    private readonly Random _random = new();
    private const int EncounterChancePercent = 8; // per step on E tile

    public WorldScreen(MapId mapId, int spawnIndex = 0)
    {
        _currentMapId = mapId;
        _spawnIndex = spawnIndex;
        TransitionOnTime = TimeSpan.FromMilliseconds(350);
        TransitionOffTime = TimeSpan.FromMilliseconds(350);
        _playerTile = new Point(1, 1);
        _playerPixel = new Vector2(1 * JrpgTile.Width, 1 * JrpgTile.Height);
    }

    public override void LoadContent()
    {
        base.LoadContent();
        _content = new ContentManager(ScreenManager.Game.Services, "Content");
        LoadMap(_currentMapId);
        var spawn = _map.GetSpawn(_spawnIndex);
        _playerTile = spawn;
        _playerPixel = new Vector2(spawn.X * JrpgTile.Width, spawn.Y * JrpgTile.Height);
        try
        {
            _playerTexture = _content.Load<Texture2D>("Sprites/Jrpg/Hero");
        }
        catch
        {
            try { _playerTexture = _content.Load<Texture2D>("Sprites/blank"); } catch { _playerTexture = null; }
        }
        try
        {
            _npcTexture = _content.Load<Texture2D>("Sprites/Jrpg/Npc");
        }
        catch
        {
            _npcTexture = null;
        }
    }

    private void LoadMap(MapId id)
    {
        _currentMapId = id;
        using var stream = MapData.GetMapStream(id);
        _map = new JrpgMap(_content, id, stream);
    }

    public override void UnloadContent()
    {
        _content.Unload();
    }

    public override void HandleInput(GameTime gameTime, InputState inputState)
    {
        if (inputState.IsMenuCancel(ControllingPlayer, out _))
        {
            ScreenManager.AddScreen(new InGameMenuScreen(), ControllingPlayer);
            return;
        }

        if (_moveCooldown > 0) return;

        int dx = 0, dy = 0;
        if (inputState.CurrentKeyboardStates[0].IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up) ||
            inputState.CurrentGamePadStates[0].DPad.Up == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            dy = -1;
        if (inputState.CurrentKeyboardStates[0].IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down) ||
            inputState.CurrentGamePadStates[0].DPad.Down == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            dy = 1;
        if (inputState.CurrentKeyboardStates[0].IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left) ||
            inputState.CurrentGamePadStates[0].DPad.Left == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            dx = -1;
        if (inputState.CurrentKeyboardStates[0].IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right) ||
            inputState.CurrentGamePadStates[0].DPad.Right == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            dx = 1;

        if (dx == 0 && dy == 0) return;

        int nx = _playerTile.X + dx;
        int ny = _playerTile.Y + dy;
        if (!_map.CanWalk(nx, ny)) return;

        _playerTile = new Point(nx, ny);
        _playerPixel = new Vector2(nx * JrpgTile.Width, ny * JrpgTile.Height);
        _moveCooldown = MoveDelay;

        // Door: transition to other map
        if (_map.TryGetDoor(nx, ny, out MapId target, out int spawnIndex))
        {
            ScreenManager.AddScreen(new WorldScreen(target, spawnIndex), ControllingPlayer);
            ExitScreen();
            return;
        }

        // Encounter tile: random battle
        if (_map.IsEncounterTile(nx, ny) && _random.Next(100) < EncounterChancePercent)
        {
            var battle = new BattleScreen((manager, won) =>
            {
                if (!won)
                    LoadingScreen.Load(manager, false, null, new MainMenuScreen());
            });
            ScreenManager.AddScreen(battle, ControllingPlayer);
        }
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
        base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        if (_moveCooldown > 0)
            _moveCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public override void Draw(GameTime gameTime)
    {
        var batch = ScreenManager.SpriteBatch;
        var viewSize = ScreenManager.BaseScreenSize;
        Vector2 camera = _playerPixel - viewSize / 2f;
        camera.X = MathHelper.Clamp(camera.X, 0, Math.Max(0, _map.Width * JrpgTile.Width - viewSize.X));
        camera.Y = MathHelper.Clamp(camera.Y, 0, Math.Max(0, _map.Height * JrpgTile.Height - viewSize.Y));

        batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null,
            Matrix.CreateTranslation(-camera.X, -camera.Y, 0) * ScreenManager.GlobalTransformation);

        _map.Draw(batch, camera, viewSize);

        if (_playerTexture != null)
            batch.Draw(_playerTexture,
                new Rectangle((int)_playerPixel.X, (int)_playerPixel.Y, JrpgTile.Width, JrpgTile.Height),
                Color.White);

        if (_npcTexture != null)
        {
            foreach (var npc in _map.NpcPositions)
            {
                batch.Draw(_npcTexture,
                    new Rectangle(npc.X * JrpgTile.Width, npc.Y * JrpgTile.Height, JrpgTile.Width, JrpgTile.Height),
                    Color.White);
            }
        }

        batch.End();
        base.Draw(gameTime);
    }
}
