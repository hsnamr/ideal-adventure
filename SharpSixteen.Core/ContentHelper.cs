using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#nullable enable

namespace SharpSixteen.Core;

/// <summary>
/// Loads content with optional overrides. Use for art from free galleries:
/// place custom sprites in Sprites/Jrpg/Custom/ to override defaults.
/// </summary>
public static class ContentHelper
{
    /// <summary>
    /// Tries to load a texture, optionally from a custom path first.
    /// Tries customPath, then defaultPath; returns null if both fail.
    /// </summary>
    public static Texture2D? LoadTextureWithOverride(ContentManager content, string? customPath, string defaultPath)
    {
        if (!string.IsNullOrEmpty(customPath))
        {
            try
            {
                return content.Load<Texture2D>(customPath);
            }
            catch
            {
                // Fall through to default
            }
        }
        try
        {
            return content.Load<Texture2D>(defaultPath);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>JRPG overworld/battle sprite paths: Custom override first, then default.</summary>
    public static string JrpgCustomPrefix => "Sprites/Jrpg/Custom/";
    public static string JrpgDefaultPrefix => "Sprites/Jrpg/";

    public static Texture2D? LoadJrpgSprite(ContentManager content, string name)
    {
        return LoadTextureWithOverride(content, JrpgCustomPrefix + name, JrpgDefaultPrefix + name);
    }

    /// <summary>Load character sprite: assets/characters/ then Sprites/Jrpg/.</summary>
    public static Texture2D? LoadCharacter(ContentManager content, string name)
    {
        return LoadTextureWithOverride(content, "assets/characters/" + name, JrpgDefaultPrefix + name);
    }

    /// <summary>Load enemy sprite: assets/enemies/ then Sprites/Jrpg/EnemySlime (fallback).</summary>
    public static Texture2D? LoadEnemy(ContentManager content, string spriteId)
    {
        return LoadTextureWithOverride(content, "assets/enemies/" + spriteId, "Sprites/Jrpg/EnemySlime");
    }

    /// <summary>Load portrait: assets/portraits/ only (no default).</summary>
    public static Texture2D? LoadPortrait(ContentManager content, string id)
    {
        try { return content.Load<Texture2D>("assets/portraits/" + id); }
        catch { return null; }
    }
}
