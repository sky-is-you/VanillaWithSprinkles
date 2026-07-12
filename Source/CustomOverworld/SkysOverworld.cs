using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using Celeste.Mod.SkysOverworld.CustomOverworld.UI;
using Celeste.Mod.UI;

namespace Celeste.Mod.SkysOverworld.CustomOverworld;

public class SkysOverworld : Scene
{
    public HiresSnow Snow;
    public TitleMenu Menu;
    public HudRenderer Hud;
    public SkysOverworld(OverworldLoader loader)
    {
        Add(new InputHandler());
        Add(Menu = new TitleMenu());
        Add(Hud = new HudRenderer());
        Add(Snow = loader.Snow);
    }

    public override void Render()
    {
        base.Render(); // render all the stuff
        Draw.SpriteBatch.Begin(SpriteSortMode.Immediate,BlendState.Additive);
        ActiveFont.Draw("Sky's Overworld v0.2.0 Overhaul", new Vector2(56+16,1080-8-28), new Vector2(0,.5f), Vector2.One*.75f, Calc.HexToColor("16344A"));
        SkysOverworldModule.UISprites.Atlas["SkyIsYou/SkysOverworld/icon/img"].Draw(new Vector2(8,1080-8-28), new Vector2(0,28));
        Draw.SpriteBatch.End();
    }
}