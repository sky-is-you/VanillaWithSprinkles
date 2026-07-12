using System;
using System.Collections;
using System.Collections.Generic;
using Monocle;
using MonoMod.ModInterop;

namespace Celeste.Mod.VanillaWithSprinkles;

public class OverworldWithSprinklesModule : EverestModule {
    public static OverworldWithSprinklesModule Instance { get; private set; }

    public override Type SettingsType => typeof(OverworldWithSprinklesModuleSettings);
    public static SpriteBank UISprites;
    public static bool AssetsLoaded=false;
    public static OverworldWithSprinklesModuleSettings Settings => (OverworldWithSprinklesModuleSettings) Instance._Settings;

//    public override Type SessionType => typeof(OverworldWithSprinklesModuleSession);
//    public static OverworldWithSprinklesModuleSession Session => (OverworldWithSprinklesModuleSession) Instance._Session;

//    public override Type SaveDataType => typeof(OverworldWithSprinklesModuleSaveData);
//    public static OverworldWithSprinklesModuleSaveData SaveData => (OverworldWithSprinklesModuleSaveData) Instance._SaveData;

    public OverworldWithSprinklesModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(OverworldWithSprinklesModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(SkysOverworldModule), LogLevel.Info);
#endif
    }
    
    public void LoadAssets()
    {
        UISprites = new(GFX.Gui,"Graphics/VanillaWithSprinklesXmls/Overworld.xml");
        AssetsLoaded = true;
        Everest.Events.GameLoader.OnLoadThread -= LoadAssets; // remove callback after fired
    }

    private void RegisterOverworld()
    {
        OverworldSwitcherImports.RegisterOverworldScene(typeof(VWSWorld.VWSOverworld), "Vanilla w/ Sprinkles");
        // todo make mods only need to do registration once on game load
//        OverworldSwitcherImports.OverworldLoading -= RegisterOverworld;
    }
    
    

    public override void Load()
    {
        typeof(OverworldSwitcherImports).ModInterop();
        Everest.Events.GameLoader.OnLoadThread += LoadAssets;
        OverworldSwitcherImports.OverworldLoading += RegisterOverworld;
    }

    public override void Unload() {
    }
}