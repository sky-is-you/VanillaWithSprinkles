using System.Collections;
using Monocle;

namespace Celeste.Mod.VanillaWithSprinkles.VWSWorld.UI;

public abstract class VWSUi : Entity
{
    public VWSOverworld MyOverworld => SceneAs<VWSOverworld>();
    public bool Focused;
    public bool Selected => MyOverworld?.Current == this;
    public virtual bool IsStart(VWSOverworld world, Overworld.StartMode start) => false;
    public abstract IEnumerator Enter(VWSUi from);
    public abstract IEnumerator Leave(VWSUi next);
    public VWSUi()
    {
        AddTag(Tags.HUD);
        Depth = -10000;
    }
}