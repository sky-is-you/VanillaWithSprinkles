using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Monocle;
using MonoMod.ModInterop;
using Celeste.Mod.SkysOverworldCore.SkyOverworld;
using On.MonoMod;

namespace Celeste.Mod.SkysOverworldCore;

public class SkysOverworldCoreModule : EverestModule {
    public static SkysOverworldCoreModule Instance { get; private set; }

    public override Type SettingsType => typeof(SkysOverworldCoreModuleSettings);
    public static SkysOverworldCoreModuleSettings Settings => (SkysOverworldCoreModuleSettings) Instance._Settings;

    public static SpriteBank UISprites;
    public static bool AssetsLoaded=false;

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
        OverworldSwitcherImports.OverworldLoading += RegisterWorld;
    }

    private void RegisterWorld()
    {
        OverworldSwitcherImports.RegisterOverworldScene(typeof(SkyOverworld.SkyOverworld), "Sky's Overworld");
    }

    public override void Load()
    {
        typeof(OverworldHelperImports).ModInterop();
        typeof(OverworldSwitcherImports).ModInterop();
        Everest.Events.GameLoader.OnLoadThread += LoadAssets;
    }

    public override void Unload() {
        Everest.Events.GameLoader.OnLoadThread -= LoadAssets;
    }
}