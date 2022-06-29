using System;
using System.Threading.Tasks;
using FluentAssertions;
using ThrottleDebounce;
using Xunit;

namespace Tests; 

public class DebouncerTest: BaseTest {

    public class General: ThrottlerTest {

        [Fact]
        public void ThrottleActionNeitherLeadingNorTrailing() {
            Action thrower = () => Debouncer.Debounce(() => { }, WAIT_TIME, leading: false, trailing: false);
            thrower.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ThrottleActionZeroWaitTime() {
            Action thrower = () => Debouncer.Debounce(() => { }, TimeSpan.Zero);
            thrower.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ThrottleActionNegativeWaitTime() {
            Action thrower = () => Debouncer.Debounce(() => { }, TimeSpan.FromMilliseconds(-100));
            thrower.Should().Throw<ArgumentException>();
        }

    }

    // Put each test method inside its own class so that xUnit will run them in parallel, which saves time since each test involves some waiting
    public class DebounceActionLeadingAndTrailingClass: DebouncerTest {

        [Fact]
        public async void DebounceActionLeadingAndTrailing() {
            Func<int> debounced = Debouncer.Debounce(() => ++executionCount, WAIT_TIME, leading: true, trailing: true).Invoke;

            int result = debounced();
            result.Should().Be(1);
            executionCount.Should().Be(1);

            result = debounced.Invoke();
            result.Should().Be(1);
            executionCount.Should().Be(1);

            await Task.Delay(WAIT_TIME * 3);

            executionCount.Should().Be(2);

            result = debounced.Invoke();
            result.Should().Be(3);
            executionCount.Should().Be(3);
        }

    }

    public class DebounceMakesProgressDuringRepeatedInvocationsClass: DebouncerTest {

        [Fact]
        public async void DebounceMakesProgressDuringRepeatedInvocations() {
            Func<int> debounced = Debouncer.Debounce(() => ++executionCount, WAIT_TIME * 2, leading: true, trailing: true).Invoke;

            int result = debounced();
            result.Should().Be(1, "result after 0 seconds");
            executionCount.Should().Be(1, "executionCount after 0 seconds and 1 invocation");
            await Task.Delay(WAIT_TIME);

            executionCount.Should().Be(1, "executionCount after 0.1 seconds and 1 invocation");
            result = debounced();
            result.Should().Be(1, "result after 0.1 seconds");
            executionCount.Should().Be(1, "executionCount after 0.1 seconds and 2 invocations");
            await Task.Delay(WAIT_TIME);

            executionCount.Should().Be(1, "executionCount after 0.2 seconds and 2 invocation");
            result = debounced();
            result.Should().Be(1, "result after 0.2 seconds");
            executionCount.Should().Be(1, "executionCount after 0.2 seconds and 3 invocations");
            await Task.Delay(WAIT_TIME);

            executionCount.Should().Be(1, "executionCount after 0.3 seconds and 3 invocation");
            result = debounced();
            result.Should().Be(1, "result after 0.3 seconds");
            executionCount.Should().Be(1, "executionCount after 0.3 seconds and 4 invocations");
            await Task.Delay(WAIT_TIME);

            executionCount.Should().Be(1, "executionCount after 0.4 seconds and 4 invocation");
            result = debounced();
            result.Should().Be(1, "result after 0.4 seconds");
            executionCount.Should().Be(1, "executionCount after 0.4 seconds and 5 invocations");
            await Task.Delay(WAIT_TIME * 6);

            executionCount.Should().Be(2, "executionCount after 1 second and 5 invocation");
            result = debounced();
            result.Should().Be(3, "result after 1 second");
            executionCount.Should().Be(3, "executionCount after 1 second and 6 invocations");
        }

    }

    public class DebounceActionLeadingOnlyClass: DebouncerTest {

        [Fact]
        public async void DebounceActionLeadingOnly() {
            Func<int> debounced = Debouncer.Debounce(() => ++executionCount, WAIT_TIME, leading: true, trailing: false).Invoke;

            int result = debounced.Invoke();
            result.Should().Be(1);
            executionCount.Should().Be(1);

            result = debounced.Invoke();
            result.Should().Be(1);
            executionCount.Should().Be(1);

            await Task.Delay(WAIT_TIME * 2);

            executionCount.Should().Be(1);

            result = debounced.Invoke();
            result.Should().Be(2);
            executionCount.Should().Be(2);
        }

    }

    public class DebounceActionTrailingOnlyClass: DebouncerTest {

        [Fact]
        public async void DebounceActionTrailingOnly() {
            Func<int> debounced = Debouncer.Debounce(() => ++executionCount, WAIT_TIME, leading: false, trailing: true).Invoke;

            int result = debounced.Invoke();
            result.Should().Be(default);
            executionCount.Should().Be(0);

            result = debounced.Invoke();
            result.Should().Be(default);
            executionCount.Should().Be(0);

            await Task.Delay(WAIT_TIME * 2);

            executionCount.Should().Be(1);

            result = debounced.Invoke();
            result.Should().Be(1);
            executionCount.Should().Be(1);
        }

    }

    public class DisposingPreventsLaterExecutionsClass: ThrottlerTest {

        [Fact]
        public async void DisposingPreventsLaterExecutions() {
            RateLimitedFunc<int> rateLimited = Debouncer.Debounce(() => ++executionCount, WAIT_TIME, leading: true, trailing: true);
            Func<int>            debounced   = rateLimited.Invoke;

            int result = debounced();
            result.Should().Be(1);
            executionCount.Should().Be(1);

            result = debounced();
            result.Should().Be(1);
            executionCount.Should().Be(1);

            rateLimited.Dispose();

            await Task.Delay(WAIT_TIME * 3);

            executionCount.Should().Be(1);
        }

    }

}