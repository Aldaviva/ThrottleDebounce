#nullable enable

using System;
using System.Reflection;
using System.Threading;
using Timer = System.Timers.Timer;

namespace ThrottleDebounce;

internal partial class RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> {

    private readonly Delegate                   _rateLimitedCallback;
    private readonly bool                       _leading;
    private readonly bool                       _trailing;
    private readonly Timer                      _minTimer;
    private readonly Timer?                     _maxTimer;
    private readonly FixedSizeArrayPool<object> _parameterArrayPool;
    private readonly int                        _arity;

    private int       _queuedInvocations;
    private object[]? _mostRecentInvocationParameters;
    private TResult?  _mostRecentResult;

    private volatile int  _minTimerRunning;
    private volatile bool _disposed;

    /// <exception cref="ArgumentException">if <paramref name="leading"/> and <paramref name="trailing"/> were both <c>false</c>, or if <paramref name="maxWait"/> is non-positive</exception>
    internal RateLimiter(Delegate rateLimitedCallback, TimeSpan wait, bool leading, bool trailing, TimeSpan maxWait = default) {
        if (!leading && !trailing) {
            throw new ArgumentException("One or both of the leading and trailing arguments must be true, but both were false.");
        } else if (TimeSpan.Zero.Equals(wait)) {
            throw new ArgumentException("The wait argument must have a positive duration.");
        } else if (maxWait < TimeSpan.Zero) {
            throw new ArgumentException("The maxWait argument must not have a negative duration.");
        }

        _rateLimitedCallback = rateLimitedCallback;
        _leading             = leading;
        _trailing            = trailing;

        _arity              = _rateLimitedCallback.GetMethodInfo().GetParameters().Length;
        _parameterArrayPool = _arity != 0 ? new FixedSizeArrayPool<object>(_arity, 2) : null!;

        _minTimer = new Timer { AutoReset = false, Interval = wait.TotalMilliseconds };
        _minTimer.Elapsed += delegate {
            _minTimerRunning = 0;
            WaitTimeHasElapsed();
        };

        if (maxWait != TimeSpan.Zero) {
            _maxTimer         =  new Timer { AutoReset = false, Interval = maxWait.TotalMilliseconds };
            _maxTimer.Elapsed += delegate { WaitTimeHasElapsed(); };
        }
    }

    private void WaitTimeHasElapsed() {
        if (!_disposed
            && Interlocked.Exchange(ref _queuedInvocations, 0) > 0
            && (_arity != 0 ? Interlocked.Exchange(ref _mostRecentInvocationParameters, null) : Throttler.NoParams) is {} parameters) {

            _mostRecentResult = (TResult) _rateLimitedCallback.DynamicInvoke(parameters);

            if (_arity != 0) {
                _parameterArrayPool.Return(parameters);
            }

            ResetTimers();
        }
    }

    private TResult? OnUserInvocation(object[] arguments) {
        if (!_disposed) {

            if (_arity != 0 && Interlocked.Exchange(ref _mostRecentInvocationParameters, arguments) is {} droppedParameters) {
                _parameterArrayPool.Return(droppedParameters);
            }

            bool isMinTimerRunning = Interlocked.Exchange(ref _minTimerRunning, 1) != 0;
            if (_leading && !isMinTimerRunning) {
                _mostRecentResult = (TResult) _rateLimitedCallback.DynamicInvoke(arguments);
            } else if (_trailing) {
                Interlocked.Add(ref _queuedInvocations, 1);
            }

            ResetTimers();
        }

        return _mostRecentResult;
    }

    private void ResetTimers() {
        try {
            // #13: Restart minTimer without actually changing its interval, which is better than stopping and starting it because that temporarily sets an internal object to null and can cause a concurrency problem when another thread tries to call a method on the null object
            _minTimer.Interval = _minTimer.Interval;
            _minTimer.Start();
            _minTimerRunning = 1;
            _maxTimer?.Start();
        } catch (ObjectDisposedException) {
            // Do nothing. Don't try to start timers if they have been concurrently disposed of in another thread.
        }
    }

    public void Dispose() {
        _disposed = true;
        _minTimer.Dispose();
        _maxTimer?.Dispose();
        _queuedInvocations              = 0;
        _minTimerRunning                = 0;
        _mostRecentResult               = default;
        _mostRecentInvocationParameters = null;
    }

}