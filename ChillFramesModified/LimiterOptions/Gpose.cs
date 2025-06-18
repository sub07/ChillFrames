using ChillFramesModified.Classes;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace ChillFramesModified.LimiterOptions;

public class Gpose : IFrameLimiterOption {
    public string Label => "GPose";
    
    public bool Active => GameMain.IsInGPose();
    
    public ref bool Enabled => ref System.Config.General.DisableDuringGpose;
}