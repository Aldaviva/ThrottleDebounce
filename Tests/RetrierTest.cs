using ThrottleDebounce.Retry;

namespace Tests;

public class RetrierTest {

    [Fact]
    public void ActionImmediateSuccess() {
        Failer failer = new(0);
        Retrier.Attempt(_ => failer.InvokeAction());
        failer.InvocationCount.Should().Be(1);
    }

    [Fact]
    public void ActionRetrySuccess() {
        Failer failer = new(1);
        Retrier.Attempt(_ => failer.InvokeAction(), new RetryOptions { Delay = _ => TimeSpan.Zero });
        failer.InvocationCount.Should().Be(2);
    }

    [Fact]
    public void ActionRetryFailure() {
        Failer failer  = new(2);
        Action thrower = () => Retrier.Attempt(_ => failer.InvokeAction(), new RetryOptions { MaxAttempts = 2, Delay = _ => TimeSpan.FromMilliseconds(1) });
        thrower.Should().ThrowExactly<Failure>();
        failer.InvocationCount.Should().Be(2);
    }

    [Fact]
    public void ActionRetryNotAllowed() {
        Failer failer  = new(1);
        Action thrower = () => Retrier.Attempt(_ => failer.InvokeAction(), new RetryOptions { IsRetryAllowed = (_, _) => false });
        thrower.Should().ThrowExactly<Failure>();
        failer.InvocationCount.Should().Be(1);
    }

    [Fact]
    public void FuncImmediateSuccess() {
        Failer failer = new(0);
        Retrier.Attempt(_ => failer.InvokeFunc());
        failer.InvocationCount.Should().Be(1);
    }

    [Fact]
    public void FuncRetrySuccess() {
        Failer failer = new(1);
        Retrier.Attempt(_ => failer.InvokeFunc());
        failer.InvocationCount.Should().Be(2);
    }

    [Fact]
    public void FuncRetryFailure() {
        Failer     failer  = new(2);
        Func<bool> thrower = () => Retrier.Attempt(_ => failer.InvokeFunc(), new RetryOptions { MaxAttempts = 2, Delay = _ => TimeSpan.FromMilliseconds(1) });
        thrower.Should().ThrowExactly<Failure>();
        failer.InvocationCount.Should().Be(2);
    }

    [Fact]
    public void FuncRetryNotAllowed() {
        Failer     failer  = new(1);
        Func<bool> thrower = () => Retrier.Attempt(_ => failer.InvokeFunc(), new RetryOptions { IsRetryAllowed = (_, _) => false });
        thrower.Should().ThrowExactly<Failure>();
        failer.InvocationCount.Should().Be(1);
    }

    [Fact]
    public async Task AsyncActionImmediateSuccess() {
        Failer failer = new(0);
        await Retrier.Attempt(async _ => await failer.InvokeActionAsync());
        failer.InvocationCount.Should().Be(1);
    }

    [Fact]
    public async Task AsyncActionRetrySuccess() {
        Failer failer = new(1);
        await Retrier.Attempt(async _ => await failer.InvokeActionAsync(), new AsyncRetryOptions { Delay = _ => TimeSpan.Zero });
        failer.InvocationCount.Should().Be(2);
    }

    [Fact]
    public async Task AsyncActionRetryFailure() {
        Failer     failer  = new(2);
        Func<Task> thrower = async () => await Retrier.Attempt(async _ => await failer.InvokeActionAsync(), new RetryOptions { MaxAttempts = 2, Delay = _ => TimeSpan.FromMilliseconds(1) });
        await thrower.Should().ThrowExactlyAsync<Failure>();
        failer.InvocationCount.Should().Be(2);
    }

    [Fact]
    public async Task AsyncActionRetryNotAllowed() {
        Failer     failer  = new(1);
        Func<Task> thrower = async () => await Retrier.Attempt(async _ => await failer.InvokeActionAsync(), new RetryOptions { IsRetryAllowed = (_, _) => false });
        await thrower.Should().ThrowExactlyAsync<Failure>();
        failer.InvocationCount.Should().Be(1);
    }

    [Fact]
    public async Task AsyncFuncImmediateSuccess() {
        Failer failer = new(0);
        await Retrier.Attempt(async _ => await failer.InvokeFuncAsync());
        failer.InvocationCount.Should().Be(1);
    }

    [Fact]
    public async Task AsyncFuncRetrySuccess() {
        Failer failer = new(1);
        await Retrier.Attempt(async _ => await failer.InvokeFuncAsync(), new RetryOptions { Delay = _ => TimeSpan.Zero });
        failer.InvocationCount.Should().Be(2);
    }

