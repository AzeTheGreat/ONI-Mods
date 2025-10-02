using System;
using BetterInfoCards;
using Xunit;

namespace BetterInfoCards.Tests
{
    public sealed class ResetPoolTests
    {
        private sealed class Dummy
        {
        }

        [Fact]
        public void TracksHighWatermarkAcrossGrowth()
        {
            Action reset = () => { };
            var pool = new ResetPool<Dummy>(ref reset);

            _ = pool.Get();
            _ = pool.Get();
            reset();

            Assert.Equal(2, pool.HighWatermark);
            Assert.Equal(2, pool.LastCycleUsage);
            Assert.Equal(2, pool.Count);
        }

        [Fact]
        public void ShrinksPoolAfterRepeatedLowUsage()
        {
            Action reset = () => { };
            var pool = new ResetPool<Dummy>(
                ref reset,
                lowUsageCyclesBeforeTrim: 2,
                lowUsageRatio: 0.6,
                trimSlack: 0,
                idleTrimAge: TimeSpan.Zero);

            for (int i = 0; i < 6; i++)
                _ = pool.Get();
            reset();

            // Only rent a single instance for two consecutive cycles.
            _ = pool.Get();
            reset();
            _ = pool.Get();
            reset();

            Assert.Equal(6, pool.HighWatermark);
            Assert.Equal(1, pool.Count);
            Assert.Equal(1, pool.LastCycleUsage);
        }
    }
}
