using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.SkysOverworldCore.SkyOverworld;

public class SkyOverworld : Overworld
{
    public void PreConstructor()
    {
        DummifyVanilla();
    }

    public SkyOverworld(OverworldLoader loader) : base(loader)
    {
        Logger.Info("SkysOverworldCore","SkysOverworld Constructor Called");
        Add(Mountain = new SkyMountainRenderer(this));
        Maddy.Renderer = Mountain;
        Snow3D.Model = Mountain.Model;
        foreach (Entity e in Entities)
        {
            if (e.GetType().IsAssignableTo(typeof(MoonParticle3D))) Remove(e);
            if (e.GetType().IsAssignableTo(typeof(HudRenderer))) e.RemoveSelf();
        }

        HudRenderer temp;
        Add(temp = new HudRenderer());
        Add(new MoonParticle3D(Mountain.Model, new Vector3(0, 31, 0)));
        ReloadMenus(loader.StartMode);
        Mountain.OnEaseEnd = () =>
        {
            if (Mountain.Area >= 0 && (!Maddy.Show || lastArea != Mountain.Area))
            {
                Maddy.Running(Mountain.Area < 7);
                Maddy.Wiggler.Start();
            }
            lastArea = Mountain.Area;
        };
        lastArea = Mountain.Area;
        if (Mountain.Area < 0) Maddy.Hide();
        else Maddy.Position = AreaData.Areas[Mountain.Area].MountainCursor;
        Settings.Instance.ApplyVolumes();
    }
    public static void DummifyVanilla()
    {
        On.Celeste.Overworld.ctor += ((orig, self, loader) =>
        {
            Scene dummy = new Scene();
            self.RendererList = dummy.RendererList;
            self.Entities = dummy.Entities;
            self.TagLists = dummy.TagLists;
            orig(self, loader);
            self.RendererList = new RendererList(self);
            self.Entities = new EntityList(self);
            self.TagLists = new TagLists();
        });
    }
}