    [Fact]
    public async Task AsyncFuncRetryFailure() {
        Failer     failer  = new(2);
        Func<Task> thrower = async () => await Retrier.Attempt(async _ => await failer.InvokeFuncAsync(), new RetryOptions { MaxAttempts = 2, Delay = _ => TimeSpan.FromMilliseconds(1) });
        await thrower.Should().ThrowExactlyAsync<Failure>();
        failer.InvocationCount.Should().Be(2);
    }

    [Fact]
    public async Task AsyncFuncRetryNotAllowed() {
        Failer     failer  = new(1);
        Func<Task> thrower = async () => await Retrier.Attempt(async _ => await failer.InvokeFuncAsync(), new RetryOptions { IsRetryAllowed = (_, _) => false });
        await thrower.Should().ThrowExactlyAsync<Failure>();
        failer.InvocationCount.Should().Be(1);
    }

    [Fact]
    public async Task MaxDelay() {
        Failer failer = new(1);
        CancellationTokenSource cts = new(200);
        Func<Task> thrower = async () => await Retrier.Attempt(async _ => await failer.InvokeActionAsync(), new RetryOptions { Delay = _ => TimeSpan.MaxValue, CancellationToken = cts.Token });
        await thrower.Should().ThrowAsync<TaskCanceledException>();
        failer.InvocationCount.Should().Be(1);
    }

    [Fact]
    public void ActionCancellation() {
        Failer                  failer = new();
        CancellationTokenSource cts    = new();
        Action thrower = () => Retrier.Attempt(attempt => {
            if (attempt >= 4) {
                cts.Cancel();
            }

            failer.InvokeAction();
        }, new RetryOptions { MaxAttempts = 10, CancellationToken = cts.Token });

        thrower.Should().Throw<TaskCanceledException>();
        failer.InvocationCount.Should().Be(5);
    }

    [Fact]
    public void FuncCancellation() {
        Failer                  failer = new();
        CancellationTokenSource cts    = new();
        Func<int> thrower = () => Retrier.Attempt(attempt => {
            if (attempt >= 4) {
                cts.Cancel();
            }

            failer.InvokeAction();
            return -1; // never returns because failer always throws
        }, new RetryOptions { MaxAttempts = 10, CancellationToken = cts.Token });

        thrower.Should().Throw<TaskCanceledException>();
        failer.InvocationCount.Should().Be(5);
    }

    [Fact]
    public async Task AsyncFuncTCancellation() {
        Failer                  failer      = new();
        CancellationTokenSource cts         = new();
        int                     maxAttempts = 10;
        Func<Task<int>> thrower = () => {
            return Retrier.Attempt(attempt => {
                if (attempt >= 4) {
                    cts.Cancel();
                }

                failer.InvokeAction();
                return Task.FromResult(-1); // never returns because failer always throws
            }, new RetryOptions { MaxAttempts = maxAttempts, CancellationToken = cts.Token });
        };

        await thrower.Should().ThrowAsync<TaskCanceledException>();
        failer.InvocationCount.Should().Be(5);
    }

    [Fact]
    public async Task AsyncFuncCancellation() {
        Failer                  failer = new();
        CancellationTokenSource cts    = new();
        Func<Task> thrower = () => Retrier.Attempt(attempt => {
            if (attempt >= 4) {
                cts.Cancel();
            }

            failer.InvokeAction();
            return Task.CompletedTask; // never returns because failer always throws
        }, new RetryOptions { MaxAttempts = 10, CancellationToken = cts.Token });

        await thrower.Should().ThrowAsync<TaskCanceledException>();
        failer.InvocationCount.Should().Be(5);
    }

    [Fact]
    public void ActionMaxOverallDurationExceededBeforeRetry() {
        Failer failer = new();
        Action thrower = () => Retrier.Attempt(_ => failer.InvokeAction(), new RetryOptions {
            MaxOverallDuration = TimeSpan.FromMilliseconds(100),
            BeforeRetry        = (_, _) => Thread.Sleep(250)
        });

        thrower.Should().ThrowExactly<Failure>();
        failer.InvocationCount.Should().Be(1);
    }

    [Fact]
    public void FuncMaxOverallDurationExceededBeforeRetry() {
        Failer failer = new();
        Action thrower = () => Retrier.Attempt(_ => failer.InvokeFunc(), new RetryOptions {
            MaxOverallDuration = TimeSpan.FromMilliseconds(100),
            BeforeRetry        = (_, _) => Thread.Sleep(250)
        });

        thrower.Should().ThrowExactly<Failure>();
        failer.InvocationCount.Should().Be(1);
    }

