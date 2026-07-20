using System.Collections;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.VanillaWithSprinkles.VWSWorld.UI.Menus;

public class VWSChapterSelect : VWSUi
{
    public override bool IsStart(VWSOverworld world, Overworld.StartMode start)
    {
        return start is Overworld.StartMode.AreaComplete or Overworld.StartMode.AreaQuit;
    }

    public override IEnumerator Enter(VWSUi from)
    {
        yield return null;
        Visible = true;
    }

    public override IEnumerator Leave(VWSUi next)
    {
        yield return null;
        Visible = false;
    }

    public override void Update()
    {
        if (Focused && Selected && Input.MenuCancel.Pressed) MyOverworld.Goto<VWSMainMenu>();
        base.Update();
    }

    public override void Render()
    {
        ActiveFont.DrawOutline("Chapter Select",new Vector2(960,150),Vector2.One*.5f,Vector2.One*1.5f,Color.White,3f,Color.Lime);
    }
}