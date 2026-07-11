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

    // TODO: fix black screen in between takeover
    private void TakeoverVanillaOverworld(On.Celeste.OverworldLoader.orig_LoadThread orig, OverworldLoader self)
    {
        Logger.Info("SkysOverworldCore","Taking over");
        if (!MTN.Loaded) MTN.Load();
        if (!MTN.DataLoaded) MTN.LoadData();
        self.CheckVariantsPostcardAtLaunch();
        self.overworld = new SkyOverworld.SkyOverworld(self);
        self.overworld.Entities.UpdateLists();
        self.RendererList.UpdateLists();
        self.loaded = true;
        self.activeThread.Priority = ThreadPriority.Normal;
    }
    
    public override void Load()
    {
        typeof(OverworldHelperImports).ModInterop();
        if (Settings.Enabled)
        {
            Everest.Events.GameLoader.OnLoadThread += LoadAssets;
            // takeover overworld whenever it loads
            SkyOverworld.SkyOverworld.DummifyVanilla();
            On.Celeste.OverworldLoader.LoadThread += TakeoverVanillaOverworld;
        }
    }

    public override void Unload() {
        if (Settings.Enabled) On.Celeste.OverworldLoader.LoadThread -= TakeoverVanillaOverworld;;
    }
}