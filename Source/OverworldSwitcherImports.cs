using System;
using Celeste.Mod.Meta;
using MonoMod.ModInterop;

namespace Celeste.Mod.SkysOverworld;

[ModImportName("OverworldSwitcher")]
public static class OverworldSwitcherImports
{
    public static Action<Action> SubscribeToOverworldLoading;
    public static Action<Action> UnsubscribeFromOverworldLoading;
    public static Action<Type, string> RegisterOverworldScene;
    public static Action<Type> UnregisterOverworldSceneByType;
    public static Action<string> UnregisterOverworldSceneByName;

    public static event Action OverworldLoading
    {
        add => SubscribeToOverworldLoading(value);
        remove => UnsubscribeFromOverworldLoading(value);
    }
    public static void UnregisterOverworldScene(Type t) => UnregisterOverworldSceneByType(t);
    public static void UnregisterOverworldScene(string n) => UnregisterOverworldSceneByName(n);
}