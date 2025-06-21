#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace ThrottleDebounce.Retry;

/// <summary>
/// Control the limits and behaviors of <see cref="ThrottleDebounce.Retry.Retrier"/>.
/// </summary>
public interface IOptions {

    /// <summary>The total number of times the function is allowed to run in this invocation, including the first attempt. Equal to 1 initial attempt plus at most <c>MaxAttempts - 1</c> retries if it throws an exception. Must be at least 1, if you set this to 0 it will clip to 1. Defaults to <c>null</c>, which means infinite retries. If both <see cref="MaxAttempts"/> and <see cref="MaxOverallDuration"/> are specified, they will both be applied, so it will stop retrying after either enough attempts OR enough time has passed.</summary>
    long? MaxAttempts { get; }

    /// <summary>Limits the attempts by total time, measured from the start of the first attempt to the end of the last attempt. Any attempts that would start after this duration has ended will not be run, and the most recent attempt exception will be thrown. Defaults to <c>null</c>, which means no upper limit on overall duration. If both <see cref="Options.MaxAttempts"/> and <see cref="Options.MaxOverallDuration"/> are specified, they will both be applied, so it will stop retrying after either enough attempts OR enough time has passed. This does not limit the duration of one individual attempt (which is unbounded), or the amount of waiting time between attempts (see <see cref="Options.Delay"/>), but rather the cumulative time allowed for all attempts together, from calling <c>Retrier.Attempt</c> to it returning or throwing. If you set this to a non-positive <see cref="TimeSpan"/>, it will make exactly one attempt.</summary>
    TimeSpan? MaxOverallDuration { get; }

    /// <summary>
    /// <para>How long to wait between attempts. Defaults to <c>null</c>, which means no delay. This is a function of how many retries have been attempted (starting from <c>0</c>), to allow for strategies such as exponential back-off. Return values outside the range <c>[0, int.MaxValue]</c> ms will be clipped (<c>[0, uint.MaxValue-1]</c> ms starting in .NET 6).</para>
    /// <para>See <see cref="Delays"/> for built-in implementations of different backoff strategies, and <see href="https://dotnetfiddle.net/PrDP6x"/> if you want to play with those strategies and their parameters.</para>
    /// </summary>
    Func<long, TimeSpan>? Delay { get; }

    /// <summary>Allows you to cancel remaining attempts and delays.</summary>
    CancellationToken CancellationToken { get; }

    /// <summary>Allows you to implement custom logic to allow retries, besides just fixed attempt quantity, elapsed duration, and cancellation. This lets you specify certain exceptions that indicate permanent failures to not trigger retries. For example, <see cref="ArgumentOutOfRangeException"/> will usually be thrown every time you call a function with the same arguments, so there is no reason to retry, and your <see cref="Options.IsRetryAllowed"/> function could return <c>false</c> in that case. Also takes the most recent failed attempt number as a parameter, starting at 0 after the first failure. Defaults to retrying on every exception, except <see cref="OutOfMemoryException"/>, which can never be retried.</summary>
    Func<Exception, long, bool>? IsRetryAllowed { get; }

    /// <summary>Action to run between attempts, possibly to log the previous failure. Runs before any <see cref="Options.Delay"/>. Defaults to no action. Takes as parameters the attempt number of the most recent failed attempt (starting from <c>0</c>) and the most recent <see cref="Exception"/> thrown. Does not run after successful attempts, but does run after the final attempt when <see cref="Options.MaxAttempts"/> is exhausted and the final attempt fails.</summary>
    Action<Exception, long>? AfterFailure { get; }

    /// <summary>Action to run between attempts, possibly to clean up some state before the next retry. For example, you may want to disconnect a failed connection before reconnecting. Runs after any <see cref="Options.Delay"/>. Defaults to no action. Takes as parameters the attempt number of the retry that is about to run (starting from <c>1</c>) and the most recent <see cref="Exception"/> thrown. Does not run before the initial attempt.</summary>
    Action<Exception, long>? BeforeRetry { get; }

    /// <inheritdoc />
    public interface Async: IOptions {

        /// <inheritdoc cref="IOptions.IsRetryAllowed" />
        new Func<Exception, long, Task<bool>>? IsRetryAllowed { get; }

        /// <inheritdoc cref="IOptions.AfterFailure" />
        new Func<Exception, long, Task>? AfterFailure { get; }

        /// <inheritdoc cref="IOptions.BeforeRetry" />
        new Func<Exception, long, Task>? BeforeRetry { get; }

    }

}

/// <inheritdoc cref="IOptions" />
public record struct Options(): IOptions.Async {

    /// <inheritdoc />
    public long? MaxAttempts {
        get;
        set => field = value is < 1 ? 1 : value;
    }

    /// <inheritdoc />
    public TimeSpan? MaxOverallDuration {
        get;
        set => field = value switch {
            null                                                   => null,
            { } infinite when infinite == Timeout.InfiniteTimeSpan => null,
            { } negative when negative < TimeSpan.Zero             => TimeSpan.Zero,
            _                                                      => value
        };
    }

    /// <inheritdoc />
    public Func<long, TimeSpan>? Delay { get; set; }

    /// <inheritdoc />
    public Func<Exception, long, bool>? IsRetryAllowed { get; set; }

    /// <inheritdoc />
    public Action<Exception, long>? AfterFailure { get; set; }

    /// <inheritdoc />
    public Action<Exception, long>? BeforeRetry { get; set; }

    /// <inheritdoc />
    public CancellationToken CancellationToken { get; set; } = CancellationToken.None;

    readonly Func<Exception, long, Task<bool>>? IOptions.Async.IsRetryAllowed => IsRetryAllowed is { } isRetryAllowed ? (e, attempt) => Task.FromResult(isRetryAllowed(e, attempt)) : null;

    readonly Func<Exception, long, Task>? IOptions.Async.AfterFailure => AfterFailure is { } afterFailure ? (attempt, e) => {
        afterFailure(attempt, e);
        return Retrier.CompletedTask;
    } : null;

    readonly Func<Exception, long, Task>? IOptions.Async.BeforeRetry => BeforeRetry is { } beforeRetry ? (attempt, e) => {
        beforeRetry(attempt, e);
        return Retrier.CompletedTask;
    } : null;

    /// <inheritdoc cref="IOptions.Async" />
    public record struct Async(): IOptions.Async {

        /// <inheritdoc />
        public long? MaxAttempts {
            get;
            set => field = value is < 1 ? 1 : value;
        }
        /// <inheritdoc />
        public TimeSpan? MaxOverallDuration {
            get;
            set => field = value switch {
                null                                                   => null,
                { } infinite when infinite == Timeout.InfiniteTimeSpan => null,
                { } negative when negative < TimeSpan.Zero             => TimeSpan.Zero,
                _                                                      => value
            };
        }

        /// <inheritdoc />
        public Func<long, TimeSpan>? Delay { get; set; }

        /// <inheritdoc />
        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;

        /// <inheritdoc />
        public Func<Exception, long, Task<bool>>? IsRetryAllowed { get; set; }

        /// <inheritdoc />
        public Func<Exception, long, Task>? AfterFailure { get; set; }

        /// <inheritdoc />
        public Func<Exception, long, Task>? BeforeRetry { get; set; }

        readonly Func<Exception, long, bool>? IOptions.IsRetryAllowed => null;

        readonly Action<Exception, long>? IOptions.AfterFailure => null;

        readonly Action<Exception, long>? IOptions.BeforeRetry => null;

    }

}