using System;
using SharpSixteen.ScreenManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SharpSixteen.Screens;

/// <summary>
/// In-game menu (opened with Esc): Status, Save, Back to Main Title.
/// </summary>
class InGameMenuScreen : MenuScreen
{
    private MenuEntry _statusEntry;
    private MenuEntry _saveEntry;
    private MenuEntry _backToTitleEntry;

    public InGameMenuScreen()
        : base("Menu")
    {
        _statusEntry = new MenuEntry("Status");
        _saveEntry = new MenuEntry("Save");
        _backToTitleEntry = new MenuEntry("Back to Main Title");

        _statusEntry.Selected += StatusEntrySelected;
        _saveEntry.Selected += SaveEntrySelected;
        _backToTitleEntry.Selected += BackToTitleEntrySelected;

        MenuEntries.Add(_statusEntry);
        MenuEntries.Add(_saveEntry);
        MenuEntries.Add(_backToTitleEntry);
    }

    private void StatusEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        ScreenManager.AddScreen(new StatusScreen(), e.PlayerIndex);
    }

    private void SaveEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        var msg = new MessageBoxScreen("Saved!");
        ScreenManager.AddScreen(msg, e.PlayerIndex);
    }

    private void BackToTitleEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        LoadingScreen.Load(ScreenManager, false, null, new MainMenuScreen());
    }
}
