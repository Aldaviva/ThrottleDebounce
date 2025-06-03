#nullable enable

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace ThrottleDebounce;

/// <summary>
/// Run a delegate with retries if it throws an exception.
/// </summary>
public static class Retrier {

    // ExceptionAdjustment: M:System.TimeSpan.FromMilliseconds(System.Double) -T:System.OverflowException
    // These are the specific maximum values that don't overflow, tailored for the runtime version.
    private static readonly TimeSpan MaxDelay = TimeSpan.FromMilliseconds(Environment.Version.Major >= 6 ? uint.MaxValue - 1 : int.MaxValue);

    /// <summary>
    /// Run the given <paramref name="action"/> until it returns without throwing an exception.
    /// </summary>
    /// <param name="action">An action which is prone to sometimes throw exceptions. The <see cref="int"/> argument is the number of attempts, starting from <c>0</c> for the initial attempt.</param>
    /// <param name="options">Control limits and behaviors of the attempts.</param>
    /// <exception cref="TaskCanceledException">If the <see cref="Options.CancellationToken"/> is cancelled.</exception>
    /// <exception cref="Exception">Any exception thrown by <paramref name="action"/> on its final attempt.</exception>
    public static void Attempt(Action<int> action, Options options = default) {
        Stopwatch  totalDuration = new();
        Exception? lastException = null;
        int        attempt;
        for (attempt = 0; ShouldLoop(attempt, totalDuration, options); attempt++) {
            try {
                totalDuration.Start();
                action.Invoke(attempt);
                return;
            } catch (Exception e) when (e is not OutOfMemoryException) {
                lastException = e;
                options.AfterFailure?.Invoke(attempt, e);

                TimeSpan delay = GetDelay(options.Delay, attempt);

                // This brace-less syntax convinces dotCover that the "throw;" statement is fully covered.
                if (!ShouldRetry(e, options) || options.IsDurationExceeded(totalDuration.Elapsed + delay)) throw;

                if (delay > TimeSpan.Zero) {
                    Task.Delay(delay, options.CancellationToken).GetAwaiter().GetResult();
                }

                options.BeforeRetry?.Invoke(attempt + 1, e);
            }
        }

        if (options.CancellationToken.IsCancellationRequested) {
            throw new TaskCanceledException("Remaining Retrier attempts were cancelled");
        } else if (options.IsDurationExceeded(totalDuration.Elapsed)) {
            throw lastException!;
        }

        try {
            action.Invoke(attempt);
        } catch (Exception e) when (e is not OutOfMemoryException) {
            options.AfterFailure?.Invoke(attempt, e);
            throw;
        }
    }

    /// <summary>
    /// Run the given <paramref name="func"/> until it returns without throwing an exception.
    /// </summary>
    /// <param name="func">A function which is prone to sometimes throw exceptions. The <see cref="int"/> argument is the number of attempts, starting from <c>0</c> for the initial attempt.</param>
    /// <param name="options">Control limits and behaviors of the attempts.</param>
    /// <returns>The <typeparamref name="T"/> return value from the first successful attempt of <paramref name="func"/> that does not throw an exception.</returns>
    /// <exception cref="TaskCanceledException">If the <see cref="Options.CancellationToken"/> is cancelled.</exception>
    /// <exception cref="Exception">Any exception thrown by <paramref name="func"/> on its final attempt.</exception>
    public static T Attempt<T>(Func<int, T> func, Options options = default) {
        Stopwatch  totalDuration = new();
        Exception? lastException = null;
        int        attempt;
        for (attempt = 0; ShouldLoop(attempt, totalDuration, options); attempt++) {
            try {
                totalDuration.Start();
                return func.Invoke(attempt);
            } catch (Exception e) when (e is not OutOfMemoryException) {
                lastException = e;
                options.AfterFailure?.Invoke(attempt, e);

                TimeSpan delay = GetDelay(options.Delay, attempt);

                if (!ShouldRetry(e, options) || options.IsDurationExceeded(totalDuration.Elapsed + delay)) throw;

                if (delay > TimeSpan.Zero) {
                    Task.Delay(delay, options.CancellationToken).GetAwaiter().GetResult();
                }

                options.BeforeRetry?.Invoke(attempt + 1, e);
            }
        }

        if (options.CancellationToken.IsCancellationRequested) {
            throw new TaskCanceledException("Remaining Retrier attempts were cancelled");
        } else if (options.IsDurationExceeded(totalDuration.Elapsed)) {
            throw lastException!;
        }

        try {
            return func.Invoke(attempt);
        } catch (Exception e) when (e is not OutOfMemoryException) {
            options.AfterFailure?.Invoke(attempt, e);
            throw;
        }
    }

