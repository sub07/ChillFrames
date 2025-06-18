using ChillFramesModified.Classes;
using KamiLib.Extensions;

namespace ChillFramesModified.LimiterOptions;

public class Combat : IFrameLimiterOption {
    public string Label => "Combat";
    
    public bool Active => Service.Condition.IsInCombat();
    
    public ref bool Enabled => ref System.Config.General.DisableDuringCombatSetting;
}