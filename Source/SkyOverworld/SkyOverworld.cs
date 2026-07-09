using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.SkysOverworldCore.SkyOverworld;

public class SkyOverworld : Overworld
{
    public HudRenderer hudRenderer;
    public SkyOverworld(SkyOverworldLoader loader) : base(loader)
    {
        Logger.Info("SkysOverworldCore","SkysOverworld Constructor Called");
        // purge scene
        base.Entities.UpdateLists();
        // remove entities from base init for which no variable name is assigned
        foreach (Entity e in Entities.Where((entity) =>
                 {
                     return (
                         entity.GetType().IsAssignableTo(typeof(MoonParticle3D)) ||
                         entity.GetType().IsAssignableTo(typeof(HudRenderer)) ||
                         entity.GetType().IsAssignableTo(typeof(InputEntity)));
                 }))
        {
            Logger.Info("SkysOverworldCore",e.GetType().Name);
            Remove(e);
        }
        Remove(Maddy);
        Remove(Snow3D);
        Remove(Snow);
        Remove(Mountain);
        // reinit stuff
        Add(Mountain = new SkyMountainRenderer(this));
        Add(hudRenderer = new HudRenderer());
        Add(new InputEntity(this));
        Add(Snow = loader.Snow ?? new HiresSnow());
        Add(Snow3D = new Snow3D(Mountain.Model));
        Add(new MoonParticle3D(Mountain.Model, new Vector3(0, 31, 0)));
        Add(Maddy = new Maddy3D(Mountain));
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
    }
}