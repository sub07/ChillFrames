using ChillFramesModified.Classes;
using KamiLib.Extensions;

namespace ChillFramesModified.LimiterOptions;

public class Cutscene : IFrameLimiterOption {
    public string Label => "Cutscenes";
    
    public bool Active => Service.Condition.IsInCutscene();
    
    public ref bool Enabled => ref System.Config.General.DisableDuringCutsceneSetting;
}