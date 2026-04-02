namespace Tests;

public class DebouncerTest: BaseTest {

    [Fact]
    public void ThrottleActionNeitherLeadingNorTrailing() {
        Action thrower = () => Debouncer.Debounce(() => {}, WaitTime, leading: false, trailing: false);
        thrower.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ThrottleActionZeroWaitTime() {
        Action thrower = () => Debouncer.Debounce(() => {}, TimeSpan.Zero);
        thrower.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ThrottleActionNegativeWaitTime() {
        Action thrower = () => Debouncer.Debounce(() => {}, TimeSpan.FromMilliseconds(-100));
        thrower.Should().Throw<ArgumentException>();
    }

    [Fact]
    public async Task DebounceActionLeadingAndTrailing() {
        Func<int> debounced = Debouncer.Debounce(() => ++ExecutionCount, WaitTime, leading: true, trailing: true).Invoke;

        int result = debounced();
        result.Should().Be(1);
        ExecutionCount.Should().Be(1);

        result = debounced.Invoke();
        result.Should().Be(1);
        ExecutionCount.Should().Be(1);

        await Task.Delay(WaitTime * 3, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(2);

        result = debounced.Invoke();
        result.Should().Be(3);
        ExecutionCount.Should().Be(3);
    }

    [Fact]
    public async Task DebounceMakesProgressDuringRepeatedInvocations() {
        Func<int> debounced = Debouncer.Debounce(() => ++ExecutionCount, WaitTime * 2, leading: true, trailing: true).Invoke;

        int result = debounced();
        result.Should().Be(1, "result after 0 seconds");
        ExecutionCount.Should().Be(1, "executionCount after 0 seconds and 1 invocation");
        await Task.Delay(WaitTime, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(1, "executionCount after 0.1 seconds and 1 invocation");
        result = debounced();
        result.Should().Be(1, "result after 0.1 seconds");
        ExecutionCount.Should().Be(1, "executionCount after 0.1 seconds and 2 invocations");
        await Task.Delay(WaitTime, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(1, "executionCount after 0.2 seconds and 2 invocation");
        result = debounced();
        result.Should().Be(1, "result after 0.2 seconds");
        ExecutionCount.Should().Be(1, "executionCount after 0.2 seconds and 3 invocations");
        await Task.Delay(WaitTime, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(1, "executionCount after 0.3 seconds and 3 invocation");
        result = debounced();
        result.Should().Be(1, "result after 0.3 seconds");
        ExecutionCount.Should().Be(1, "executionCount after 0.3 seconds and 4 invocations");
        await Task.Delay(WaitTime, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(1, "executionCount after 0.4 seconds and 4 invocation");
        result = debounced();
        result.Should().Be(1, "result after 0.4 seconds");
        ExecutionCount.Should().Be(1, "executionCount after 0.4 seconds and 5 invocations");
        await Task.Delay(WaitTime * 6, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(2, "executionCount after 1 second and 5 invocation");
        result = debounced();
        result.Should().Be(3, "result after 1 second");
        ExecutionCount.Should().Be(3, "executionCount after 1 second and 6 invocations");
    }

    [Fact]
    public async Task DebounceActionLeadingOnly() {
        Func<int> debounced = Debouncer.Debounce(() => ++ExecutionCount, WaitTime, leading: true, trailing: false).Invoke;

        int result = debounced.Invoke();
        result.Should().Be(1);
        ExecutionCount.Should().Be(1);

        result = debounced.Invoke();
        result.Should().Be(1);
        ExecutionCount.Should().Be(1);

        await Task.Delay(WaitTime * 2, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(1);

        result = debounced.Invoke();
        result.Should().Be(2);
        ExecutionCount.Should().Be(2);
    }

    [Fact]
    public async Task DebounceActionTrailingOnly() {
        Func<int> debounced = Debouncer.Debounce(() => ++ExecutionCount, WaitTime, leading: false, trailing: true).Invoke;

        int result = debounced.Invoke();
        result.Should().Be(0);
        ExecutionCount.Should().Be(0);

        result = debounced.Invoke();
        result.Should().Be(0);
        ExecutionCount.Should().Be(0);

        await Task.Delay(WaitTime * 2, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(1);

        result = debounced.Invoke();
        result.Should().Be(1);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public async Task DisposingPreventsLaterExecutions() {
        RateLimitedFunc<int> rateLimited = Debouncer.Debounce(() => ++ExecutionCount, WaitTime, leading: true, trailing: true);
        Func<int>            debounced   = rateLimited.Invoke;

        int result = debounced();
        result.Should().Be(1);
        ExecutionCount.Should().Be(1);

        result = debounced();
        result.Should().Be(1);
        ExecutionCount.Should().Be(1);

        rateLimited.Dispose();

        await Task.Delay(WaitTime * 3, TestContext.Current.CancellationToken);

        ExecutionCount.Should().Be(1);
    }

}