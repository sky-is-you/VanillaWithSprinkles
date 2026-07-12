using Celeste.Mod.Meta;
using Monocle;

namespace Celeste.Mod.VanillaWithSprinkles.VWSWorld;

public class VWSMountainRenderer : MountainRenderer
{
    public override void Update(Scene scene)
    {
        AreaData areaData = ((-1 < Area && Area < (AreaData.Areas?.Count ?? 0)) ? AreaData.Get(Area) : null);
        MapMeta obj = areaData?.Meta;
        bool flag = inFreeCameraDebugMode;
        if (obj?.Mountain?.ShowCore == true)
        {
            Area = 9;
            orig_Update(scene);
            Area = areaData.ID;
        }
        else
        {
            orig_Update(scene);
        }
        VWSOverworld overworld = scene as VWSOverworld;
        //if (!flag && inFreeCameraDebugMode && ((overworld.Current ?? overworld.Next) is OuiFileNaming { UseKeyboardInput: not false } || (overworld.Current ?? overworld.Next) is OuiModOptionString { UseKeyboardInput: not false }))
        //{
        //    inFreeCameraDebugMode = false;
        //}
    }
}