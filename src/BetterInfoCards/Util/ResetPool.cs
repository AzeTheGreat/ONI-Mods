using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace BetterInfoCards
{
    public class ResetPool<T> where T : new()
    {
        private static readonly double StopwatchToTimeSpanRatio = (double)TimeSpan.TicksPerSecond / Stopwatch.Frequency;

        private readonly List<T> pool = new();
        private readonly List<long> lastUsedTicks = new();
        private readonly object sync = new();
        private readonly int lowUsageCyclesBeforeTrim;
        private readonly double lowUsageRatio;
        private readonly double trimSlack;
        private readonly long idleTrimThreshold;

        private int currentlyUsedIndex;
        private int highWatermark;
        private int consecutiveLowUsageCycles;
        private int lastCycleUsage;
        private long lastResetTimestamp;

        public ResetPool(
            ref System.Action resetOn,
            int lowUsageCyclesBeforeTrim = 3,
            double lowUsageRatio = 0.5,
            double trimSlack = 0.25,
            TimeSpan? idleTrimAge = null)
        {
            if (lowUsageCyclesBeforeTrim < 1)
                throw new ArgumentOutOfRangeException(nameof(lowUsageCyclesBeforeTrim));
            if (lowUsageRatio <= 0d || lowUsageRatio > 1d)
                throw new ArgumentOutOfRangeException(nameof(lowUsageRatio));
            if (trimSlack < 0d)
                throw new ArgumentOutOfRangeException(nameof(trimSlack));

            this.lowUsageCyclesBeforeTrim = lowUsageCyclesBeforeTrim;
            this.lowUsageRatio = lowUsageRatio;
            this.trimSlack = trimSlack;
            idleTrimThreshold = ToStopwatchTicks(idleTrimAge ?? TimeSpan.FromSeconds(30));

            System.Action resetHandler = Reset;
            resetOn = (System.Action)Delegate.Combine(resetOn, resetHandler);
        }

        public ResetPool(ref System.Action onBeginDrawing)
        {
            OnBeginDrawing = onBeginDrawing;
        }

        public int Count
        {
            get
            {
                lock (sync)
                {
                    return pool.Count;
                }
            }
        }

        public int CurrentlyUsed => Volatile.Read(ref currentlyUsedIndex);

        public int HighWatermark => Volatile.Read(ref highWatermark);

        public int LastCycleUsage => Volatile.Read(ref lastCycleUsage);

        public System.Action OnBeginDrawing { get; }

        public TimeSpan GetIdleTimeFor(int index)
        {
            lock (sync)
            {
                if ((uint)index >= (uint)pool.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                long now = Stopwatch.GetTimestamp();
                long lastUsed = lastUsedTicks[index];
                return StopwatchTicksToTimeSpan(now - lastUsed);
            }
        }

        public TimeSpan TimeSinceLastReset()
        {
            long snapshot = Interlocked.Read(ref lastResetTimestamp);
            if (snapshot == 0)
                return TimeSpan.MaxValue;

            long now = Stopwatch.GetTimestamp();
            return StopwatchTicksToTimeSpan(now - snapshot);
        }

        public T Get()
        {
            lock (sync)
            {
                int index = currentlyUsedIndex;
                T obj;
                long now = Stopwatch.GetTimestamp();

                if (index < pool.Count)
                {
                    obj = pool[index];
                    lastUsedTicks[index] = now;
                }
                else
                {
                    obj = new T();
                    pool.Add(obj);
                    lastUsedTicks.Add(now);
                }

                currentlyUsedIndex = index + 1;
                return obj;
            }
        }

        private void Reset()
        {
            lock (sync)
            {
                int usedThisCycle = currentlyUsedIndex;
                currentlyUsedIndex = 0;
                lastCycleUsage = usedThisCycle;

                if (usedThisCycle > highWatermark)
                    highWatermark = usedThisCycle;

                long now = Stopwatch.GetTimestamp();
                Interlocked.Exchange(ref lastResetTimestamp, now);

                if (pool.Count == 0)
                {
                    consecutiveLowUsageCycles = 0;
                    return;
                }

                bool lowUsage = usedThisCycle == 0 || usedThisCycle <= Math.Floor(pool.Count * lowUsageRatio);
                consecutiveLowUsageCycles = lowUsage ? consecutiveLowUsageCycles + 1 : 0;

                if (consecutiveLowUsageCycles >= lowUsageCyclesBeforeTrim)
                {
                    TrimPool(now, usedThisCycle);
                    consecutiveLowUsageCycles = 0;
                }
            }
        }

        private void TrimPool(long now, int requiredCount)
        {
            if (pool.Count == 0)
                return;

            int buffer = requiredCount == 0
                ? 0
                : Math.Max(1, (int)Math.Ceiling(requiredCount * trimSlack));
            int target = Math.Min(pool.Count, requiredCount + buffer);

            for (int i = pool.Count - 1; i >= target; i--)
            {
                long idleTicks = now - lastUsedTicks[i];
                if (idleTicks < idleTrimThreshold)
                    continue;

                pool.RemoveAt(i);
                lastUsedTicks.RemoveAt(i);
            }
        }

        private static long ToStopwatchTicks(TimeSpan duration)
        {
            if (duration == Timeout.InfiniteTimeSpan)
                return long.MaxValue;

            double ticks = duration.Ticks * (double)Stopwatch.Frequency / TimeSpan.TicksPerSecond;
            return ticks < 0 ? 0 : (long)ticks;
        }

        private static TimeSpan StopwatchTicksToTimeSpan(long ticks)
        {
            if (ticks <= 0)
                return TimeSpan.Zero;

            long durationTicks = (long)(ticks * StopwatchToTimeSpanRatio);
            return TimeSpan.FromTicks(Math.Max(0, durationTicks));
        }
    }
}
