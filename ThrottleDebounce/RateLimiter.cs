#nullable enable

using System;
using System.Reflection;
using System.Threading;
using Timer = System.Timers.Timer;

namespace ThrottleDebounce;

internal partial class RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> {

    private readonly Delegate                   rateLimitedCallback;
    private readonly bool                       leading;
    private readonly bool                       trailing;
    private readonly Timer                      minTimer;
    private readonly Timer?                     maxTimer;
    private readonly FixedSizeArrayPool<object> parameterArrayPool;
    private readonly int                        arity;

    private int       queuedInvocations;
    private object[]? mostRecentInvocationParameters;
    private TResult?  mostRecentResult;

    private volatile int  minTimerRunning;
    private volatile bool disposed;

    /// <exception cref="ArgumentException">if <paramref name="leading"/> and <paramref name="trailing"/> were both <see langword="false"/>, or if <paramref name="maxWait"/> is non-positive</exception>
    internal RateLimiter(Delegate rateLimitedCallback, TimeSpan wait, bool leading, bool trailing, TimeSpan maxWait = default) {
        if (!leading && !trailing) {
            throw new ArgumentException("One or both of the leading and trailing arguments must be true, but both were false.");
        } else if (TimeSpan.Zero.Equals(wait)) {
            throw new ArgumentException("The wait argument must have a positive duration.");
        } else if (maxWait < TimeSpan.Zero) {
            throw new ArgumentException("The maxWait argument must not have a negative duration.");
        }

        this.rateLimitedCallback = rateLimitedCallback;
        this.leading             = leading;
        this.trailing            = trailing;

        arity              = this.rateLimitedCallback.GetMethodInfo().GetParameters().Length;
        parameterArrayPool = arity != 0 ? new FixedSizeArrayPool<object>(arity, 2) : null!;

        minTimer = new Timer { AutoReset = false, Interval = wait.TotalMilliseconds };
        minTimer.Elapsed += delegate {
            minTimerRunning = 0;
            WaitTimeHasElapsed();
        };

        if (maxWait != TimeSpan.Zero) {
            maxTimer         =  new Timer { AutoReset = false, Interval = maxWait.TotalMilliseconds };
            maxTimer.Elapsed += delegate { WaitTimeHasElapsed(); };
        }
    }

    private void WaitTimeHasElapsed() {
        if (!disposed
            && Interlocked.Exchange(ref queuedInvocations, 0) > 0
            && (arity != 0 ? Interlocked.Exchange(ref mostRecentInvocationParameters, null) : Throttler.NO_PARAMS) is { } parameters) {

            mostRecentResult = (TResult) rateLimitedCallback.DynamicInvoke(parameters);

            if (arity != 0) {
                parameterArrayPool.Return(parameters);
            }

            resetTimers();
        }
    }

    private TResult? OnUserInvocation(object[] arguments) {
        if (!disposed) {

            if (arity != 0 && Interlocked.Exchange(ref mostRecentInvocationParameters, arguments) is { } droppedParameters) {
                parameterArrayPool.Return(droppedParameters);
            }

            bool isMinTimerRunning = Interlocked.Exchange(ref minTimerRunning, 1) != 0;
            if (leading && !isMinTimerRunning) {
                mostRecentResult = (TResult) rateLimitedCallback.DynamicInvoke(arguments);
            } else if (trailing) {
                Interlocked.Add(ref queuedInvocations, 1);
            }

            resetTimers();
        }

        return mostRecentResult;
    }

    private void resetTimers() {
        try {
            minTimer.Stop();
            minTimer.Start();
            minTimerRunning = 1;
            maxTimer?.Start();
        } catch (ObjectDisposedException) {
            // Do nothing. Don't try to start timers if they have been concurrently disposed of in another thread.
        }
    }

    public void Dispose() {
        disposed = true;
        minTimer.Dispose();
        maxTimer?.Dispose();
        queuedInvocations              = 0;
        minTimerRunning                = 0;
        mostRecentResult               = default;
        mostRecentInvocationParameters = null;
    }

}