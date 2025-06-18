using ChillFramesModified.Classes;
using KamiLib.Extensions;

namespace ChillFramesModified.LimiterOptions;

public class BoundByDuty : IFrameLimiterOption {
    public string Label => "Duties";
    
    public bool Active => Service.Condition.IsBoundByDuty();
    
    public ref bool Enabled => ref System.Config.General.DisableDuringDutySetting;
}