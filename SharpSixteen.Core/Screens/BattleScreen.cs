using System;
using SharpSixteen.Core;
using SharpSixteen.Core.Database;
using SharpSixteen.Core.Inputs;
using SharpSixteen.ScreenManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

#nullable enable

namespace SharpSixteen.Screens;

/// <summary>
/// Simple SNES-style turn-based battle: Attack, Magic, Defend, Run.
/// One enemy; win returns to world, lose returns to main menu (or last save).
/// </summary>
public class BattleScreen : GameScreen
{
    private SpriteFont _font = null!;
    private Texture2D _panel = null!;
    private Texture2D? _playerSprite;
    private Texture2D? _enemySprite;
    private ContentManager _content = null!;
    private readonly Action<ScreenManager, bool, int, int, int, int> _onBattleEnd; // (manager, victory, hp, maxHp, mp, maxMp)
    private int _menuIndex;
    private static readonly string[] MenuItems = { "Attack", "Magic", "Defend", "Run" };
    private BattleState _state = BattleState.PlayerTurn;
    private float _stateTimer;
    private string _message = "Enemy approaches!";

    private int _playerHp;
    private int _playerMaxHp = 30;
    private int _playerMp;
    private int _playerMaxMp = 20;
    private int _enemyHp;
    private int _enemyMaxHp = 15;
    private int _enemyAttack = 3;
    private string _enemyName = "Enemy";
    private readonly string _enemySpriteId;
    private const int PlayerAttack = 5;
    private const int DefendReduction = 2;
    private bool _playerDefending;

    private bool _inMagicMenu;
    private int _magicIndex;

    public BattleScreen(Action<ScreenManager, bool, int, int, int, int> onBattleEnd, int playerHp = 30, int playerMaxHp = 30, int playerMp = 20, int playerMaxMp = 20, string? enemyId = null)
    {
        _onBattleEnd = onBattleEnd ?? ((s, b, h, mh, mp, mm) => { });
        _playerHp = Math.Clamp(playerHp, 0, playerMaxHp);
        _playerMaxHp = Math.Max(1, playerMaxHp);
        _playerMp = Math.Clamp(playerMp, 0, playerMaxMp);
        _playerMaxMp = Math.Max(0, playerMaxMp);
        var enemy = Enemies.Get(enemyId ?? "Slime") ?? Enemies.All[0];
        _enemyName = enemy.Name;
        _enemyHp = enemy.Hp;
        _enemyMaxHp = enemy.Hp;
        _enemyAttack = Math.Max(1, enemy.Attack);
        _enemySpriteId = enemy.SpriteId;
        TransitionOnTime = TimeSpan.FromMilliseconds(400);
        TransitionOffTime = TimeSpan.FromMilliseconds(400);
    }

    public override void LoadContent()
    {
        base.LoadContent();
        _content = new ContentManager(ScreenManager.Game.Services, "Content");
        _font = _content.Load<SpriteFont>("Fonts/Hud");
        try { _panel = _content.Load<Texture2D>("Sprites/blank"); } catch { }
        _playerSprite = ContentHelper.LoadJrpgSprite(_content, "Hero");
        _enemySprite = ContentHelper.LoadEnemy(_content, _enemySpriteId);
    }

    public override void UnloadContent()
    {
        _content.Unload();
    }

    public override void HandleInput(GameTime gameTime, InputState inputState)
    {
        if (_state != BattleState.PlayerTurn) return;

        if (_inMagicMenu)
        {
            var skills = Skills.All;
            if (inputState.IsMenuUp(ControllingPlayer))
                _magicIndex = (_magicIndex - 1 + skills.Count) % Math.Max(1, skills.Count);
            else if (inputState.IsMenuDown(ControllingPlayer))
                _magicIndex = (_magicIndex + 1) % Math.Max(1, skills.Count);
            else if (inputState.IsMenuSelect(ControllingPlayer, out _))
            {
                if (skills.Count > 0)
                {
                    var skill = skills[_magicIndex];
                    if (_playerMp >= skill.MpCost)
                        ExecuteMagic(skill);
                    else
                        _message = "Not enough MP!";
                    _inMagicMenu = false;
                }
            }
            else if (inputState.IsMenuCancel(ControllingPlayer, out _))
                _inMagicMenu = false;
            return;
        }

        if (inputState.IsMenuUp(ControllingPlayer))
            _menuIndex = (_menuIndex - 1 + MenuItems.Length) % MenuItems.Length;
        else if (inputState.IsMenuDown(ControllingPlayer))
            _menuIndex = (_menuIndex + 1) % MenuItems.Length;
        else if (inputState.IsMenuSelect(ControllingPlayer, out _))
            ExecuteCommand();
    }

