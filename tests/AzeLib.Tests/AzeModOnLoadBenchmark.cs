using Xunit;
using Xunit.Abstractions;

namespace AzeLib.Tests;

public sealed class AzeModOnLoadBenchmark
{
    private readonly ITestOutputHelper output;

    public AzeModOnLoadBenchmark(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void DiscoverOnLoadMethodsIsCached()
    {
        var fresh = AzeLib.AzeMod.SampleOnLoadDiscovery(resetCache: true);
        var cached = AzeLib.AzeMod.SampleOnLoadDiscovery(resetCache: false);

        output.WriteLine($"Initial discovery: {fresh.MethodCount} methods in {fresh.DiscoveryDuration.TotalMilliseconds:F3} ms (cache used: {fresh.UsedCache}).");
        output.WriteLine($"Cached discovery: {cached.MethodCount} methods in {cached.DiscoveryDuration.TotalMilliseconds:F3} ms (cache used: {cached.UsedCache}).");

        Assert.False(fresh.UsedCache);
        Assert.True(cached.UsedCache);
        Assert.Equal(fresh.MethodCount, cached.MethodCount);
    }
}
