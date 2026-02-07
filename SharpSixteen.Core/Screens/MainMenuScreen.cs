using System;
using SharpSixteen.Core.Localization;
using SharpSixteen.Core.Settings;
using SharpSixteen.ScreenManagers;
using SharpSixteen.Core.Jrpg;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SharpSixteen.Screens;

/// <summary>
/// Main menu for the JRPG: Start Game, Settings, About, Exit.
/// </summary>
class MainMenuScreen : MenuScreen
{
    private ContentManager _content;
    private SettingsManager<SharpSixteenSettings> _settingsManager;
    private MenuEntry _playMenuEntry;
    private MenuEntry _settingsMenuEntry;
    private MenuEntry _aboutMenuEntry;
    private MenuEntry _exitMenuEntry;
    private Texture2D _gradientTexture;

    public MainMenuScreen()
        : base(Resources.MainMenu)
    {
        _playMenuEntry = new MenuEntry(Resources.Play);
        _settingsMenuEntry = new MenuEntry(Resources.Settings);
        _aboutMenuEntry = new MenuEntry(Resources.About);
        _exitMenuEntry = new MenuEntry(Resources.Exit);

        _playMenuEntry.Selected += PlayMenuEntrySelected;
        _settingsMenuEntry.Selected += SettingsMenuEntrySelected;
        _aboutMenuEntry.Selected += AboutMenuEntrySelected;
        _exitMenuEntry.Selected += OnCancel;

        MenuEntries.Add(_playMenuEntry);
        MenuEntries.Add(_settingsMenuEntry);
        MenuEntries.Add(_aboutMenuEntry);
        MenuEntries.Add(_exitMenuEntry);
    }

    private void SetLanguageText()
    {
        _aboutMenuEntry.Text = Resources.About;
        _playMenuEntry.Text = Resources.Play;
        _settingsMenuEntry.Text = Resources.Settings;
        _exitMenuEntry.Text = Resources.Exit;
        Title = "SharpSixteen";
    }

    public override void LoadContent()
    {
        base.LoadContent();
        _content = new ContentManager(ScreenManager.Game.Services, "Content");
        _settingsManager = ScreenManager.Game.Services.GetService<SettingsManager<SharpSixteenSettings>>();
        if (_settingsManager != null)
            _settingsManager.Settings.PropertyChanged += (s, e) => SetLanguageText();
        SetLanguageText();
        try { _gradientTexture = _content.Load<Texture2D>("Sprites/gradient"); } catch { }
    }

    public override void UnloadContent()
    {
        if (_content != null) _content.Unload();
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }

    void PlayMenuEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        // Start JRPG: go to town.
        ScreenManager.AddScreen(new WorldScreen(MapId.Town), e.PlayerIndex);
    }

    void SettingsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        ScreenManager.AddScreen(new SettingsScreen(), e.PlayerIndex);
    }

    void AboutMenuEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        ScreenManager.AddScreen(new AboutScreen(), e.PlayerIndex);
    }

    protected override void OnCancel(PlayerIndex playerIndex)
    {
        var confirmExit = new MessageBoxScreen(Resources.ExitQuestion);
        confirmExit.Accepted += (s, e) => ScreenManager.Game.Exit();
        ScreenManager.AddScreen(confirmExit, playerIndex);
    }
}