    private void ExecuteMagic(SkillRecord skill)
    {
        _playerMp = Math.Max(0, _playerMp - skill.MpCost);
        if (skill.Target == "Self")
        {
            _playerHp = Math.Min(_playerMaxHp, _playerHp + skill.Power);
            _message = $"{skill.Name}! Recovered {skill.Power} HP.";
        }
        else
        {
            _enemyHp = Math.Max(0, _enemyHp - skill.Power);
            _message = $"{skill.Name}! Dealt {skill.Power} damage!";
            if (_enemyHp <= 0)
            {
                _state = BattleState.Victory;
                _stateTimer = 1.2f;
                _onBattleEnd(ScreenManager, true, _playerHp, _playerMaxHp, _playerMp, _playerMaxMp);
                return;
            }
        }
        _state = BattleState.EnemyTurn;
        _stateTimer = 1.2f;
    }

    private void ExecuteCommand()
    {
        if (MenuItems[_menuIndex] == "Magic")
        {
            _inMagicMenu = true;
            _magicIndex = 0;
            return;
        }

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
                    _onBattleEnd(ScreenManager, true, _playerHp, _playerMaxHp, _playerMp, _playerMaxMp);
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
                _onBattleEnd(ScreenManager, true, _playerHp, _playerMaxHp, _playerMp, _playerMaxMp);
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
                int damage = Math.Max(1, _enemyAttack - (_playerDefending ? DefendReduction : 0));
                _playerHp = Math.Max(0, _playerHp - damage);
                _playerDefending = false;
                _message = $"Enemy hit for {damage}!";
                if (_playerHp <= 0)
                {
                    _state = BattleState.Defeat;
                    _stateTimer = 1.5f;
                    _onBattleEnd(ScreenManager, false, 0, _playerMaxHp, _playerMp, _playerMaxMp);
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

        // Sprites: player left, enemy right (scaled 3x for visibility)
        int scale = 3;
        int spriteSize = 16 * scale;
        if (_playerSprite != null)
            batch.Draw(_playerSprite, new Rectangle(40, (int)viewSize.Y / 2 - spriteSize / 2 - 30, spriteSize, spriteSize), Color.White);
        if (_enemySprite != null)
            batch.Draw(_enemySprite, new Rectangle((int)viewSize.X - 40 - spriteSize, (int)viewSize.Y / 2 - spriteSize / 2 - 30, spriteSize, spriteSize), Color.White);

        // Battle panel (bottom)
        int panelH = (int)viewSize.Y / 3;
        int panelY = (int)viewSize.Y - panelH;
        if (_panel != null)
            batch.Draw(_panel, new Rectangle(0, panelY, (int)viewSize.X, panelH), new Color(20, 20, 40, 230));

        // Text: HP, MP and message
        _font ??= ScreenManager.Font;
        batch.DrawString(_font, $"HP: {_playerHp}/{_playerMaxHp}  MP: {_playerMp}/{_playerMaxMp}", new Vector2(20, 20), Color.LightGreen);
        batch.DrawString(_font, $"Enemy HP: {_enemyHp}/{_enemyMaxHp}  ({_enemyName})", new Vector2(20, 42), Color.Orange);
        batch.DrawString(_font, _message, new Vector2(20, panelY + 20), Color.White);

        if (_state == BattleState.PlayerTurn)
        {
            if (_inMagicMenu)
            {
                var skills = Skills.All;
                batch.DrawString(_font, "Magic", new Vector2(20, panelY + 50), Color.Gray);
                for (int i = 0; i < skills.Count; i++)
                {
                    var s = skills[i];
                    var pos = new Vector2(20, panelY + 78 + i * 28);
                    var color = i == _magicIndex ? Color.Gold : Color.White;
                    var canUse = _playerMp >= s.MpCost;
                    if (!canUse) color = Color.Gray;
                    batch.DrawString(_font, (i == _magicIndex ? "> " : "  ") + $"{s.Name} ({s.MpCost} MP)", pos, color);
                }
            }
            else
            {
                for (int i = 0; i < MenuItems.Length; i++)
                {
                    var pos = new Vector2(20, panelY + 60 + i * 28);
                    var color = i == _menuIndex ? Color.Gold : Color.White;
                    batch.DrawString(_font, (i == _menuIndex ? "> " : "  ") + MenuItems[i], pos, color);
                }
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
