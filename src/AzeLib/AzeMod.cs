using AzeLib.Attributes;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AzeLib
{
    public static class AzeMod
    {
        public static UserMod2 UserMod { get; set; }

        private static IReadOnlyList<OnLoadMethod>? cachedOnLoadMethods;

        private sealed class OnLoadMethod
        {
            public OnLoadMethod(MethodInfo method, bool requiresHarmony)
            {
                Method = method;
                RequiresHarmony = requiresHarmony;
            }

            public MethodInfo Method { get; }

            public bool RequiresHarmony { get; }
        }

        private static (IReadOnlyList<OnLoadMethod> Methods, bool UsedCache, TimeSpan DiscoveryDuration) GetOnLoadMethods(Assembly assembly)
        {
            var stopwatch = Stopwatch.StartNew();

            if (cachedOnLoadMethods is { } cached)
            {
                stopwatch.Stop();
                return (cached, true, stopwatch.Elapsed);
            }

            var methods = assembly.GetTypes()
                .SelectMany(x => x.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                .Where(x => x.GetCustomAttribute<OnLoadAttribute>() != null)
                .Select(method => new OnLoadMethod(method, method.GetParameters().Length == 1))
                .ToList();

            cachedOnLoadMethods = methods;

            stopwatch.Stop();
            return (methods, false, stopwatch.Elapsed);
        }

        internal readonly struct OnLoadDiscoverySample
        {
            public OnLoadDiscoverySample(int methodCount, TimeSpan discoveryDuration, bool usedCache)
            {
                MethodCount = methodCount;
                DiscoveryDuration = discoveryDuration;
                UsedCache = usedCache;
            }

            public int MethodCount { get; }

            public TimeSpan DiscoveryDuration { get; }

            public bool UsedCache { get; }
        }

        internal static OnLoadDiscoverySample SampleOnLoadDiscovery(bool resetCache)
        {
            if (resetCache)
                cachedOnLoadMethods = null;

            var (methods, usedCache, discoveryDuration) = GetOnLoadMethods(typeof(AzeMod).Assembly);

            return new OnLoadDiscoverySample(methods.Count, discoveryDuration, usedCache);
        }

        // Only one class per assembly may inherit from UserMod2 or the game will crash.
        // The game can't find a private constructor, do not instantiate this manually.
        private sealed class AzeUserMod : UserMod2
        {
            public override void OnLoad(Harmony harmony)
            {
                UserMod = this;
                Debug.Log("    - version: " + UserMod.assembly.GetName().Version);
#if DEBUG
                Debug.Log("[AzeLib] OnLoad diagnostics enabled.");
                var totalStopwatch = Stopwatch.StartNew();
#endif
                PUtil.InitLibrary(false);

                var (onLoadMethods, usedCache, discoveryDuration) = GetOnLoadMethods(assembly);

#if DEBUG
                Debug.Log($"[AzeLib] Discovered {onLoadMethods.Count} OnLoad methods {(usedCache ? "from cache" : "via reflection")} in {discoveryDuration.TotalMilliseconds:F2} ms.");
#endif

                var invocationStopwatch = Stopwatch.StartNew();

                object[]? harmonyArgs = null;
                foreach (var method in onLoadMethods)
                {
                    if (method.RequiresHarmony)
                    {
                        harmonyArgs ??= new object[] { harmony };
                        method.Method.Invoke(null, harmonyArgs);
                    }
                    else
                    {
                        method.Method.Invoke(null, null);
                    }
                }

                invocationStopwatch.Stop();

#if DEBUG
                totalStopwatch.Stop();
                Debug.Log($"[AzeLib] Invoked {onLoadMethods.Count} OnLoad methods in {invocationStopwatch.Elapsed.TotalMilliseconds:F2} ms (total {totalStopwatch.Elapsed.TotalMilliseconds:F2} ms).");
#endif

                base.OnLoad(harmony);
            }
        }
    }
}
