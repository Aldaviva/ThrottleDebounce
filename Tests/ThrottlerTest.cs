namespace Tests;

public class ThrottlerTest: BaseTest {

    [Fact]
    public void ThrottleActionNeitherLeadingNorTrailing() {
        Action thrower = () => Throttler.Throttle(() => {}, WaitTime, leading: false, trailing: false);
        thrower.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ThrottleActionZeroWaitTime() {
        Action thrower = () => Throttler.Throttle(() => {}, TimeSpan.Zero);
        thrower.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ThrottleActionNegativeWaitTime() {
        Action thrower = () => Throttler.Throttle(() => {}, TimeSpan.FromMilliseconds(-100));
        thrower.Should().Throw<ArgumentException>();
    }

    [Fact]
    public async Task ThrottleLeadingAndTrailing() {
        Func<int> throttled = Throttler.Throttle(() => ++ExecutionCount, WaitTime, leading: true, trailing: true).Invoke;

        int result = throttled();
        result.Should().Be(1);
        ExecutionCount.Should().Be(1);

        result = throttled();
        result.Should().Be(1);
        ExecutionCount.Should().Be(1);

        await Task.Delay(WaitTime * 3, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(2);

        result = throttled();
        result.Should().Be(3);
        ExecutionCount.Should().Be(3);
    }

    [Fact]
    public async Task ThrottleMakesProgressDuringRepeatedInvocations() {
        Func<int> throttled = Throttler.Throttle(() => ++ExecutionCount, WaitTime * 2, leading: true, trailing: true).Invoke;

        int result = throttled();
        result.Should().Be(1, "result after 0 seconds");
        ExecutionCount.Should().Be(1, "executionCount after 0 seconds and 1 invocation");
        await Task.Delay(WaitTime, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(1, "executionCount after 0.1 seconds and 1 invocation");
        result = throttled();
        result.Should().Be(1, "result after 0.1 seconds");
        ExecutionCount.Should().Be(1, "executionCount after 0.1 seconds and 2 invocation");
        await Task.Delay(WaitTime * 2, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(2, "executionCount after 0.3 seconds and 2 invocation");
        result = throttled();
        result.Should().Be(2, "result after 0.3 seconds");
        ExecutionCount.Should().Be(2, "executionCount after 0.3 seconds and 3 invocation");
        await Task.Delay(WaitTime * 2, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(3, "executionCount after 0.5 seconds and 3 invocation");
        result = throttled();
        result.Should().Be(3, "result after 0.5 seconds");
        ExecutionCount.Should().Be(3, "executionCount after 0.5 seconds and 4 invocation");
        await Task.Delay(WaitTime * 2, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(4, "executionCount after 0.7 seconds and 4 invocation");
        result = throttled();
        result.Should().Be(4, "result after 0.7 seconds");
        ExecutionCount.Should().Be(4, "executionCount after 0.7 seconds and 5 invocation");
    }

    [Fact]
    public async Task ThrottleLeadingOnly() {
        Func<int> throttled = Throttler.Throttle(() => ++ExecutionCount, WaitTime, leading: true, trailing: false).Invoke;

        int result = throttled();
        result.Should().Be(1);
        ExecutionCount.Should().Be(1);

        result = throttled();
        result.Should().Be(1);
        ExecutionCount.Should().Be(1);

        await Task.Delay(WaitTime * 2, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(1);

        result = throttled();
        result.Should().Be(2);
        ExecutionCount.Should().Be(2);
    }

    [Fact]
    public async Task ThrottleFuncTrailingOnly() {
        Func<int> throttled = Throttler.Throttle(() => ++ExecutionCount, WaitTime, leading: false, trailing: true).Invoke;

        int result = throttled();
        result.Should().Be(0);
        ExecutionCount.Should().Be(0);

        result = throttled();
        result.Should().Be(0);
        ExecutionCount.Should().Be(0);

        await Task.Delay(WaitTime * 2, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(1);

        result = throttled();
        result.Should().Be(1);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public async Task ThrottleFuncLeadingAndTrailing() {
        Func<int, int> throttled = Throttler.Throttle((int arg) => {
            MostRecentArgument = arg;
            return ++ExecutionCount;
        }, WaitTime, leading: true, trailing: true).Invoke;

        int result = throttled(100);
        result.Should().Be(1);
        ExecutionCount.Should().Be(1);
        MostRecentArgument.Should().Be(100);

        result = throttled(200);
        result.Should().Be(1);
        ExecutionCount.Should().Be(1);
        MostRecentArgument.Should().Be(100);

        await Task.Delay(WaitTime * 3, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(2);
        MostRecentArgument.Should().Be(200);

        result = throttled(300);
        result.Should().Be(3);
        ExecutionCount.Should().Be(3);
        MostRecentArgument.Should().Be(300);
    }

    [Fact]
    public async Task DisposingPreventsLaterExecutions() {
        RateLimitedFunc<int> rateLimited = Throttler.Throttle(() => ++ExecutionCount, WaitTime, leading: true, trailing: true);
        Func<int>            throttled   = rateLimited.Invoke;

        int result = throttled();
        result.Should().Be(1);
        ExecutionCount.Should().Be(1);

        result = throttled();
        result.Should().Be(1);
        ExecutionCount.Should().Be(1);

        rateLimited.Dispose();

        await Task.Delay(WaitTime * 3, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void DisposingPreventsImmediateExecutions() {
        RateLimitedAction rateLimited = Throttler.Throttle(() => { ++ExecutionCount; }, WaitTime, leading: true, trailing: true);
        Action            throttled   = rateLimited.Invoke;

        ExecutionCount.Should().Be(0);

        rateLimited.Dispose();

        throttled();
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public async Task AutoGeneratedThrottleOverloadSpecifiesMaxWait() {
        Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> throttled = Throttler.Throttle(
            (int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l, int m, int n, int o, int p) => { ++ExecutionCount; },
            WaitTime, leading: true, trailing: true).Invoke;

        // 0.0s
        throttled(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        ExecutionCount.Should().Be(1);
        await Task.Delay(WaitTime / 2, TestContext.Current.CancellationToken);

        // 0.5s
        ExecutionCount.Should().Be(1);
        throttled(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        ExecutionCount.Should().Be(1);
        await Task.Delay(WaitTime, TestContext.Current.CancellationToken);

        // 1.5s
        ExecutionCount.Should().Be(2);
        throttled(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        ExecutionCount.Should().Be(2);
        await Task.Delay(WaitTime, TestContext.Current.CancellationToken);

        // 2.5s
        ExecutionCount.Should().Be(3);
        throttled(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        ExecutionCount.Should().Be(3);
        await Task.Delay(WaitTime, TestContext.Current.CancellationToken);

        // 3.5s
        ExecutionCount.Should().Be(4);
        throttled(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        ExecutionCount.Should().Be(4);
    }

}