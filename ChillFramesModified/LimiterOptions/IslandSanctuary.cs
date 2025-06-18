using ChillFramesModified.Classes;
using FFXIVClientStructs.FFXIV.Client.Game.MJI;

namespace ChillFramesModified.LimiterOptions;

public unsafe class IslandSanctuary : IFrameLimiterOption {
    public string Label => "Island Sanctuary";
    
    public bool Active => MJIManager.Instance() is not null && MJIManager.Instance()->IsPlayerInSanctuary == 1;
    
    public ref bool Enabled => ref System.Config.General.DisableIslandSanctuarySetting;
}