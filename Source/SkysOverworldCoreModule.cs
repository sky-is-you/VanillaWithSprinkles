using System;
using System.Collections;
using System.Collections.Generic;
using Monocle;
using MonoMod.ModInterop;

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
        Everest.Events.GameLoader.OnLoadThread -= LoadAssets; // remove callback after fired
    }

    public override void Load()
    {
        Everest.Events.GameLoader.OnLoadThread += LoadAssets;
    }

    public override void Unload() {
    }
}