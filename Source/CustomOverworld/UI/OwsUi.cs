using System.Collections;
using Monocle;

namespace Celeste.Mod.OverworldWithSprinkles.CustomOverworld.UI;

public abstract class OwsUi : Entity
{
    public OwsOverworld MyOverworld => SceneAs<OwsOverworld>();
    public bool Focused;
    public bool Selected => MyOverworld?.Current == this;
    public virtual bool IsStart(OwsOverworld world, Overworld.StartMode start) => false;
    public abstract IEnumerator Enter(OwsUi from);
    public abstract IEnumerator Leave(OwsUi next);
    public OwsUi()
    {
        AddTag(Tags.HUD);
        Depth = -10000;
    }
}