#nullable enable

using System;
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
    /// Run the given <paramref name="action"/> at most <paramref name="maxAttempts"/> times, until it returns without throwing an exception.
    /// </summary>
    /// <param name="action">An action which is prone to sometimes throw exceptions. The <see cref="int"/> argument is the number of attempts, starting from <c>0</c> for the initial attempt.</param>
    /// <param name="maxAttempts">The total number of times <paramref name="action"/> is allowed to run in this invocation, equal to <c>1</c> initial attempt plus up to <c>maxAttempts - 1</c> retries if it throws an exception. Must be at least 1, if you pass 0 it will clip to 1. Defaults to 2. For infinite retries, pass <see langword="null"/>.</param>
    /// <param name="delay">How long to wait between attempts. Defaults to <see langword="null"/>, which means no delay. This is a function of how many retries have been attempted (starting from <c>0</c>), to allow for strategies such as exponential back-off. Return values outside the range <c>[0, int.MaxValue]</c> ms will be clipped (<c>[0, uint.MaxValue-1]</c> ms starting in .NET 6).</param>
    /// <param name="isRetryAllowed">Allows certain exceptions that indicate permanent failures to not trigger retries. For example, <see cref="ArgumentOutOfRangeException"/> will usually be thrown every time you call a function with the same arguments, so there is no reason to retry, and <paramref name="isRetryAllowed"/> could return <c>false</c> in that case. Defaults to retrying on every exception besides <see cref="OutOfMemoryException"/>.</param>
    /// <param name="beforeRetry">Action to run between attempts, possibly to clean up some state before the next retry. For example, you may want to disconnect a failed connection before reconnecting. Runs after any <paramref name="delay"/>. Defaults to no action. Takes as parameters the number of retries attempted (starting from <c>0</c>) and the most recent <see cref="Exception"/> thrown.</param>
    /// <param name="cancellationToken">Allows you to cancel remaining attempts and delays.</param>
    /// <exception cref="TaskCanceledException">If the <paramref name="cancellationToken"/> is cancelled.</exception>
    /// <exception cref="Exception">Any exception thrown by <paramref name="action"/> on its final attempt.</exception>
    public static void Attempt(Action<int> action,
                               int? maxAttempts = 2,
                               Func<int, TimeSpan>? delay = null,
                               Func<Exception, bool>? isRetryAllowed = null,
                               Action<int, Exception>? beforeRetry = null,
                               CancellationToken cancellationToken = default) {
        int attempt;
        for (attempt = 0; (!maxAttempts.HasValue || attempt < maxAttempts - 1) && !cancellationToken.IsCancellationRequested; attempt++) {
            try {
                action.Invoke(attempt);
                return;
            } catch (Exception e) when (e is not OutOfMemoryException) {
                // This brace-less syntax convinces dotCover that the "throw;" statement is fully covered.
                if (!isRetryAllowed?.Invoke(e) ?? false) throw;

                if (GetDelay(delay, attempt) is { } duration) {
                    Task.Delay(duration, cancellationToken).GetAwaiter().GetResult();
                }

                beforeRetry?.Invoke(attempt, e);
            }
        }

        if (!cancellationToken.IsCancellationRequested) {
            action.Invoke(attempt);
        } else {
            throw new TaskCanceledException("Remaining Retrier attempts were cancelled");
        }
    }

    /// <summary>
    /// Run the given <paramref name="func"/> at most <paramref name="maxAttempts"/> times, until it returns without throwing an exception.
    /// </summary>
    /// <param name="func">An action which is prone to sometimes throw exceptions. The <see cref="int"/> argument is the number of attempts, starting from <c>0</c> for the initial attempt.</param>
    /// <param name="maxAttempts">The total number of times <paramref name="func"/> is allowed to run in this invocation, equal to <c>1</c> initial attempt plus up to <c>maxAttempts - 1</c> retries if it throws an exception. Must be at least 1, if you pass 0 it will clip to 1. Defaults to 2. For infinite retries, pass <see langword="null"/>.</param>
    /// <param name="delay">How long to wait between attempts. Defaults to <see langword="null"/>, which means no delay. This is a function of how many retries have been attempted (starting from <c>0</c>), to allow for strategies such as exponential back-off. Return values outside the range <c>[0, int.MaxValue]</c> ms will be clipped (<c>[0, uint.MaxValue-1]</c> ms starting in .NET 6).</param>
    /// <param name="isRetryAllowed">Allows certain exceptions that indicate permanent failures to not trigger retries. For example, <see cref="ArgumentOutOfRangeException"/> will usually be thrown every time you call a function with the same arguments, so there is no reason to retry, and <paramref name="isRetryAllowed"/> could return <c>false</c> in that case. Defaults to retrying on every exception besides <see cref="OutOfMemoryException"/>.</param>
    /// <param name="beforeRetry">Action to run between attempts, possibly to clean up some state before the next retry. For example, you may want to disconnect a failed connection before reconnecting. Runs after any <paramref name="delay"/>. Defaults to no action. Takes as parameters the number of retries attempted (starting from <c>0</c>) and the most recent <see cref="Exception"/> thrown.</param>
    /// <param name="cancellationToken">Allows you to cancel remaining attempts and delays.</param>
    /// <exception cref="TaskCanceledException">If the <paramref name="cancellationToken"/> is cancelled.</exception>
    /// <exception cref="Exception">Any exception thrown by <paramref name="func"/> on its final attempt.</exception>
    public static T Attempt<T>(Func<int, T> func,
                               int? maxAttempts = 2,
                               Func<int, TimeSpan>? delay = null,
                               Func<Exception, bool>? isRetryAllowed = null,
                               Action<int, Exception>? beforeRetry = null,
                               CancellationToken cancellationToken = default) {
        int attempt;
        for (attempt = 0; (!maxAttempts.HasValue || attempt < maxAttempts - 1) && !cancellationToken.IsCancellationRequested; attempt++) {
            try {
                return func.Invoke(attempt);
            } catch (Exception e) when (e is not OutOfMemoryException) {
                if (!isRetryAllowed?.Invoke(e) ?? false) throw;

                if (GetDelay(delay, attempt) is { } duration) {
                    Task.Delay(duration, cancellationToken).GetAwaiter().GetResult();
                }

                beforeRetry?.Invoke(attempt, e);
            }
        }

        if (!cancellationToken.IsCancellationRequested) {
            return func.Invoke(attempt);
        } else {
            throw new TaskCanceledException("Remaining Retrier attempts were cancelled");
        }
    }

    /// <summary>
    /// Run the given <paramref name="func"/> at most <paramref name="maxAttempts"/> times, until it returns without throwing an exception.
    /// </summary>
    /// <param name="func">An action which is prone to sometimes throw exceptions. The <see cref="int"/> argument is the number of attempts, starting from <c>0</c> for the initial attempt.</param>
    /// <param name="maxAttempts">The total number of times <paramref name="func"/> is allowed to run in this invocation, equal to <c>1</c> initial attempt plus up to <c>maxAttempts - 1</c> retries if it throws an exception. Must be at least 1, if you pass 0 it will clip to 1. Defaults to 2. For infinite retries, pass <see langword="null"/>.</param>
    /// <param name="delay">How long to wait between attempts. Defaults to <see langword="null"/>, which means no delay. This is a function of how many retries have been attempted (starting from <c>0</c>), to allow for strategies such as exponential back-off. Return values outside the range <c>[0, int.MaxValue]</c> ms will be clipped (<c>[0, uint.MaxValue-1]</c> ms starting in .NET 6).</param>
    /// <param name="isRetryAllowed">Allows certain exceptions that indicate permanent failures to not trigger retries. For example, <see cref="ArgumentOutOfRangeException"/> will usually be thrown every time you call a function with the same arguments, so there is no reason to retry, and <paramref name="isRetryAllowed"/> could return <c>false</c> in that case. Defaults to retrying on every exception besides <see cref="OutOfMemoryException"/>.</param>
    /// <param name="beforeRetry">Action to run between attempts, possibly to clean up some state before the next retry. For example, you may want to disconnect a failed connection before reconnecting. Runs after any <paramref name="delay"/>. Defaults to no action. Takes as parameters the number of retries attempted (starting from <c>0</c>) and the most recent <see cref="Exception"/> thrown.</param>
    /// <param name="cancellationToken">Allows you to cancel remaining attempts and delays.</param>
    /// <exception cref="TaskCanceledException">If the <paramref name="cancellationToken"/> is cancelled.</exception>
    /// <exception cref="Exception">Any exception thrown by <paramref name="func"/> on its final attempt.</exception>
    public static async Task Attempt(Func<int, Task> func,
                                     int? maxAttempts = 2,
                                     Func<int, TimeSpan>? delay = null,
                                     Func<Exception, bool>? isRetryAllowed = null,
                                     Func<int, Exception, Task>? beforeRetry = null,
                                     CancellationToken cancellationToken = default) {
        int attempt;
        for (attempt = 0; (!maxAttempts.HasValue || attempt < maxAttempts - 1) && !cancellationToken.IsCancellationRequested; attempt++) {
            try {
                await func.Invoke(attempt).ConfigureAwait(false);
                return;
            } catch (Exception e) when (e is not OutOfMemoryException) {
                if (!isRetryAllowed?.Invoke(e) ?? false) throw;

                if (GetDelay(delay, attempt) is { } duration) {
                    await Task.Delay(duration, cancellationToken).ConfigureAwait(false);
                }

                beforeRetry?.Invoke(attempt, e);
            }
        }

        if (!cancellationToken.IsCancellationRequested) {
            await func.Invoke(attempt).ConfigureAwait(false);
        } else {
            throw new TaskCanceledException("Remaining Retrier attempts were cancelled");
        }
    }

    /// <summary>
    /// Run the given <paramref name="func"/> at most <paramref name="maxAttempts"/> times, until it returns without throwing an exception.
    /// </summary>
    /// <param name="func">An action which is prone to sometimes throw exceptions. The <see cref="int"/> argument is the number of attempts, starting from <c>0</c> for the initial attempt.</param>
    /// <param name="maxAttempts">The total number of times <paramref name="func"/> is allowed to run in this invocation, equal to <c>1</c> initial attempt plus up to <c>maxAttempts - 1</c> retries if it throws an exception. Must be at least 1, if you pass 0 it will clip to 1. Defaults to 2. For infinite retries, pass <see langword="null"/>.</param>
    /// <param name="delay">How long to wait between attempts. Defaults to <see langword="null"/>, which means no delay. This is a function of how many retries have been attempted (starting from <c>0</c>), to allow for strategies such as exponential back-off. Return values outside the range <c>[0, int.MaxValue]</c> ms will be clipped (<c>[0, uint.MaxValue-1]</c> ms starting in .NET 6).</param>
    /// <param name="isRetryAllowed">Allows certain exceptions that indicate permanent failures to not trigger retries. For example, <see cref="ArgumentOutOfRangeException"/> will usually be thrown every time you call a function with the same arguments, so there is no reason to retry, and <paramref name="isRetryAllowed"/> could return <c>false</c> in that case. Defaults to retrying on every exception besides <see cref="OutOfMemoryException"/>.</param>
    /// <param name="beforeRetry">Action to run between attempts, possibly to clean up some state before the next retry. For example, you may want to disconnect a failed connection before reconnecting. Runs after any <paramref name="delay"/>. Defaults to no action. Takes as parameters the number of retries attempted (starting from <c>0</c>) and the most recent <see cref="Exception"/> thrown.</param>
    /// <param name="cancellationToken">Allows you to cancel remaining attempts and delays.</param>
    /// <exception cref="TaskCanceledException">If the <paramref name="cancellationToken"/> is cancelled.</exception>
    /// <exception cref="Exception">Any exception thrown by <paramref name="func"/> on its final attempt.</exception>
    public static async Task<T> Attempt<T>(Func<int, Task<T>> func,
                                           int? maxAttempts = 2,
                                           Func<int, TimeSpan>? delay = null,
                                           Func<Exception, bool>? isRetryAllowed = null,
                                           Func<int, Exception, Task>? beforeRetry = null,
                                           CancellationToken cancellationToken = default) {
        int attempt;
        for (attempt = 0; (!maxAttempts.HasValue || attempt < maxAttempts - 1) && !cancellationToken.IsCancellationRequested; attempt++) {
            try {
                return await func.Invoke(attempt).ConfigureAwait(false);
            } catch (Exception e) when (e is not OutOfMemoryException) {
                if (!isRetryAllowed?.Invoke(e) ?? false) throw;

                if (GetDelay(delay, attempt) is { } duration) {
                    await Task.Delay(duration, cancellationToken).ConfigureAwait(false);
                }

                beforeRetry?.Invoke(attempt, e);
            }
        }

        if (!cancellationToken.IsCancellationRequested) {
            return await func.Invoke(attempt).ConfigureAwait(false);
        } else {
            throw new TaskCanceledException("Remaining Retrier attempts were cancelled");
        }
    }

    private static TimeSpan? GetDelay(Func<int, TimeSpan>? delay, int attempt) => delay?.Invoke(attempt) switch {
        { } duration when duration <= TimeSpan.Zero => null,
        { } duration when duration > MaxDelay       => MaxDelay,
        { } duration                                => duration,
        null                                        => null
    };

    /// <summary>
    /// Built-in implementations of different backoff strategies, which can be passed to the <c>beforeRetry</c> parameter of <c>Retrier.Attempt</c>.
    /// </summary>
    public static class Delays {

        private static TimeSpan Clip(Func<TimeSpan> calculateDelay, TimeSpan limit) {
            try {
                TimeSpan delay = calculateDelay();
                return limit != TimeSpan.Zero && delay > limit ? limit : delay;
            } catch (OverflowException) {
                return limit != TimeSpan.Zero ? limit : TimeSpan.MaxValue;
            }
        }

        public static Func<int, TimeSpan> Constant(TimeSpan delay) => _ => delay;

        public static Func<int, TimeSpan> Constant(int delayMilliseconds) => Constant(TimeSpan.FromMilliseconds(delayMilliseconds));

        public static Func<int, TimeSpan> Linear(TimeSpan coefficient, TimeSpan initial = default, TimeSpan limit = default) => attempt => Clip(() => {
            checked {
                long ticks = coefficient.Ticks * attempt;
                return new TimeSpan(ticks) + initial;
            }
        }, limit);

        public static Func<int, TimeSpan> Exponential(TimeSpan coefficient, double power = 2, TimeSpan initial = default, TimeSpan limit = default) => attempt => Clip(() => {
            checked {
                long ticks = (long) (coefficient.Ticks * Math.Pow(attempt, power));
                return new TimeSpan(ticks) + initial;
            }
        }, limit);

        public static Func<int, TimeSpan> Power(TimeSpan coefficient, double @base = 2, TimeSpan initial = default, TimeSpan limit = default) => attempt => Clip(() => {
            checked {
                long ticks = attempt == 0 ? 0 : (long) (coefficient.Ticks * Math.Pow(@base, attempt));
                return new TimeSpan(ticks) + initial;
            }
        }, limit);

        public static Func<int, TimeSpan> Logarithm(TimeSpan coefficient, double @base = 10, TimeSpan initial = default, TimeSpan limit = default) => attempt => Clip(() => {
            checked {
                @base = @base > 1 ? @base : 10;
                long ticks = attempt == 0 ? 0 : (long) (coefficient.Ticks * (Math.Log(attempt, @base) + 1));
                return new TimeSpan(ticks) + initial;
            }
        }, limit);

        public static Func<int, TimeSpan> MonteCarlo(TimeSpan max, TimeSpan min = default) {
            Random random = new();
            long   range  = (max - min).Ticks;
            return _ => (range <= int.MaxValue ? new TimeSpan(random.Next((int) range)) : new TimeSpan((long) (random.NextDouble() * range))) + min;
        }

    }

}