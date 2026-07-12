using System;
using System.Collections;
using System.Collections.Generic;
using Monocle;
using MonoMod.ModInterop;

namespace Celeste.Mod.SkysOverworldCore;

public class SkysOverworldModule : EverestModule {
    public static SkysOverworldModule Instance { get; private set; }

    public override Type SettingsType => typeof(SkysOverworldModuleSettings);
    public static SpriteBank UISprites;
    public static bool AssetsLoaded=false;
    public static SkysOverworldModuleSettings Settings => (SkysOverworldModuleSettings) Instance._Settings;

//    public override Type SessionType => typeof(SkysOverworldCoreModuleSession);
//    public static SkysOverworldCoreModuleSession Session => (SkysOverworldCoreModuleSession) Instance._Session;

//    public override Type SaveDataType => typeof(SkysOverworldCoreModuleSaveData);
//    public static SkysOverworldCoreModuleSaveData SaveData => (SkysOverworldCoreModuleSaveData) Instance._SaveData;

    public SkysOverworldModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(SkysOverworldModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(SkysOverworldModule), LogLevel.Info);
#endif
    }
    
    public void LoadAssets()
    {
        UISprites = new(GFX.Gui,"Graphics/SkysOverworldCoreXmls/Overworld.xml");
        AssetsLoaded = true;
        Everest.Events.GameLoader.OnLoadThread -= LoadAssets; // remove callback after fired
    }

    public override void Load()
    {
        typeof(OverworldSwitcherImports).ModInterop();
        Everest.Events.GameLoader.OnLoadThread += LoadAssets;
    }

    public override void Unload() {
    }
}