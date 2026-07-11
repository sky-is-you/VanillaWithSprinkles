using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.SkysOverworldCore.SkyOverworld;

public class SkyOverworld : Overworld
{
    public SkyOverworld(OverworldLoader loader) : base(loader)
    {
        Logger.Info("SkysOverworldCore","SkysOverworld Constructor Called");
        Add(Mountain = new SkyMountainRenderer(this));
        Add(new HudRenderer());
        Add(routineEntity = new Entity());
        Add(new InputEntity(this));
        Add(Snow = loader.Snow ?? new HiresSnow());
        Add(Snow3D = new Snow3D(Mountain.Model));
        Add(new MoonParticle3D(Mountain.Model, new Vector3(0, 31, 0)));
        Add(Maddy = new Maddy3D(Mountain));
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