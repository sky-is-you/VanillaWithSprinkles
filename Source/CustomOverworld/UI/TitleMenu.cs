using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.OverworldWithSprinkles.CustomOverworld.UI;

public class TitleMenu : BaseMenu
{
    public override void Render()
    {
        base.Render();
        ActiveFont.Draw("Title", Vector2.Zero,Color.White);
    }
}