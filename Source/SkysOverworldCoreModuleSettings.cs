namespace Celeste.Mod.SkysOverworldCore;

[SettingName("skysoverworldcore_settings")]
public class SkysOverworldCoreModuleSettings : EverestModuleSettings
{
    [SettingName("skysoverworldcore_settings_enabled")]
    public bool Enabled { get; set; } = true;
}