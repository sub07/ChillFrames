using ChillFramesModified.Classes;
using KamiLib.Extensions;

namespace ChillFramesModified.LimiterOptions;

public class Crafting : IFrameLimiterOption {
    public string Label => "Crafting";
    
    public bool Active => Service.Condition.IsCrafting();
    
    public ref bool Enabled => ref System.Config.General.DisableDuringCraftingSetting;
}