using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Celeste.Mod.VanillaWithSprinkles.VWSWorld.UI.Elements;
using Celeste.Pico8;
using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.VanillaWithSprinkles.VWSWorld.UI.Menus;

public class VWSMainMenu : VWSUi
{
    private VWSMainMenuTextButton chapterSelectButton;
    private List<VWSMenuButton> buttons;
    public VWSMainMenu()
    {
        buttons = new();
    }
    
    private void MakeButtons()
    {
        int btnOffset = 0;
        chapterSelectButton = new VWSMainMenuTextButton(
            "menu_debug",
            this,
            new Vector2(960,540),
            new Vector2(960,540),
            OnChapterSelect);
        buttons.Add(chapterSelectButton);
        btnOffset += 60;
        VWSMainMenuTextButton optionsButton = new VWSMainMenuTextButton(
            "menu_options",
            this,
            new Vector2(960,540+btnOffset),
            new Vector2(960,540+btnOffset),
            OnOptions);
        buttons.Add(optionsButton);
        btnOffset += 60;
        VWSMainMenuTextButton pico8Button = new VWSMainMenuTextButton(
            "menu_modoptions",
            this,
            new Vector2(960,540+btnOffset),
            new Vector2(960,540+btnOffset),
            OnModOptions);
        buttons.Add(pico8Button);
        btnOffset += 60;
        VWSMainMenuTextButton pico8Button1 = new VWSMainMenuTextButton(
            "menu_pico8",
            this,
            new Vector2(960,540+btnOffset),
            new Vector2(960,540+btnOffset),
            OnPico8);
        buttons.Add(pico8Button1);
        btnOffset += 60;
        VWSMainMenuTextButton pico8Button2 = new VWSMainMenuTextButton(
            "menu_pico8",
            this,
            new Vector2(960,540+btnOffset),
            new Vector2(960,540+btnOffset),
            OnPico8);
        buttons.Add(pico8Button2);
        btnOffset += 60;
        VWSMainMenuTextButton pico8Button3 = new VWSMainMenuTextButton(
            "menu_pico8",
            this,
            new Vector2(960,540+btnOffset),
            new Vector2(960,540+btnOffset),
            OnPico8);
        buttons.Add(pico8Button3);
        btnOffset += 60;
        VWSMainMenuTextButton pico8Button4 = new VWSMainMenuTextButton(
            "menu_pico8",
            this,
            new Vector2(960,540+btnOffset),
            new Vector2(960,540+btnOffset),
            OnPico8);
        buttons.Add(pico8Button4);
        btnOffset += 60;
        VWSMainMenuTextButton pico8Button5 = new VWSMainMenuTextButton(
            "menu_pico8",
            this,
            new Vector2(960,540+btnOffset),
            new Vector2(960,540+btnOffset),
            OnPico8);
        buttons.Add(pico8Button5);
        btnOffset += 60;
        VWSMainMenuTextButton pico8Button6 = new VWSMainMenuTextButton(
            "menu_pico8",
            this,
            new Vector2(960,540+btnOffset),
            new Vector2(960,540+btnOffset),
            OnPico8);
        buttons.Add(pico8Button6);
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].UpButton = (i > 0) ? buttons[i - 1] : buttons[^1];
            buttons[i].DownButton = (i < buttons.Count-1) ? buttons[i + 1] : buttons[0];
            Scene.Add(buttons[i]);
        }
        Scene.Entities.UpdateLists();
        if (!Visible || !Focused) return;
        foreach (VWSMenuButton btn in buttons) btn.Position = btn.TargetPosition;
    }

    private void OnChapterSelect()
    {
        MyOverworld.Goto<VWSChapterSelect>();
    }
    private void OnOptions()
    {
        MyOverworld.Goto<VWSChapterSelect>();
    }
    private void OnModOptions()
    {
        MyOverworld.Goto<VWSChapterSelect>();
    }
    private void OnPico8()
    {
        Emulator e = new Emulator(Scene);
        Engine.Scene = e;
    }

    public override void Added(Scene scene)
    {
        base.Added(scene);
        MakeButtons();
    }

    public override IEnumerator Enter(VWSUi from)
    {
        VWSMenuButton.ClearSelection(Scene);
        if (from is VWSTitle) chapterSelectButton.StartSelected();
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
        foreach (VWSMenuButton btn in buttons)
            if (btn.Scene == Scene) btn.Update();
        base.Update();
    }
    public override void Render()
    {
        ActiveFont.DrawOutline("Main Menu",new Vector2(960,150),Vector2.One*.5f,Vector2.One*1.5f,Color.White,3f,Color.Lime);
        foreach (VWSMenuButton btn in buttons)
            if (btn.Scene == Scene) btn.Render();
    }
}