    [Fact]
    public async Task AsyncFuncMaxOverallDurationExceededBeforeRetry() {
        Failer failer = new();
        Func<Task> thrower = () => Retrier.Attempt(_ => {
            failer.InvokeAction();
            return Task.CompletedTask;
        }, new RetryOptions {
            MaxOverallDuration = TimeSpan.FromMilliseconds(100),
            BeforeRetry        = (_, _) => Thread.Sleep(250)
        });

        await thrower.Should().ThrowExactlyAsync<Failure>();
        failer.InvocationCount.Should().Be(1);
    }

    [Fact]
    public async Task AsyncFuncTMaxOverallDurationExceededBeforeRetry() {
        Failer failer = new();
        Func<Task<int>> thrower = () => Retrier.Attempt(_ => {
            failer.InvokeAction();
            return Task.FromResult(-1);
        }, new RetryOptions {
            MaxOverallDuration = TimeSpan.FromMilliseconds(100),
            BeforeRetry        = (_, _) => Thread.Sleep(250)
        });

        await thrower.Should().ThrowExactlyAsync<Failure>();
        failer.InvocationCount.Should().Be(1);
    }

    [Fact]
    public void OnBeforeRetry() {
        Failer     failer          = new(1);
        long?      delayAttempt    = null;
        long?      onBeforeAttempt = null;
        Exception? exception       = null;

        Retrier.Attempt(_ => failer.InvokeAction(), new RetryOptions {
            MaxAttempts = 2,
            Delay = a => {
                delayAttempt = a;
                return TimeSpan.Zero;
            },
            BeforeRetry = (e, a) => {
                onBeforeAttempt = a;
                exception       = e;
            }
        });

        delayAttempt.Should().Be(0);
        onBeforeAttempt.Should().Be(1);
        exception.Should().BeOfType<Failure>();
    }

    [Theory]
    [InlineData(0, 8000)]
    [InlineData(1, 8000)]
    [InlineData(2, 8000)]
    [InlineData(3, 8000)]
    public void ConstantDelay(long afterAttempt, int expectedMillis) {
        Func<long, TimeSpan> delay = Delays.Constant(TimeSpan.FromSeconds(8));
        delay(afterAttempt).TotalMilliseconds.Should().BeApproximately(expectedMillis, 2);

        delay = Delays.Constant(8000);
        delay(afterAttempt).TotalMilliseconds.Should().BeApproximately(expectedMillis, 2);
    }

