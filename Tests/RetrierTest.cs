using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using ThrottleDebounce;
using Xunit;

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
        Retrier.Attempt(_ => failer.InvokeAction(), delay: _ => TimeSpan.Zero);
        failer.InvocationCount.Should().Be(2);
    }

    [Fact]
    public void ActionRetryFailure() {
        Failer failer  = new(2);
        Action thrower = () => Retrier.Attempt(_ => failer.InvokeAction(), delay: _ => TimeSpan.FromMilliseconds(1));
        thrower.Should().ThrowExactly<Failure>();
        failer.InvocationCount.Should().Be(2);
    }

    [Fact]
    public void ActionRetryNotAllowed() {
        Failer failer  = new(1);
        Action thrower = () => Retrier.Attempt(_ => failer.InvokeAction(), isRetryAllowed: _ => false);
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
        Func<bool> thrower = () => Retrier.Attempt(_ => failer.InvokeFunc(), delay: _ => TimeSpan.FromMilliseconds(1));
        thrower.Should().ThrowExactly<Failure>();
        failer.InvocationCount.Should().Be(2);
    }

    [Fact]
    public void FuncRetryNotAllowed() {
        Failer     failer  = new(1);
        Func<bool> thrower = () => Retrier.Attempt(_ => failer.InvokeFunc(), isRetryAllowed: _ => false);
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
        await Retrier.Attempt(async _ => await failer.InvokeActionAsync(), delay: _ => TimeSpan.Zero);
        failer.InvocationCount.Should().Be(2);
    }

    [Fact]
    public async Task AsyncActionRetryFailure() {
        Failer     failer  = new(2);
        Func<Task> thrower = async () => await Retrier.Attempt(async _ => await failer.InvokeActionAsync(), delay: _ => TimeSpan.FromMilliseconds(1));
        await thrower.Should().ThrowExactlyAsync<Failure>();
        failer.InvocationCount.Should().Be(2);
    }

    [Fact]
    public async Task AsyncActionRetryNotAllowed() {
        Failer     failer  = new(1);
        Func<Task> thrower = async () => await Retrier.Attempt(async _ => await failer.InvokeActionAsync(), isRetryAllowed: _ => false);
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
        await Retrier.Attempt(async _ => await failer.InvokeFuncAsync(), delay: _ => TimeSpan.Zero);
        failer.InvocationCount.Should().Be(2);
    }

    [Fact]
    public async Task AsyncFuncRetryFailure() {
        Failer     failer  = new(2);
        Func<Task> thrower = async () => await Retrier.Attempt(async _ => await failer.InvokeFuncAsync(), delay: _ => TimeSpan.FromMilliseconds(1));
        await thrower.Should().ThrowExactlyAsync<Failure>();
        failer.InvocationCount.Should().Be(2);
    }

    [Fact]
    public async Task AsyncFuncRetryNotAllowed() {
        Failer     failer  = new(1);
        Func<Task> thrower = async () => await Retrier.Attempt(async _ => await failer.InvokeFuncAsync(), isRetryAllowed: _ => false);
        await thrower.Should().ThrowExactlyAsync<Failure>();
        failer.InvocationCount.Should().Be(1);
    }

    [Fact]
    public async Task MaxDelay() {
        Failer                  failer  = new(1);
        CancellationTokenSource cts     = new(200);
        Func<Task>              thrower = async () => await Retrier.Attempt(async _ => await failer.InvokeActionAsync(), delay: _ => TimeSpan.MaxValue, cancellationToken: cts.Token);
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
        }, 10, cancellationToken: cts.Token);

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
        }, 10, cancellationToken: cts.Token);

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
            }, maxAttempts, cancellationToken: cts.Token);
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
        }, 10, cancellationToken: cts.Token);

        await thrower.Should().ThrowAsync<TaskCanceledException>();
        failer.InvocationCount.Should().Be(5);
    }

    [Fact]
    public void OnBeforeRetry() {
        Failer     failer          = new(1);
        int?       delayAttempt    = null;
        int?       onBeforeAttempt = null;
        Exception? exception       = null;

        Retrier.Attempt(_ => failer.InvokeAction(),
            delay: a => {
                delayAttempt = a;
                return TimeSpan.Zero;
            }, beforeRetry: (a, e) => {
                onBeforeAttempt = a;
                exception       = e;
            });

        delayAttempt.Should().Be(0);
        onBeforeAttempt.Should().Be(0);
        exception.Should().BeOfType<Failure>();
    }

    [Theory]
    [InlineData(0, 8000)]
    [InlineData(1, 8000)]
    [InlineData(2, 8000)]
    [InlineData(3, 8000)]
    public void ConstantDelay(int afterAttempt, int expectedMillis) {
        Func<int, TimeSpan> delay = Retrier.Delays.Constant(TimeSpan.FromSeconds(8));
        delay(afterAttempt).TotalMilliseconds.Should().BeApproximately(expectedMillis, 2);
    }

    [Theory]
    [InlineData(0, 1000)]
    [InlineData(1, 2000)]
    [InlineData(2, 3000)]
    [InlineData(3, 3000)]
    public void LinearDelay(int afterAttempt, int expectedMillis) {
        Func<int, TimeSpan> delay = Retrier.Delays.Linear(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3));
        delay(afterAttempt).TotalMilliseconds.Should().BeApproximately(expectedMillis, 2);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1000)]
    [InlineData(2, 4000)]
    [InlineData(3, 9000)]
    public void ExponentialDelay(int afterAttempt, int expectedMillis) {
        Func<int, TimeSpan> delay = Retrier.Delays.Exponential(TimeSpan.FromSeconds(1));
        delay(afterAttempt).TotalMilliseconds.Should().BeApproximately(expectedMillis, 2);
    }

    [Theory]
    [InlineData(0, 0000)]
    [InlineData(1, 2000)]
    [InlineData(2, 4000)]
    [InlineData(3, 8000)]
    public void PowerDelay(int afterAttempt, int expectedMillis) {
        Func<int, TimeSpan> delay = Retrier.Delays.Power(TimeSpan.FromSeconds(1));
        delay(afterAttempt).TotalMilliseconds.Should().BeApproximately(expectedMillis, 2);
    }

    [Theory]
    [InlineData(0, 0000)]
    [InlineData(1, 1000)]
    [InlineData(2, 1301)]
    [InlineData(3, 1477)]
    public void LogDelay(int afterAttempt, int expectedMillis) {
        Func<int, TimeSpan> delay = Retrier.Delays.Logarithm(TimeSpan.FromSeconds(1));
        delay(afterAttempt).TotalMilliseconds.Should().BeApproximately(expectedMillis, 2);
    }

    [Fact]
    public void RandomDelay() {
        Func<int, TimeSpan> delay = Retrier.Delays.MonteCarlo(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
        for (int attempt = 0; attempt < 10; attempt++) {
            delay(attempt).TotalMilliseconds.Should().BeInRange(1000, 10_000);
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