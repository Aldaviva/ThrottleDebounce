#nullable enable

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ThrottleDebounce.Retry;

/// <summary>
/// Run a delegate with retries if it throws an exception.
/// </summary>
public static class Retrier {

    private const string CancelledMessage = "Remaining Retrier attempts were cancelled";

    // ExceptionAdjustment: M:System.TimeSpan.FromMilliseconds(System.Double) -T:System.OverflowException
    // These are the specific maximum values that don't overflow, tailored for the runtime version.
    private static readonly TimeSpan MaxDelay = TimeSpan.FromMilliseconds(Environment.Version.Major >= 6 ? uint.MaxValue - 1 : int.MaxValue);

    internal static readonly Task CompletedTask =
#if NETSTANDARD1_3_OR_GREATER
        Task.CompletedTask;
#else
        Task.WhenAll();
#endif

    // Task<T> Retrier.Attempt<T>(Func<long, Task<T>>, IAsyncOptions?)
    private static readonly Lazy<MethodInfo> AttemptAsyncWithReturnValue = new(() => typeof(Retrier).GetMethods(BindingFlags.Public | BindingFlags.Static).First(method =>
        method.Name == nameof(Attempt) && method.ReturnType is { IsGenericType: true } returnType && returnType.GetGenericTypeDefinition() == typeof(Task<>)), LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Run the given <paramref name="action"/> until it returns without throwing an exception.
    /// </summary>
    /// <param name="action">An action which is prone to sometimes throw exceptions. The <see cref="long"/> argument is the number of attempts, starting from <c>0</c> for the initial attempt.</param>
    /// <param name="options">Control limits and behaviors of the attempts.</param>
    /// <exception cref="TaskCanceledException">If the <see cref="RetryOptions.CancellationToken"/> is cancelled.</exception>
    /// <exception cref="Exception">Any exception thrown by <paramref name="action"/> on its final attempt.</exception>
    public static void Attempt(Action<long> action, RetryOptions options = default) {
        Stopwatch  totalDuration = new();
        Exception? lastException = null;
        long       attempt;
        for (attempt = 0; ShouldLoop(attempt, totalDuration, options); attempt++) {
            try {
                totalDuration.Start();
                action(attempt);
                return;
            } catch (Exception e) when (e is not OutOfMemoryException) {
                lastException = e;
                options.AfterFailure?.Invoke(e, attempt);

                TimeSpan delay = GetDelay(options.Delay, attempt);

                // This brace-less syntax convinces dotCover that the "throw;" statement is fully covered.
                if (!ShouldRetry(e, attempt, options) || IsDurationExceeded(totalDuration.Elapsed + delay, options)) throw;

                if (delay > TimeSpan.Zero) {
                    // Use Task.Delay because Thread.Sleep does not allow cancellation.
                    // Synchronously blocking on a Task.Delay never deadlocks, even without ConfigureAwait(false).
                    Task.Delay(delay, options.CancellationToken).GetAwaiter().GetResult();
                }

                options.BeforeRetry?.Invoke(e, attempt + 1);
            }
        }

        if (options.CancellationToken.IsCancellationRequested) {
            throw new TaskCanceledException(CancelledMessage);
        } else if (IsDurationExceeded(totalDuration.Elapsed, options)) {
            throw lastException!;
        }

        try {
            action(attempt);
        } catch (Exception e) when (e is not OutOfMemoryException) {
            options.AfterFailure?.Invoke(e, attempt);
            throw;
        }
    }

    /// <summary>
    /// Run the given <paramref name="func"/> until it returns without throwing an exception.
    /// </summary>
    /// <param name="func">A function which is prone to sometimes throw exceptions. The <see cref="long"/> argument is the number of attempts, starting from <c>0</c> for the initial attempt.</param>
    /// <param name="options">Control limits and behaviors of the attempts.</param>
    /// <returns>The <typeparamref name="T"/> return value from the first successful attempt of <paramref name="func"/> that does not throw an exception.</returns>
    /// <exception cref="TaskCanceledException">If the <see cref="RetryOptions.CancellationToken"/> is cancelled.</exception>
    /// <exception cref="Exception">Any exception thrown by <paramref name="func"/> on its final attempt.</exception>
    public static T Attempt<T>(Func<long, T> func, RetryOptions options = default) {
        Type typeofT = typeof(T);
        if (typeofT == typeof(Task)) { // Prevent confusing bugs caused by compiler picking the wrong method overload when T is a Task or Task<T>
            return (T) (object) Attempt((Func<long, Task>) (object) func, (IAsyncRetryOptions) options);
        } else if (typeofT.IsGenericType && typeofT.GetGenericTypeDefinition() == typeof(Task<>)) {
            return (T) AttemptAsyncWithReturnValue.Value.MakeGenericMethod(typeofT.GenericTypeArguments[0]).Invoke(null, [func, options]);
        }

        Stopwatch  totalDuration = new();
        Exception? lastException = null;
        long       attempt;
        for (attempt = 0; ShouldLoop(attempt, totalDuration, options); attempt++) {
            try {
                totalDuration.Start();
                return func(attempt);
            } catch (Exception e) when (e is not OutOfMemoryException) {
                lastException = e;
                options.AfterFailure?.Invoke(e, attempt);

                TimeSpan delay = GetDelay(options.Delay, attempt);

                if (!ShouldRetry(e, attempt, options) || IsDurationExceeded(totalDuration.Elapsed + delay, options)) throw;

                if (delay > TimeSpan.Zero) {
                    Task.Delay(delay, options.CancellationToken).GetAwaiter().GetResult();
                }

                options.BeforeRetry?.Invoke(e, attempt + 1);
            }
        }

        if (options.CancellationToken.IsCancellationRequested) {
            throw new TaskCanceledException(CancelledMessage);
        } else if (IsDurationExceeded(totalDuration.Elapsed, options)) {
            throw lastException!;
        }

        try {
            return func(attempt);
        } catch (Exception e) when (e is not OutOfMemoryException) {
            options.AfterFailure?.Invoke(e, attempt);
            throw;
        }
    }

    /// <summary>
    /// Run the given async <paramref name="func"/> until it returns without throwing an exception.
    /// </summary>
    /// <param name="func">An asynchronous function which is prone to sometimes throw exceptions. The <see cref="long"/> argument is the number of attempts, starting from <c>0</c> for the initial attempt.</param>
    /// <param name="options">Control limits and behaviors of the attempts.</param>
    /// <returns>The <see cref="Task"/> return value from the first successful attempt of <paramref name="func"/> that does not throw an exception.</returns>
    /// <exception cref="TaskCanceledException">If the <see cref="RetryOptions.CancellationToken"/> is cancelled.</exception>
    /// <exception cref="Exception">Any exception thrown by <paramref name="func"/> on its final attempt.</exception>
    public static async Task Attempt(Func<long, Task> func, IAsyncRetryOptions? options = null) {
        Stopwatch  totalDuration = new();
        Exception? lastException = null;
        options ??= new AsyncRetryOptions();
        long attempt;
        for (attempt = 0; ShouldLoop(attempt, totalDuration, options); attempt++) {
            try {
                totalDuration.Start();
                await func(attempt).ConfigureAwait(false);
                return;
            } catch (Exception e) when (e is not OutOfMemoryException) {
                lastException = e;
                await (options.AfterFailure?.Invoke(e, attempt) ?? CompletedTask).ConfigureAwait(false);

                TimeSpan delay = GetDelay(options.Delay, attempt);

                if (!await ShouldRetry(e, attempt, options).ConfigureAwait(false) || IsDurationExceeded(totalDuration.Elapsed + delay, options)) throw;

                if (delay > TimeSpan.Zero) {
                    await Task.Delay(delay, options.CancellationToken).ConfigureAwait(false);
                }

                await (options.BeforeRetry?.Invoke(e, attempt + 1) ?? CompletedTask).ConfigureAwait(false);
            }
        }

        if (options.CancellationToken.IsCancellationRequested) {
            throw new TaskCanceledException(CancelledMessage);
        } else if (IsDurationExceeded(totalDuration.Elapsed, options)) {
            throw lastException!;
        }

        try {
            await func(attempt).ConfigureAwait(false);
        } catch (Exception e) when (e is not OutOfMemoryException) {
            await (options.AfterFailure?.Invoke(e, attempt) ?? CompletedTask).ConfigureAwait(false);
            throw;
        }
    }

    /// <summary>
    /// Run the given async <paramref name="func"/> until it returns without throwing an exception.
    /// </summary>
    /// <param name="func">An asynchronous function which is prone to sometimes throw exceptions. The <see cref="long"/> argument is the number of attempts, starting from <c>0</c> for the initial attempt.</param>
    /// <param name="options">Control limits and behaviors of the attempts.</param>
    /// <returns>The <typeparamref name="T"/> return value from the first successful attempt of <paramref name="func"/> that does not throw an exception.</returns>
    /// <exception cref="TaskCanceledException">If the <see cref="RetryOptions.CancellationToken"/> is cancelled.</exception>
    /// <exception cref="Exception">Any exception thrown by <paramref name="func"/> on its final attempt.</exception>
    public static async Task<T> Attempt<T>(Func<long, Task<T>> func, IAsyncRetryOptions? options = null) {
        Stopwatch  totalDuration = new();
        Exception? lastException = null;
        options ??= new AsyncRetryOptions();
        long attempt;
        for (attempt = 0; ShouldLoop(attempt, totalDuration, options); attempt++) {
            try {
                totalDuration.Start();
                return await func(attempt).ConfigureAwait(false);
            } catch (Exception e) when (e is not OutOfMemoryException) {
                lastException = e;
                await (options.AfterFailure?.Invoke(e, attempt) ?? CompletedTask).ConfigureAwait(false);

                TimeSpan delay = GetDelay(options.Delay, attempt);

                if (!await ShouldRetry(e, attempt, options).ConfigureAwait(false) || IsDurationExceeded(totalDuration.Elapsed + delay, options)) throw;

                if (delay > TimeSpan.Zero) {
                    await Task.Delay(delay, options.CancellationToken).ConfigureAwait(false);
                }

                await (options.BeforeRetry?.Invoke(e, attempt + 1) ?? CompletedTask).ConfigureAwait(false);
            }
        }

        if (options.CancellationToken.IsCancellationRequested) {
            throw new TaskCanceledException(CancelledMessage);
        } else if (IsDurationExceeded(totalDuration.Elapsed, options)) {
            throw lastException!;
        }

        try {
            return await func(attempt).ConfigureAwait(false);
        } catch (Exception e) when (e is not OutOfMemoryException) {
            await (options.AfterFailure?.Invoke(e, attempt) ?? CompletedTask).ConfigureAwait(false);
            throw;
        }
    }

    private static TimeSpan GetDelay(Func<long, TimeSpan>? delay, long attempt) => delay?.Invoke(attempt) switch {
        { } duration when duration <= TimeSpan.Zero => TimeSpan.Zero,
        { } duration when duration > MaxDelay       => MaxDelay,
        { } duration                                => duration,
        null                                        => TimeSpan.Zero
    };

    private static bool ShouldRetry(Exception exception, long attempt, RetryOptions options) => options.IsRetryAllowed?.Invoke(exception, attempt) ?? true;

    private static async Task<bool> ShouldRetry(Exception exception, long attempt, IAsyncRetryOptions options) =>
        options.IsRetryAllowed?.Invoke(exception, attempt) is not { } isRetryAllowed || await isRetryAllowed.ConfigureAwait(false);

    private static bool ShouldLoop(long attempt, Stopwatch totalDuration, IRetryOptions options) =>
        (options.MaxAttempts == null || attempt < options.MaxAttempts - 1) &&
        (attempt == 0 || !IsDurationExceeded(totalDuration.Elapsed, options)) &&
        !options.CancellationToken.IsCancellationRequested;

    private static bool IsDurationExceeded(TimeSpan elapsed, IRetryOptions options) => options.MaxOverallDuration < elapsed;

}