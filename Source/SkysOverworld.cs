using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Celeste.Mod.SkysOverworld;

public class SkysOverworld : Scene
{
    public HiresSnow Snow;
    public SkysOverworld(OverworldLoader loader)
    {
        Add(Snow = loader.Snow);
    }

    public override void Render()
    {
        base.Render();
        Draw.SpriteBatch.Begin(SpriteSortMode.Immediate,BlendState.NonPremultiplied);
        ActiveFont.Draw("Test scene\nHello world\nSky's Overworld v0.2.0 Overhaul", Vector2.Zero, Color.White);
    }
}