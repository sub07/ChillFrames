﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using ChillFramesModified.Classes;
using Dalamud.Plugin.Services;

namespace ChillFramesModified.Controllers;

public enum LimiterState {
    Enabled,
    Disabled,
    SteadyState
}

public class FrameLimiterController : IDisposable {
    private readonly Stopwatch steppingStopwatch = Stopwatch.StartNew();
    private readonly Stopwatch timer = Stopwatch.StartNew();
    private float delayRatio = 1.0f;
    private bool enabledLastFrame;

    private LimiterState state;

    private static LimiterSettings Settings => System.Config.Limiter;

    private static int TargetIdleFramerate => Settings.IdleFramerateTarget;
    private static int TargetIdleFrametime => 1000 / TargetIdleFramerate;
    private static int PreciseIdleFrametime => (int) (1000.0f / TargetIdleFramerate * 10000);

    private static int TargetActiveFramerate => Settings.ActiveFramerateTarget;
    private static int TargetActiveFrametime => 1000 / TargetActiveFramerate;
    private static int PreciseActiveFrametime => (int) (1000.0f / TargetActiveFramerate * 10000);

    private static float DisableIncrement => System.Config.DisableIncrementSetting;
    private static float EnableIncrement => System.Config.EnableIncrementSetting;

    public static TimeSpan LastFrametime;
    
    public FrameLimiterController() {
        Service.Framework.Update += OnFrameworkUpdate;
    }

    public void Dispose() {
        Service.Framework.Update -= OnFrameworkUpdate;
    }

    private void OnFrameworkUpdate(IFramework framework) {
        UpdateState();

        UpdateRate();

        TryLimitFramerate();

        LastFrametime = timer.Elapsed;
        timer.Restart();

        if (System.Config.General.EnableDtrBar) {
            System.DtrController.Update();
        }
    }

    [MethodImpl(MethodImplOptions.NoOptimization)]
    private void TryLimitFramerate() {
        if (!System.Config.PluginEnable) return;
        if (FrameLimiterCondition.IsBlacklisted) return;
        if (System.BlockList.Count > 0) return;

        if (!FrameLimiterCondition.DisableFramerateLimit() || state != LimiterState.SteadyState) {
            PerformLimiting(TargetIdleFrametime, PreciseIdleFrametime);
        }
        else if (FrameLimiterCondition.DisableFramerateLimit() || state != LimiterState.SteadyState) {
            PerformLimiting(TargetActiveFrametime, PreciseActiveFrametime);
        }
    }

    private void PerformLimiting(int targetFrametime, int preciseFrameTickTime) {
        var delayTime = (int) (targetFrametime - timer.ElapsedMilliseconds);

        if (delayTime - 1 > 0) {
            Thread.Sleep(delayTime - 1);
        }

        while (timer.ElapsedTicks <= preciseFrameTickTime) {
            ((Action) (() => { }))();
        }
    }

    private void UpdateState() {
        var shouldLimit = !FrameLimiterCondition.DisableFramerateLimit();

        if (enabledLastFrame != shouldLimit) {
            state = enabledLastFrame switch {
                true => LimiterState.Disabled,
                false => LimiterState.Enabled
            };
        }

        enabledLastFrame = shouldLimit;
    }

    private void UpdateRate() {
        const int stepDelay = 40;

        if (steppingStopwatch.ElapsedMilliseconds > stepDelay) {
            switch (state) {
                case LimiterState.Enabled when delayRatio < 1.0f:
                    delayRatio += EnableIncrement;
                    break;

                case LimiterState.Enabled when delayRatio >= 1.0f:
                    state = LimiterState.SteadyState;
                    delayRatio = 1.0f;
                    break;

                case LimiterState.Disabled when delayRatio > 0.0f:
                    delayRatio -= DisableIncrement;
                    break;

                case LimiterState.Disabled when delayRatio <= 0.0f:
                    delayRatio = 0.0f;
                    state = LimiterState.SteadyState;
                    break;

                case LimiterState.SteadyState:
                    break;
            }

            steppingStopwatch.Restart();
        }
    }
}