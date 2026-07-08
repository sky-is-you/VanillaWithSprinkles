namespace Celeste.Mod.SkysOverworldCore;

public class SkysOverworld : Overworld
{
    public HudRenderer hudRenderer;
    public SkysOverworld(SkysOverworldLoader loader) : base(loader)
    {
        Logger.Info("SkysOverworldCore","SkysOverworld Constructor Called");
        Maddy.Hide();
        Remove(Maddy);
        Remove(Mountain);
        Mountain.Dispose();
        Add(hudRenderer = new HudRenderer());
        Add(Mountain = new SkysMountainRenderer(this));
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