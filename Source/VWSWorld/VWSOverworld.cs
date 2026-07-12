using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using Celeste.Mod.VanillaWithSprinkles.VWSWorld.UI;
using Celeste.Mod.UI;

namespace Celeste.Mod.VanillaWithSprinkles.VWSWorld;

public class VWSOverworld : Scene
{
    public HiresSnow Snow;
    public VWSMountainRenderer Mountain;
    
    public VWSUi Last;
    public VWSUi Current;
    public VWSUi Next;
    public List<VWSUi> UIs = new List<VWSUi>();
    private bool transitioning;
    private Entity routineEntity;
    
    private bool drawWatermark = true;
    public float inputEase = 1f;
    public VWSOverworld(OverworldLoader loader)
    {
        Add(Mountain = new VWSMountainRenderer());
        Add(new HudRenderer());
        Add(routineEntity = new Entity());
        Add(Snow = loader.Snow ?? new HiresSnow());
        Add(new Snow3D(Mountain.Model));
        RendererList.UpdateLists();
        ReloadMenus(loader.StartMode);
    }

    public void ReloadMenus(Overworld.StartMode startMode)
    {
        UIs.ForEach(Remove);
        UIs.Clear();
        IEnumerable<Type> OwsUiTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => (type.Namespace?.StartsWith("Celeste.Mod.VanillaWithSprinkles.VWSWorld.UI.Menus") ?? false) && type.IsAssignableTo(typeof(VWSUi)));
        foreach (Type OwsUiType in OwsUiTypes)
        {
            VWSUi oui = RegisterUiType(OwsUiType);
            if (oui.IsStart(this, startMode))
            {
                oui.Visible = true;
                Last = (Current = oui);
            }
        }
    }

    public VWSUi RegisterUiType(Type type)
    {
        VWSUi oui = (VWSUi)Activator.CreateInstance(type);
        if (oui == null) return null;
        oui.Visible = false;
        Add(oui);
        UIs.Add(oui);
        return oui;
    }

    public T Goto<T>() where T : VWSUi
    {
        T UI = GetUI<T>();
        if (UI!=null) routineEntity.Add(new Coroutine(GotoRoutine(UI)));
        return UI;
    }

    private IEnumerator GotoRoutine(VWSUi next)
    {
        while (Current == null) yield return null;
        transitioning = true;
        Next = next;
        Last = Current;
        Current = null;
        Last.Focused = false;
        yield return Last.Leave(next);
        if (next.Scene != null)
        {
            yield return next.Enter(Last);
            next.Focused = true;
            Current = next;
            transitioning = false;
        }

        Next = null;
    }

    public T GetUI<T>() where T : VWSUi => (T)UIs.Find(UI => UI is T);

    public override void Render()
    {
        base.Render(); // render all the stuff
        if (drawWatermark)
        {
            Draw.SpriteBatch.Begin(SpriteSortMode.Immediate,BlendState.Additive);
            ActiveFont.Draw("Vanilla with Sprinkles", new Vector2(56+16,1080-8-28+16), new Vector2(0,1), Vector2.One*.75f, Calc.HexToColor("16344A"));
            ActiveFont.Draw("v0.2.0 (prerel) - have fun :>", new Vector2(56+16,1080-8-28+16), new Vector2(0,.25f), Vector2.One*.75f/2, Calc.HexToColor("16344A"));
            OverworldWithSprinklesModule.UISprites.Atlas["SkyIsYou/VanillaWithSprinkles/icon/img"].Draw(new Vector2(8,1080-8-28), new Vector2(0,28));
            Draw.SpriteBatch.End();
        }
    }

    public override void Update()
    {
        inputEase -= Engine.DeltaTime;
        if (inputEase <= 0) inputEase += 1;
        base.Update();
    }
}