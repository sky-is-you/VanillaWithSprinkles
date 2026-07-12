using System.Collections;
using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.OverworldWithSprinkles.CustomOverworld.UI.Menus;

public class OwsTitle : OwsUi
{
    public OwsTitle()
    {
    }

    public override IEnumerator Enter(OwsUi from)
    {
        MyOverworld.Mountain.EaseCamera(-1, new MountainCamera(new Vector3(0.350f,0.865f,10.556f),new Vector3(0.350f,0.865f,8.556f)),targetRotate:false);
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
        world.Mountain.SnapCamera(-1, new MountainCamera(new Vector3(0.350f,0.865f,10.556f),new Vector3(0.350f,0.865f,8.556f)));
        return start == Overworld.StartMode.Titlescreen;
    }

    public override void Update()
    {
        int gamepadIndex = -1;
        if (Selected && Input.AnyGamepadConfirmPressed(out gamepadIndex))
        {
            Input.Gamepad = gamepadIndex;
            MyOverworld.Goto<OwsMainMenu>();
        }
        base.Update();
    }
    public override void Render()
    {
        ActiveFont.Draw("Overworld with Sprinkles v0.2.0\nConfirm => Main Menu",new Vector2(960,540),Vector2.One*.5f,Vector2.One,Color.White);
    }
}