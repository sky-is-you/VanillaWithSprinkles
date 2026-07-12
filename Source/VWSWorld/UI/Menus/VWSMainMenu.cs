using System.Collections;
using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.VanillaWithSprinkles.VWSWorld.UI.Menus;

public class VWSMainMenu : VWSUi
{
    public VWSMainMenu()
    {
    }

    public override IEnumerator Enter(VWSUi from)
    {
        MyOverworld.Mountain.EaseCamera(0, new MountainCamera(new Vector3(0,25,15),new Vector3(0,5,0)),targetRotate:true);
        yield return null;
        Visible = true;
    }

    public override IEnumerator Leave(VWSUi next)
    {
        yield return null;
        Visible = false;
    }

    public override bool IsStart(VWSOverworld world, Overworld.StartMode start)
    {
        if (start == Overworld.StartMode.MainMenu)
        {
            MyOverworld.Mountain.EaseCamera(0, new MountainCamera(new Vector3(0, 25, 15), new Vector3(0, 5, 0)),
                targetRotate: true);
            return true;
        }

        return false;
    }

    public override void Update()
    {
        if (Focused && Selected && Input.MenuCancel.Pressed) MyOverworld.Goto<VWSTitle>();
        base.Update();
    }
    public override void Render()
    {
        ActiveFont.DrawOutline("Main Menu",new Vector2(960,150),Vector2.One*.5f,Vector2.One*1.5f,Color.White,3f,Color.Lime);
    }
}