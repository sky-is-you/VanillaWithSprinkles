using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Monocle;
using MonoMod.ModInterop;
using Celeste.Mod.SkysOverworldCore.SkyOverworld;
using On.MonoMod;

namespace Celeste.Mod.SkysOverworldCore;

public class SkysOverworldCoreModule : EverestModule {
    public static SkysOverworldCoreModule Instance { get; private set; }

    public override Type SettingsType => typeof(SkysOverworldCoreModuleSettings);
    public static SpriteBank UISprites;
    public static bool AssetsLoaded=false;
    public static SkysOverworldCoreModuleSettings Settings => (SkysOverworldCoreModuleSettings) Instance._Settings;

//    public override Type SessionType => typeof(SkysOverworldCoreModuleSession);
//    public static SkysOverworldCoreModuleSession Session => (SkysOverworldCoreModuleSession) Instance._Session;

//    public override Type SaveDataType => typeof(SkysOverworldCoreModuleSaveData);
//    public static SkysOverworldCoreModuleSaveData SaveData => (SkysOverworldCoreModuleSaveData) Instance._SaveData;

    public SkysOverworldCoreModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(SkysOverworldCoreModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(SkysOverworldCoreModule), LogLevel.Info);
#endif
    }
    public void LoadAssets()
    {
        UISprites = new(GFX.Gui,"Graphics/SkysOverworldCoreXmls/Overworld.xml");
        AssetsLoaded = true;
    }
    private SkyOverworldLoader GetNewCustomOverworld(Overworld.StartMode startMode)
    {
        HiresSnow snow = null;
        if (Engine.Scene.GetType().IsAssignableTo(typeof(GameLoader)))
            snow = ((GameLoader)Engine.Scene).Snow;
        if (Engine.Scene.GetType().IsAssignableTo(typeof(OverworldLoader)))
            snow = ((OverworldLoader)Engine.Scene).Snow;
        return new SkyOverworldLoader(startMode,snow); // TODO some way to get startmode
    }

    private void LoadCustomOverworld() => LoadCustomOverworld(Overworld.StartMode.Titlescreen);
    private void LoadCustomOverworld(Overworld.StartMode startMode)
    {
        HiresSnow snow = null;
        if (Engine.Scene.GetType().IsAssignableTo(typeof(GameLoader)))
            snow = ((GameLoader)Engine.Scene).Snow;
        if (Engine.Scene.GetType().IsAssignableTo(typeof(OverworldLoader)))
            snow = ((OverworldLoader)Engine.Scene).Snow;
        Engine.Scene = GetNewCustomOverworld(startMode); // TODO some way to get startmode
    }

    // TODO: fix black screen in between takeover
    private void TakeoverVanillaOverworld(Overworld overworld)
    {
        Logger.Info("SkysOverworldCore","Taking over");
        overworld.Remove(overworld.Mountain); // avoid weird crash TODO: what is this
        overworld.Current.PreUpdate += entity => LoadCustomOverworld(Overworld.StartMode.MainMenu);
    }
    
    public override void Load()
    {
        typeof(OverworldHelperImports).ModInterop();
        if (Settings.Enabled)
        {
            // takeover overworld whenever it loads
            OverworldHelperImports.VanillaOverworldCreated += TakeoverVanillaOverworld;
            // start into our overworld instead of vanilla when game loads
            Everest.Events.GameLoader.OnLoadThread += LoadCustomOverworld;
        }
    }
    
    

    public override void Unload() {
        if (Settings.Enabled) OverworldHelperImports.VanillaOverworldCreated -= TakeoverVanillaOverworld;
    }
}