using System;
using SharpSixteen.Core.Inputs;
using SharpSixteen.ScreenManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SharpSixteen.Screens;

/// <summary>
/// Simple status screen showing HP and placeholder stats. Back with Esc or Select.
/// </summary>
class StatusScreen : GameScreen
{
    private SpriteFont _font;
    private ContentManager _content;

    public StatusScreen()
    {
        TransitionOnTime = TimeSpan.FromMilliseconds(200);
        TransitionOffTime = TimeSpan.FromMilliseconds(200);
    }

    public override void LoadContent()
    {
        base.LoadContent();
        _content = new ContentManager(ScreenManager.Game.Services, "Content");
        _font = _content.Load<SpriteFont>("Fonts/Hud");
    }

    public override void UnloadContent()
    {
        _content?.Unload();
    }

    public override void HandleInput(GameTime gameTime, InputState inputState)
    {
        if (inputState.IsMenuCancel(ControllingPlayer, out _) || inputState.IsMenuSelect(ControllingPlayer, out _))
            ExitScreen();
    }

    public override void Draw(GameTime gameTime)
    {
        var batch = ScreenManager.SpriteBatch;
        var viewSize = ScreenManager.BaseScreenSize;

        batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, ScreenManager.GlobalTransformation);

        float y = 80;
        batch.DrawString(_font, "Status", new Vector2(viewSize.X / 2 - 30, y), Color.Gold);
        y += 50;
        batch.DrawString(_font, "HP: 30 / 30", new Vector2(40, y), Color.LightGreen);
        y += 28;
        batch.DrawString(_font, "No other stats yet.", new Vector2(40, y), Color.White);
        y += 60;
        batch.DrawString(_font, "Press Esc or Enter to close", new Vector2(viewSize.X / 2 - 120, y), Color.Gray);

        batch.End();
        base.Draw(gameTime);
    }
}
