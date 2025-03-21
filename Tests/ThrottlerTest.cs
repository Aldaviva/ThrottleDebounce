using FluentAssertions;
using System;
using System.Threading.Tasks;
using ThrottleDebounce;
using Xunit;

namespace Tests;

public class ThrottlerTest: BaseTest {

    // public class General: ThrottlerTest {

    [Fact]
    public void ThrottleActionNeitherLeadingNorTrailing() {
        Action thrower = () => Throttler.Throttle(() => { }, WAIT_TIME, leading: false, trailing: false);
        thrower.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ThrottleActionZeroWaitTime() {
        Action thrower = () => Throttler.Throttle(() => { }, TimeSpan.Zero);
        thrower.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ThrottleActionNegativeWaitTime() {
        Action thrower = () => Throttler.Throttle(() => { }, TimeSpan.FromMilliseconds(-100));
        thrower.Should().Throw<ArgumentException>();
    }

    // }

    // Put each test method inside its own class so that xUnit will run them in parallel, which saves time since each test involves some waiting
    // public class ThrottleLeadingAndTrailingClass: ThrottlerTest {

    [Fact]
    public async Task ThrottleLeadingAndTrailing() {
        Func<int> throttled = Throttler.Throttle(() => ++executionCount, WAIT_TIME, leading: true, trailing: true).Invoke;

        int result = throttled();
        result.Should().Be(1);
        executionCount.Should().Be(1);

        result = throttled();
        result.Should().Be(1);
        executionCount.Should().Be(1);

        await Task.Delay(WAIT_TIME * 3);

        executionCount.Should().Be(2);

        result = throttled();
        result.Should().Be(3);
        executionCount.Should().Be(3);
    }

    // }

    // public class ThrottleMakesProgressDuringRepeatedInvocationsClass: ThrottlerTest {

    [Fact]
    public async Task ThrottleMakesProgressDuringRepeatedInvocations() {
        Func<int> throttled = Throttler.Throttle(() => ++executionCount, WAIT_TIME * 2, leading: true, trailing: true).Invoke;

        int result = throttled();
        result.Should().Be(1, "result after 0 seconds");
        executionCount.Should().Be(1, "executionCount after 0 seconds and 1 invocation");
        await Task.Delay(WAIT_TIME);

        executionCount.Should().Be(1, "executionCount after 0.1 seconds and 1 invocation");
        result = throttled();
        result.Should().Be(1, "result after 0.1 seconds");
        executionCount.Should().Be(1, "executionCount after 0.1 seconds and 2 invocation");
        await Task.Delay(WAIT_TIME * 2);

        executionCount.Should().Be(2, "executionCount after 0.3 seconds and 2 invocation");
        result = throttled();
        result.Should().Be(2, "result after 0.3 seconds");
        executionCount.Should().Be(2, "executionCount after 0.3 seconds and 3 invocation");
        await Task.Delay(WAIT_TIME * 2);

        executionCount.Should().Be(3, "executionCount after 0.5 seconds and 3 invocation");
        result = throttled();
        result.Should().Be(3, "result after 0.5 seconds");
        executionCount.Should().Be(3, "executionCount after 0.5 seconds and 4 invocation");
        await Task.Delay(WAIT_TIME * 2);

        executionCount.Should().Be(4, "executionCount after 0.7 seconds and 4 invocation");
        result = throttled();
        result.Should().Be(4, "result after 0.7 seconds");
        executionCount.Should().Be(4, "executionCount after 0.7 seconds and 5 invocation");
    }

    // }

    // public class ThrottleLeadingOnlyClass: ThrottlerTest {

    [Fact]
    public async Task ThrottleLeadingOnly() {
        Func<int> throttled = Throttler.Throttle(() => ++executionCount, WAIT_TIME, leading: true, trailing: false).Invoke;

        int result = throttled();
        result.Should().Be(1);
        executionCount.Should().Be(1);

        result = throttled();
        result.Should().Be(1);
        executionCount.Should().Be(1);

        await Task.Delay(WAIT_TIME * 2);

        executionCount.Should().Be(1);

        result = throttled();
        result.Should().Be(2);
        executionCount.Should().Be(2);
    }

    // }

    // public class ThrottleFuncTrailingOnlyClass: ThrottlerTest {

    [Fact]
    public async Task ThrottleFuncTrailingOnly() {
        Func<int> throttled = Throttler.Throttle(() => ++executionCount, WAIT_TIME, leading: false, trailing: true).Invoke;

        int result = throttled();
        result.Should().Be(default);
        executionCount.Should().Be(0);

        result = throttled();
        result.Should().Be(default);
        executionCount.Should().Be(0);

        await Task.Delay(WAIT_TIME * 2);

        executionCount.Should().Be(1);

        result = throttled();
        result.Should().Be(1);
        executionCount.Should().Be(1);
    }

    // }

    // public class ThrottleFuncLeadingAndTrailingClass: ThrottlerTest {

    [Fact]
    public async Task ThrottleFuncLeadingAndTrailing() {
        Func<int, int> throttled = Throttler.Throttle((int arg) => {
            mostRecentArgument = arg;
            return ++executionCount;
        }, WAIT_TIME, leading: true, trailing: true).Invoke;

        int result = throttled(100);
        result.Should().Be(1);
        executionCount.Should().Be(1);
        mostRecentArgument.Should().Be(100);

        result = throttled(200);
        result.Should().Be(1);
        executionCount.Should().Be(1);
        mostRecentArgument.Should().Be(100);

        await Task.Delay(WAIT_TIME * 3);

        executionCount.Should().Be(2);
        mostRecentArgument.Should().Be(200);

        result = throttled(300);
        result.Should().Be(3);
        executionCount.Should().Be(3);
        mostRecentArgument.Should().Be(300);
    }

    // }

    // public class DisposingPreventsLaterExecutionsClass: ThrottlerTest {

    [Fact]
    public async Task DisposingPreventsLaterExecutions() {
        RateLimitedFunc<int> rateLimited = Throttler.Throttle(() => ++executionCount, WAIT_TIME, leading: true, trailing: true);
        Func<int>            throttled   = rateLimited.Invoke;

        int result = throttled();
        result.Should().Be(1);
        executionCount.Should().Be(1);

        result = throttled();
        result.Should().Be(1);
        executionCount.Should().Be(1);

        rateLimited.Dispose();

        await Task.Delay(WAIT_TIME * 3);

        executionCount.Should().Be(1);
    }

    // }

    // public class DisposingPreventsImmediateExecutionsClass: ThrottlerTest {

    [Fact]
    public void DisposingPreventsImmediateExecutions() {
        RateLimitedAction rateLimited = Throttler.Throttle(() => { ++executionCount; }, WAIT_TIME, leading: true, trailing: true);
        Action            throttled   = rateLimited.Invoke;

        executionCount.Should().Be(0);

        rateLimited.Dispose();

        throttled();
        executionCount.Should().Be(0);
    }

    // }

    // public class AutoGeneratedThrottleOverloadSpecifiesMaxWaitClass: ThrottlerTest {

    [Fact]
    public async Task AutoGeneratedThrottleOverloadSpecifiesMaxWait() {
        Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> throttled = Throttler.Throttle(
            (int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l, int m, int n, int o, int p) => { ++executionCount; },
            WAIT_TIME, leading: true, trailing: true).Invoke;

        // 0.0s
        throttled(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        executionCount.Should().Be(1);
        await Task.Delay(WAIT_TIME / 2);

        // 0.5s
        executionCount.Should().Be(1);
        throttled(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        executionCount.Should().Be(1);
        await Task.Delay(WAIT_TIME);

        // 1.5s
        executionCount.Should().Be(2);
        throttled(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        executionCount.Should().Be(2);
        await Task.Delay(WAIT_TIME);

        // 2.5s
        executionCount.Should().Be(3);
        throttled(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        executionCount.Should().Be(3);
        await Task.Delay(WAIT_TIME);

        // 3.5s
        executionCount.Should().Be(4);
        throttled(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        executionCount.Should().Be(4);
    }

    // }

}