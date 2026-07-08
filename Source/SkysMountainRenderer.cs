using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Celeste.Mod.SkysOverworldCore;

public class SkysMountainRenderer : MountainRenderer
{
    private SkysOverworld overworld;
    private Easer overlayEaser;
    private Easer vignetteEaser;
    public SkysMountainRenderer(SkysOverworld parentOverworld) : base()
    {
        overworld = parentOverworld;
        overlayEaser = new(.45f, 1f);
        vignetteEaser = new(.2f, .8f);
        OverworldHelperImports.AreaChanged += ChangeOverlayAlphaTarget;
    }

    ~SkysMountainRenderer()
    {
        OverworldHelperImports.AreaChanged -= ChangeOverlayAlphaTarget;
        Dispose();
    }

    private void ChangeOverlayAlphaTarget(AreaKey area)
    {
        SkysMapMeta meta = OverworldHelperImports.ReadConfig<SkysMapMeta>(area);
        if (meta != null && meta.SkysOverworldCore != null)
        {
            overlayEaser.Target = meta.SkysOverworldCore.OverlayOpacity;
            vignetteEaser.Target = meta.SkysOverworldCore.ShowVignette ? .2f : 0f;
        }
        else
        {
            overlayEaser.Target = 0.45f;
            vignetteEaser.Target = .2f;
        }
    }

    public override void Render(Scene scene)
    {
//        base.Render(scene);
        Model.Render();
        overworld.Snow.overlayAlpha = overlayEaser.Ease;
        Draw.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
        OVR.Atlas["vignette"].Draw(Vector2.Zero,Vector2.Zero,new Color(Calc.HexToColor("333333"),vignetteEaser.Ease));
        Draw.SpriteBatch.End();
        overworld.hudRenderer.RenderContent(scene);
        if (inFreeCameraDebugMode)
        {
            Draw.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            ActiveFont.DrawOutline(GetCameraString(), new Vector2(8f, 8f), Vector2.Zero, Vector2.One * 0.75f, Color.Red, 2f, Color.Black);
            SkysOverworldCoreModule.UISprites.Atlas["SkyIsYou/SkysOverworldCore/icon/img"].Draw(new Vector2(8,1072),new Vector2(0,56));
            ActiveFont.DrawOutline("SkysOverworldCore v0.1.0", new Vector2(68, 1044), new Vector2(0, .5f), Vector2.One*0.75f, Color.DeepSkyBlue*.5f,1f,Color.Transparent);
            Draw.SpriteBatch.End();
        }
    }

    public override void Update(Scene scene)
    {
        overlayEaser.Update();
        vignetteEaser.Update();
        base.Update(scene);
    }
}