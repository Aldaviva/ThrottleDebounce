using System;
using System.Threading.Tasks;
using FluentAssertions;
using ThrottleDebounce;
using Xunit;

namespace Tests {

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
                Func<int> debounced = Debouncer.Debounce(() => ++executionCount, WAIT_TIME, leading: true, trailing: true).Invoke;

                // 0.0s
                int result = debounced.Invoke();
                result.Should().Be(1);
                executionCount.Should().Be(1);
                await Task.Delay(WAIT_TIME / 2);

                // 0.5s
                executionCount.Should().Be(1);
                result = debounced.Invoke();
                result.Should().Be(1);
                executionCount.Should().Be(1);
                await Task.Delay(WAIT_TIME / 2);

                // 1.0s
                executionCount.Should().Be(1);
                result = debounced.Invoke();
                result.Should().Be(1);
                executionCount.Should().Be(1);
                await Task.Delay(WAIT_TIME / 2);

                // 1.5s
                executionCount.Should().Be(1);
                result = debounced.Invoke();
                result.Should().Be(1);
                executionCount.Should().Be(1);
                await Task.Delay(WAIT_TIME / 2);

                // 2.0s
                executionCount.Should().Be(1);
                result = debounced.Invoke();
                result.Should().Be(1);
                executionCount.Should().Be(1);
                await Task.Delay(WAIT_TIME * 3);

                // 5.0s
                executionCount.Should().Be(2);
                result = debounced.Invoke();
                result.Should().Be(3);
                executionCount.Should().Be(3);
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

}