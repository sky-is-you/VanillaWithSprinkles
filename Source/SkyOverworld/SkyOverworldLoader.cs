using System.Collections;
using Monocle;

namespace Celeste.Mod.SkysOverworldCore.SkyOverworld;

public class SkyOverworldLoader : OverworldLoader
{
    public SkyOverworldLoader(Overworld.StartMode start, HiresSnow snow=null) : base(start, snow)
    {
    }

    public override void Begin()
    {
        Add(Snow);
        RendererList.Update();
        Logger.Info("SkysOverworldCore","SkysOverworldLoader Begin");
        if (!MTN.Loaded) MTN.Load();
        if (!MTN.DataLoaded) MTN.LoadData();
        if (!SkysOverworldCoreModule.AssetsLoaded) SkysOverworldCoreModule.Instance.LoadAssets();
        overworld = new SkyOverworld(this);
        Entity takeoverEntity = new Entity();
        takeoverEntity.Add(new Coroutine(Takeover()));
        Add(takeoverEntity);
        Update();
    }

    private IEnumerator Takeover()
    {
        yield return null;
        Engine.Scene = overworld;
    }
}