using FluentAssertions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThrottleDebounce;
using Xunit;

namespace Tests;

public class RaceTest {

    [Fact]
    public async Task SingleExecutionForInitialBurst() {
        const int burstSize        = 24;
        int       executions       = 0;
        TimeSpan  throttleDuration = TimeSpan.FromSeconds(0.5);

        RateLimitedAction throttled = Throttler.Throttle(() => { Interlocked.Increment(ref executions); }, throttleDuration, trailing: false);

        Barrier initialBurst = new(burstSize);

        await Task.WhenAll(Enumerable.Range(0, burstSize).Select(_ => Task.Run(() => {
            initialBurst.SignalAndWait();
            throttled.Invoke();
        })));

        await Task.Delay(throttleDuration * 1.5);

        executions.Should().Be(1);
    }

}