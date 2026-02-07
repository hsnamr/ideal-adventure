using System;
using SharpSixteen.Core.Inputs;
using SharpSixteen.ScreenManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SharpSixteen.Screens;

/// <summary>
/// Simple SNES-style turn-based battle: Attack, Defend, Run.
/// One enemy; win returns to world, lose returns to main menu (or last save).
/// </summary>
public class BattleScreen : GameScreen
{
    private SpriteFont _font;
    private Texture2D _panel;
    private ContentManager _content;
    private readonly Action<ScreenManager, bool> _onBattleEnd; // (manager, victory)
    private int _menuIndex;
    private static readonly string[] MenuItems = { "Attack", "Defend", "Run" };
    private BattleState _state = BattleState.PlayerTurn;
    private float _stateTimer;
    private string _message = "Enemy approaches!";

    private int _playerHp = 30;
    private int _playerMaxHp = 30;
    private int _enemyHp = 15;
    private int _enemyMaxHp = 15;
    private const int PlayerAttack = 5;
    private const int EnemyAttack = 3;
    private const int DefendReduction = 2;
    private bool _playerDefending;

    public BattleScreen(Action<ScreenManager, bool> onBattleEnd)
    {
        _onBattleEnd = onBattleEnd ?? ((s, b) => { });
        TransitionOnTime = TimeSpan.FromMilliseconds(400);
        TransitionOffTime = TimeSpan.FromMilliseconds(400);
    }

    public override void LoadContent()
    {
        base.LoadContent();
        _content = new ContentManager(ScreenManager.Game.Services, "Content");
        _font = _content.Load<SpriteFont>("Fonts/Hud");
        try { _panel = _content.Load<Texture2D>("Sprites/blank"); } catch { }
    }

    public override void UnloadContent()
    {
        _content.Unload();
    }

    public override void HandleInput(GameTime gameTime, InputState inputState)
    {
        if (_state != BattleState.PlayerTurn) return;

        if (inputState.IsMenuUp(ControllingPlayer))
        {
            _menuIndex = (_menuIndex - 1 + MenuItems.Length) % MenuItems.Length;
        }
        else if (inputState.IsMenuDown(ControllingPlayer))
        {
            _menuIndex = (_menuIndex + 1) % MenuItems.Length;
        }
        else if (inputState.IsMenuSelect(ControllingPlayer, out _))
        {
            ExecuteCommand();
        }
    }

    private void ExecuteCommand()
    {
        switch (MenuItems[_menuIndex])
        {
            case "Attack":
                int damage = PlayerAttack;
                _enemyHp = Math.Max(0, _enemyHp - damage);
                _message = $"Dealt {damage} damage!";
                if (_enemyHp <= 0)
                {
                    _state = BattleState.Victory;
                    _stateTimer = 1.2f;
                    _onBattleEnd(ScreenManager, true);
                }
                else
                {
                    _state = BattleState.EnemyTurn;
                    _stateTimer = 1.2f;
                }
                break;
            case "Defend":
                _playerDefending = true;
                _message = "Defending...";
                _state = BattleState.EnemyTurn;
                _stateTimer = 1.2f;
                break;
            case "Run":
                _message = "Fled!";
                _state = BattleState.Victory; // treat run as success
                _stateTimer = 0.8f;
                _onBattleEnd(ScreenManager, true);
                break;
        }
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
        base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        if (otherScreenHasFocus || coveredByOtherScreen) return;

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_state == BattleState.EnemyTurn)
        {
            _stateTimer -= dt;
            if (_stateTimer <= 0)
            {
                int damage = Math.Max(1, EnemyAttack - (_playerDefending ? DefendReduction : 0));
                _playerHp = Math.Max(0, _playerHp - damage);
                _playerDefending = false;
                _message = $"Enemy hit for {damage}!";
                if (_playerHp <= 0)
                {
                    _state = BattleState.Defeat;
                    _stateTimer = 1.5f;
                    _onBattleEnd(ScreenManager, false);
                }
                else
                {
                    _state = BattleState.PlayerTurn;
                }
            }
        }
        else if (_state == BattleState.Victory || _state == BattleState.Defeat)
        {
            _stateTimer -= dt;
            if (_stateTimer <= 0)
                ExitScreen();
        }
    }

    public override void Draw(GameTime gameTime)
    {
        var batch = ScreenManager.SpriteBatch;
        var viewSize = ScreenManager.BaseScreenSize;

        batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, ScreenManager.GlobalTransformation);

        // Dark background
        if (_panel != null)
            batch.Draw(_panel, new Rectangle(0, 0, (int)viewSize.X, (int)viewSize.Y), Color.DarkSlateGray * 0.95f);

        // Battle panel (bottom)
        int panelH = (int)viewSize.Y / 3;
        int panelY = (int)viewSize.Y - panelH;
        if (_panel != null)
            batch.Draw(_panel, new Rectangle(0, panelY, (int)viewSize.X, panelH), new Color(20, 20, 40, 230));

        // Text: HP and message
        _font ??= ScreenManager.Font;
        batch.DrawString(_font, $"HP: {_playerHp}/{_playerMaxHp}", new Vector2(20, 20), Color.LightGreen);
        batch.DrawString(_font, $"Enemy HP: {_enemyHp}/{_enemyMaxHp}", new Vector2(20, 42), Color.Orange);
        batch.DrawString(_font, _message, new Vector2(20, panelY + 20), Color.White);

        if (_state == BattleState.PlayerTurn)
        {
            for (int i = 0; i < MenuItems.Length; i++)
            {
                var pos = new Vector2(20, panelY + 60 + i * 28);
                var color = i == _menuIndex ? Color.Gold : Color.White;
                batch.DrawString(_font, (i == _menuIndex ? "> " : "  ") + MenuItems[i], pos, color);
            }
        }

        if (_state == BattleState.Defeat)
            batch.DrawString(_font, "You were defeated...", new Vector2(viewSize.X / 2 - 80, viewSize.Y / 2 - 20), Color.Red);
        else if (_state == BattleState.Victory && _enemyHp <= 0)
            batch.DrawString(_font, "Victory!", new Vector2(viewSize.X / 2 - 40, viewSize.Y / 2 - 20), Color.Gold);

        batch.End();
        base.Draw(gameTime);
    }

    private enum BattleState
    {
        PlayerTurn,
        EnemyTurn,
        Victory,
        Defeat
    }
}
