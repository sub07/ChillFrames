using ChillFramesModified.Classes;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace ChillFramesModified.LimiterOptions;

public unsafe class Estate : IFrameLimiterOption {
    public string Label => "Inside Estate";

    public bool Active => HousingManager.Instance()->IsInside();

    public ref bool Enabled => ref System.Config.General.DisableInEstatesSetting;
}