    /// <summary>
    /// Run the given async <paramref name="func"/> until it returns without throwing an exception.
    /// </summary>
    /// <param name="func">An asynchronous function which is prone to sometimes throw exceptions. The <see cref="int"/> argument is the number of attempts, starting from <c>0</c> for the initial attempt.</param>
    /// <param name="options">Control limits and behaviors of the attempts.</param>
    /// <returns>The <see cref="Task"/> return value from the first successful attempt of <paramref name="func"/> that does not throw an exception.</returns>
    /// <exception cref="TaskCanceledException">If the <see cref="Options.CancellationToken"/> is cancelled.</exception>
    /// <exception cref="Exception">Any exception thrown by <paramref name="func"/> on its final attempt.</exception>
    public static async Task Attempt(Func<int, Task> func, Options options = default) {
        Stopwatch  totalDuration = new();
        Exception? lastException = null;
        int        attempt;
        for (attempt = 0; ShouldLoop(attempt, totalDuration, options); attempt++) {
            try {
                totalDuration.Start();
                await func.Invoke(attempt).ConfigureAwait(false);
                return;
            } catch (Exception e) when (e is not OutOfMemoryException) {
                lastException = e;
                options.AfterFailure?.Invoke(attempt, e);

                TimeSpan delay = GetDelay(options.Delay, attempt);

                if (!ShouldRetry(e, options) || options.IsDurationExceeded(totalDuration.Elapsed + delay)) throw;

                if (delay > TimeSpan.Zero) {
                    await Task.Delay(delay, options.CancellationToken).ConfigureAwait(false);
                }

                options.BeforeRetry?.Invoke(attempt + 1, e);
            }
        }

        if (options.CancellationToken.IsCancellationRequested) {
            throw new TaskCanceledException("Remaining Retrier attempts were cancelled");
        } else if (options.IsDurationExceeded(totalDuration.Elapsed)) {
            throw lastException!;
        }

        try {
            await func.Invoke(attempt).ConfigureAwait(false);
        } catch (Exception e) when (e is not OutOfMemoryException) {
            options.AfterFailure?.Invoke(attempt, e);
            throw;
        }
    }

    /// <summary>
    /// Run the given async <paramref name="func"/> until it returns without throwing an exception.
    /// </summary>
    /// <param name="func">An asynchronous function which is prone to sometimes throw exceptions. The <see cref="int"/> argument is the number of attempts, starting from <c>0</c> for the initial attempt.</param>
    /// <param name="options">Control limits and behaviors of the attempts.</param>
    /// <returns>The <typeparamref name="T"/> return value from the first successful attempt of <paramref name="func"/> that does not throw an exception.</returns>
    /// <exception cref="TaskCanceledException">If the <see cref="Options.CancellationToken"/> is cancelled.</exception>
    /// <exception cref="Exception">Any exception thrown by <paramref name="func"/> on its final attempt.</exception>
    public static async Task<T> Attempt<T>(Func<int, Task<T>> func, Options options = default) {
        Stopwatch  totalDuration = new();
        Exception? lastException = null;
        int        attempt;
        for (attempt = 0; ShouldLoop(attempt, totalDuration, options); attempt++) {
            try {
                totalDuration.Start();
                return await func.Invoke(attempt).ConfigureAwait(false);
            } catch (Exception e) when (e is not OutOfMemoryException) {
                lastException = e;
                options.AfterFailure?.Invoke(attempt, e);

                TimeSpan delay = GetDelay(options.Delay, attempt);

                if (!ShouldRetry(e, options) || options.IsDurationExceeded(totalDuration.Elapsed + delay)) throw;

                if (delay > TimeSpan.Zero) {
                    await Task.Delay(delay, options.CancellationToken).ConfigureAwait(false);
                }

                options.BeforeRetry?.Invoke(attempt + 1, e);
            }
        }

        if (options.CancellationToken.IsCancellationRequested) {
            throw new TaskCanceledException("Remaining Retrier attempts were cancelled");
        } else if (options.IsDurationExceeded(totalDuration.Elapsed)) {
            throw lastException!;
        }

        try {
            return await func.Invoke(attempt).ConfigureAwait(false);
        } catch (Exception e) when (e is not OutOfMemoryException) {
            options.AfterFailure?.Invoke(attempt, e);
            throw;
        }
    }

