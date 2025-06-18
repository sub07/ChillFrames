using ChillFramesModified.Classes;
using KamiLib.Extensions;

namespace ChillFramesModified.LimiterOptions;

public class QuestEvent : IFrameLimiterOption {
    public string Label => "Quest Event";
    
    public bool Active => Service.Condition.IsInQuestEvent();
    
    public ref bool Enabled => ref System.Config.General.DisableDuringQuestEventSetting;
}