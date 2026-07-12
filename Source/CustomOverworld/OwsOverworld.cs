using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using Celeste.Mod.OverworldWithSprinkles.CustomOverworld.UI;
using Celeste.Mod.UI;

namespace Celeste.Mod.OverworldWithSprinkles.CustomOverworld;

public class OwsOverworld : Scene
{
    public HiresSnow Snow;
    public TitleMenu Menu;
    public HudRenderer Hud;
    private bool drawWatermark = true;
    public OwsOverworld(OverworldLoader loader)
    {
        Add(new InputHandler());
        Add(Menu = new TitleMenu());
        Add(Hud = new HudRenderer());
        Add(Snow = loader.Snow);
    }

    public override void Render()
    {
        base.Render(); // render all the stuff
        if (drawWatermark)
        {
            Draw.SpriteBatch.Begin(SpriteSortMode.Immediate,BlendState.Additive);
            ActiveFont.Draw("Overworld with Sprinkles", new Vector2(56+16,1080-8-28+16), new Vector2(0,1), Vector2.One*.75f, Calc.HexToColor("16344A"));
            ActiveFont.Draw("v0.2.0 (prerel) - have fun :>", new Vector2(56+16,1080-8-28+16), new Vector2(0,.25f), Vector2.One*.75f/2, Calc.HexToColor("16344A"));
            OverworldWithSprinklesModule.UISprites.Atlas["SkyIsYou/OverworldWithSprinkles/icon/img"].Draw(new Vector2(8,1080-8-28), new Vector2(0,28));
            Draw.SpriteBatch.End();
        }
    }
}