    private static TimeSpan GetDelay(Func<int, TimeSpan>? delay, int attempt) => delay?.Invoke(attempt) switch {
        { } duration when duration <= TimeSpan.Zero => TimeSpan.Zero,
        { } duration when duration > MaxDelay       => MaxDelay,
        { } duration                                => duration,
        null                                        => TimeSpan.Zero
    };

    private static bool ShouldRetry(Exception exception, Options options) => options.IsRetryAllowed?.Invoke(exception) ?? true;

    private static bool ShouldLoop(int attempt, Stopwatch totalDuration, Options options) =>
        (options.MaxAttempts == null || attempt < options.MaxAttempts - 1) &&
        (attempt == 0 || !options.IsDurationExceeded(totalDuration.Elapsed)) &&
        !options.CancellationToken.IsCancellationRequested;

    /// <summary>
    /// Control the limits and behaviors of <see cref="Retrier"/>.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    public record struct Options() {

        /// <summary>The total number of times the function is allowed to run in this invocation, including the first attempt. Equal to 1 initial attempt plus at most <c>MaxAttempts - 1</c> retries if it throws an exception. Must be at least 1, if you set this to 0 it will clip to 1. Defaults to <c>null</c>, which means infinite retries. If both <see cref="MaxAttempts"/> and <see cref="MaxOverallDuration"/> are specified, they will both be applied, so it will stop retrying after either enough attempts OR enough time has passed.</summary>
        public int? MaxAttempts {
            get;
            set => field = value is < 1 ? 1 : value;
        }

        /// <summary>Limits the attempts by total time, measured from the start of the first attempt to the end of the last attempt. Any attempts that would start after this duration has ended will not be run, and the most recent attempt exception will be thrown. Defaults to <c>null</c>, which means no upper limit on overall duration. If both <see cref="MaxAttempts"/> and <see cref="MaxOverallDuration"/> are specified, they will both be applied, so it will stop retrying after either enough attempts OR enough time has passed. This does not limit the duration of one individual attempt (which is unbounded), or the amount of waiting time between attempts (see <see cref="Delay"/>), but rather the cumulative time allowed for all attempts together, from calling <c>Retrier.Attempt</c> to it returning or throwing. If you set this to a non-positive <see cref="TimeSpan"/>, it will make exactly one attempt.</summary>
        public TimeSpan? MaxOverallDuration {
            get;
            set => field = value switch {
                null                                                   => null,
                { } infinite when infinite == Timeout.InfiniteTimeSpan => null,
                { } negative when negative < TimeSpan.Zero             => TimeSpan.Zero,
                _                                                      => value
            };
        }

        /// <summary>How long to wait between attempts. Defaults to <c>null</c>, which means no delay. This is a function of how many retries have been attempted (starting from <c>0</c>), to allow for strategies such as exponential back-off. Return values outside the range <c>[0, int.MaxValue]</c> ms will be clipped (<c>[0, uint.MaxValue-1]</c> ms starting in .NET 6).</summary>
        public Func<int, TimeSpan>? Delay { get; set; }

        /// <summary>Allows certain exceptions that indicate permanent failures to not trigger retries. For example, <see cref="ArgumentOutOfRangeException"/> will usually be thrown every time you call a function with the same arguments, so there is no reason to retry, and your <see cref="IsRetryAllowed"/> function could return <c>false</c> in that case. Defaults to retrying on every exception except <see cref="OutOfMemoryException"/>.</summary>
        public Func<Exception, bool>? IsRetryAllowed { get; set; }

