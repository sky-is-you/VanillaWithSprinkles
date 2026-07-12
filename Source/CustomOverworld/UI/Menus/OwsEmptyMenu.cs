using System.Collections;
using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.OverworldWithSprinkles.CustomOverworld.UI.Menus;

public class OwsEmptyMenu : OwsUi
{
    public OwsEmptyMenu()
    {
    }

    public override IEnumerator Enter(OwsUi from)
    {
        yield return null;
        Visible = true;
    }

    public override IEnumerator Leave(OwsUi next)
    {
        yield return null;
        Visible = false;
    }

    public override bool IsStart(OwsOverworld world, Overworld.StartMode start)
    {
        return false;
    }
    public override void Render()
    {
        ActiveFont.Draw("gone fishing",new Vector2(960,540),Vector2.One*.5f,Vector2.One,Color.White);
    }
}