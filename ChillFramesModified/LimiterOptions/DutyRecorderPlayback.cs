using ChillFramesModified.Classes;
using KamiLib.Extensions;

namespace ChillFramesModified.LimiterOptions;

public class DutyRecorderPlayback : IFrameLimiterOption {
    public string Label => "Duty Recorder Playback";
    
    public bool Active => Service.Condition.IsDutyRecorderPlayback();
    
    public ref bool Enabled => ref System.Config.General.DisableDuringDutyRecorderPlaybackSetting;
}