        /// <summary>Action to run between attempts, possibly to log the previous failure. Runs before any <see cref="Delay"/>. Defaults to no action. Takes as parameters the attempt number of the most recent failed attempt (starting from <c>0</c>) and the most recent <see cref="Exception"/> thrown. Does not run after successful attempts, but does run after the final attempt when <see cref="MaxAttempts"/> is exhausted and the final attempt fails.</summary>
        public Action<int, Exception>? AfterFailure { get; set; }

        /// <summary>Action to run between attempts, possibly to clean up some state before the next retry. For example, you may want to disconnect a failed connection before reconnecting. Runs after any <see cref="Delay"/>. Defaults to no action. Takes as parameters the attempt number of the retry that is about to run (starting from <c>1</c>) and the most recent <see cref="Exception"/> thrown. Does not run before the initial attempt.</summary>
        public Action<int, Exception>? BeforeRetry { get; set; }

        /// <summary>Allows you to cancel remaining attempts and delays.</summary>
        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;

        internal bool IsDurationExceeded(TimeSpan elapsed) => MaxOverallDuration < elapsed;

    }

    /// <summary>
    /// Built-in implementations of different backoff strategies, which can be passed to the <c>delay</c> parameter of <c>Retrier.Attempt</c>.
    /// </summary>
    /// <remarks>To visualize and play with different strategies and values, check out <see href="https://dotnetfiddle.net/CxBCrK"/>.</remarks>
    public static class Delays {

        private static TimeSpan Clip(Func<TimeSpan> calculateDelay, TimeSpan max) {
            try {
                TimeSpan delay = calculateDelay();
                return delay < TimeSpan.Zero ? TimeSpan.Zero : max != TimeSpan.Zero && delay > max ? max : delay;
            } catch (OverflowException) {
                return max != TimeSpan.Zero ? max : TimeSpan.MaxValue;
            }
        }

        /// <summary>
        /// Always wait a fixed duration before each retry.
        /// </summary>
        /// <remarks>To visualize and play with different strategies and values, check out <see href="https://dotnetfiddle.net/CxBCrK"/>.</remarks>
        /// <param name="delay">How long to wait before the next retry</param>
        /// <returns>A function that can be assigned to <see cref="Options.Delay"/></returns>
        public static Func<int, TimeSpan> Constant(TimeSpan delay) => _ => delay;

        /// <inheritdoc cref="Constant(System.TimeSpan)" />
        public static Func<int, TimeSpan> Constant(int delayMilliseconds) => Constant(TimeSpan.FromMilliseconds(delayMilliseconds));

        /// <summary>
        /// Wait a linearly-increasing duration before each retry (<c>delay = coefficient * retry + initial</c>). The initial value of <c>retry</c> is 0.
        /// </summary>
        /// <remarks>To visualize and play with different strategies and values, check out <see href="https://dotnetfiddle.net/CxBCrK"/>.</remarks>
        /// <param name="coefficient">A duration to multiply by the next retry's number (starting from 0) to get a duration for this retry delay</param>
        /// <param name="offset">A constant duration to add</param>
        /// <param name="max">An upper limit above which the duration will not increase (the line becomes horizontal at this y-position)</param>
        /// <returns>A function that can be assigned to <see cref="Options.Delay"/></returns>
        public static Func<int, TimeSpan> Linear(TimeSpan coefficient, TimeSpan offset = default, TimeSpan max = default) => retry => Clip(() => {
            checked {
                long ticks = coefficient.Ticks * retry;
                return new TimeSpan(ticks) + offset;
            }
        }, max);

        /// <summary>
        /// Wait an exponentially-increasing duration before each retry (<c>delay = coefficient * retry^power + initial</c>). The initial value of <c>retry</c> is 0.
        /// </summary>
        /// <remarks>To visualize and play with different strategies and values, check out <see href="https://dotnetfiddle.net/CxBCrK"/>.</remarks>
        /// <param name="coefficient">A duration to multiply by the exponential value to get a duration for this retry delay </param>
        /// <param name="power">The power to which the retry number (starting from 0) will be raised. Defaults to <c>2</c> to square it.</param>
        /// <param name="offset">A constant duration to add</param>
        /// <param name="max">An upper limit above which the duration will not increase (the line becomes horizontal at this y-position)</param>
        /// <returns>A function that can be assigned to <see cref="Options.Delay"/></returns>
        public static Func<int, TimeSpan> Exponential(TimeSpan coefficient, double power = 2, TimeSpan offset = default, TimeSpan max = default) => retry => Clip(() => {
            checked {
                long ticks = (long) (coefficient.Ticks * Math.Pow(retry, power));
                return new TimeSpan(ticks) + offset;
            }
        }, max);

        /// <summary>
        /// Wait a duration at increases with a power series before each retry (<c>delay = coefficient * base^retry + initial</c>). The initial value of <c>retry</c> is 0.
        /// </summary>
        /// <remarks>To visualize and play with different strategies and values, check out <see href="https://dotnetfiddle.net/CxBCrK"/>.</remarks>
        /// <param name="coefficient">A duration to multiply by the exponential value to get a duration for this retry delay</param>
        /// <param name="base">A number to raise to the power of the number of retries that have already been attempted (starting from 0). Defaults to <c>2</c> for a <c>2^x</c> power series.</param>
        /// <param name="offset">A constant duration to add</param>
        /// <param name="max">An upper limit above which the duration will not increase (the line becomes horizontal at this y-position)</param>
        /// <returns>A function that can be assigned to <see cref="Options.Delay"/></returns>
        public static Func<int, TimeSpan> Power(TimeSpan coefficient, double @base = 2, TimeSpan offset = default, TimeSpan max = default) => retry => Clip(() => {
            checked {
                long ticks = retry == 0 ? 0 : (long) (coefficient.Ticks * Math.Pow(@base, retry));
                return new TimeSpan(ticks) + offset;
            }
        }, max);

        /// <summary>
        /// Wait a logarithmically-increasing duration before each retry (<c>delay = coefficient * (log(retry, base) + 1) + offset</c>). The initial value of <c>retry</c> is 0, which results in a delay of <paramref name="offset"/> because the logarithm would be negative infinity otherwise.
        /// </summary>
        /// <remarks>To visualize and play with different strategies and values, check out <see href="https://dotnetfiddle.net/CxBCrK"/>.</remarks>
        /// <param name="coefficient">A duration to multiply by the logarithm to get a duration for this retry delay</param>
        /// <param name="base">The logarithmic base. Defaults to 10 for a base-10 logarithm when omitted or less than 1.</param>
        /// <param name="offset">A constant duration to add</param>
        /// <param name="max">An upper limit above which the duration will not increase (the line becomes horizontal at this y-position)</param>
        /// <returns>A function that can be assigned to <see cref="Options.Delay"/></returns>
        public static Func<int, TimeSpan> Logarithm(TimeSpan coefficient, double @base = 10, TimeSpan offset = default, TimeSpan max = default) => retry => Clip(() => {
            checked {
                @base = @base > 1 ? @base : 10;
                long ticks = retry == 0 ? 0 : (long) (coefficient.Ticks * (Math.Log(retry, @base) + 1));
                return new TimeSpan(ticks) + offset;
            }
        }, max);

        /// <summary>
        /// Wait a random duration before each retry.
        /// </summary>
        /// <remarks>To visualize and play with different strategies and values, check out <see href="https://dotnetfiddle.net/CxBCrK"/>.</remarks>
        /// <param name="max">The upper limit of the duration to wait</param>
        /// <param name="min">The lower limit of the duration to wait. Defaults to 0 seconds.</param>
        /// <returns>A function that can be passed to the <c>delay</c> parameter of <c>Retrier.Attempt</c></returns>
        public static Func<int, TimeSpan> MonteCarlo(TimeSpan max, TimeSpan min = default) {
            Random random = new();
            long   range  = (max - min).Ticks;
            return _ => (range <= int.MaxValue ? new TimeSpan(random.Next((int) range)) : new TimeSpan((long) (random.NextDouble() * range))) + min;
        }

    }

}