    [Theory]
    [InlineData(0, 1000)]
    [InlineData(1, 2000)]
    [InlineData(2, 3000)]
    [InlineData(3, 3000)]
    public void LinearDelay(long afterAttempt, int expectedMillis) {
        Func<long, TimeSpan> delay = Delays.Linear(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3));
        delay(afterAttempt).TotalMilliseconds.Should().BeApproximately(expectedMillis, 2);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1000)]
    [InlineData(2, 4000)]
    [InlineData(3, 9000)]
    public void ExponentialDelay(int afterAttempt, int expectedMillis) {
        Func<long, TimeSpan> delay = Delays.Exponential(TimeSpan.FromSeconds(1));
        delay(afterAttempt).TotalMilliseconds.Should().BeApproximately(expectedMillis, 2);
    }

    [Theory]
    [InlineData(0, 0000)]
    [InlineData(1, 2000)]
    [InlineData(2, 4000)]
    [InlineData(3, 8000)]
    [InlineData(128, 60000)]
    public void PowerDelay(long afterAttempt, int expectedMillis) {
        Func<long, TimeSpan> delay = Delays.Power(TimeSpan.FromSeconds(1), max: TimeSpan.FromMinutes(1));
        delay(afterAttempt).TotalMilliseconds.Should().BeApproximately(expectedMillis, 2);
    }

    [Theory]
    [InlineData(0, 0000)]
    [InlineData(1, 1000)]
    [InlineData(2, 1301)]
    [InlineData(3, 1477)]
    public void LogDelay(long afterAttempt, int expectedMillis) {
        Func<long, TimeSpan> delay = Delays.Logarithmic(TimeSpan.FromSeconds(1));
        delay(afterAttempt).TotalMilliseconds.Should().BeApproximately(expectedMillis, 2);
    }

    [Fact]
    public void RandomDelay() {
        Func<long, TimeSpan> delay = Delays.MonteCarlo(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
        for (int attempt = 0; attempt < 10; attempt++) {
            delay(attempt).TotalMilliseconds.Should().BeInRange(1000, 10_000);
        }

        delay = Delays.MonteCarlo(TimeSpan.FromSeconds(10));
        for (int attempt = 0; attempt < 10; attempt++) {
            delay(attempt).TotalMilliseconds.Should().BeInRange(0, 10_000);
        }
    }

    public class RetryOptionsTest {

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public void MaxAttemptsClipping(int maxAttempts, int expected) {
            new RetryOptions { MaxAttempts      = maxAttempts }.MaxAttempts.Should().Be(expected);
            new AsyncRetryOptions { MaxAttempts = maxAttempts }.MaxAttempts.Should().Be(expected);
        }

        [Theory, MemberData(nameof(OptionsMaxOverallDurationData))]
        public void OptionsMaxOverallDuration(TimeSpan? maxOverallDuration, TimeSpan? expected) {
            new RetryOptions().MaxOverallDuration.Should().BeNull();

            IAsyncRetryOptions options = new RetryOptions { MaxOverallDuration = maxOverallDuration };
            options.MaxOverallDuration.Should().Be(expected);

            options = new AsyncRetryOptions { MaxOverallDuration = maxOverallDuration };
            options.MaxOverallDuration.Should().Be(expected);
        }

        public static TheoryData<TimeSpan?, TimeSpan?> OptionsMaxOverallDurationData => new() {
            { null, null },
            { Timeout.InfiniteTimeSpan, null },
            { TimeSpan.FromSeconds(-1), TimeSpan.Zero },
            { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1) }
        };

        [Fact]
        public async Task SyncOptionsForAsyncAttemptSuccess() {
            bool      afterFailureRan = false, beforeRetryRan = false;
            Exception exception       = new("test");

            IAsyncRetryOptions options = new RetryOptions {
                IsRetryAllowed = (_, _) => exception is not OutOfMemoryException,
                AfterFailure   = (_, _) => afterFailureRan = true,
                BeforeRetry    = (_, _) => beforeRetryRan = true
            };

            (await options.IsRetryAllowed!(exception, 0)).Should().BeTrue();
            await options.AfterFailure!(exception, 0);
            await options.BeforeRetry!(exception, 0);

            afterFailureRan.Should().BeTrue();
            beforeRetryRan.Should().BeTrue();
        }

        [Fact]
        public async Task SyncOptionsForAsyncAttemptFailure() {
            Exception exception = new("test");

            IAsyncRetryOptions options = new RetryOptions {
                IsRetryAllowed = (_, _) => throw new ApplicationException("test2"),
                AfterFailure   = (_, _) => throw new ApplicationException("test2"),
                BeforeRetry    = (_, _) => throw new ApplicationException("test2")
            };

            await options.Invoking(o => o.IsRetryAllowed!(exception, 0)).Should().ThrowAsync<ApplicationException>();
            await options.Invoking(o => o.AfterFailure!(exception, 0)).Should().ThrowAsync<ApplicationException>();
            await options.Invoking(o => o.BeforeRetry!(exception, 0)).Should().ThrowAsync<ApplicationException>();
        }

        /// This is impossible due to the type system on the methods that take these types, because instead of taking the IRetryOptions, it takes the RetryOptions.
        [Fact]
        public void AsyncOptionsForSyncAttempt() {
            Exception exception = new("test");

            IRetryOptions options = new AsyncRetryOptions {
                IsRetryAllowed = (_, _) => Task.FromResult(exception is not OutOfMemoryException),
                AfterFailure   = (_, _) => Task.CompletedTask,
                BeforeRetry    = (_, _) => Task.CompletedTask
            };

            options.IsRetryAllowed.Should().BeNull();
            options.AfterFailure.Should().BeNull();
            options.BeforeRetry.Should().BeNull();
        }

    }

    private class Failure: Exception;

    private class Failer {

        private static readonly Failure Exception = new();

        private readonly int? _timesToFail;

        public int InvocationCount { get; private set; }

        public Failer(int? timesToFail = null) {
            _timesToFail = timesToFail;
        }

        private void FailIfNecessary() {
            InvocationCount++;
            if (!_timesToFail.HasValue || InvocationCount <= _timesToFail) {
                throw Exception;
            }
        }

        public void InvokeAction() {
            FailIfNecessary();
        }

        public bool InvokeFunc() {
            FailIfNecessary();

            return true;
        }

        public Task InvokeActionAsync() {
            FailIfNecessary();
            return Task.CompletedTask;
        }

        public Task<bool> InvokeFuncAsync() {
            FailIfNecessary();
            return Task.FromResult(true);
        }

    }

}