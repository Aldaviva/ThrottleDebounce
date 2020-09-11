using System;
using System.Threading;
using ThrottleDebounce.RateLimitedDelegates;
using Timer = System.Timers.Timer;

namespace ThrottleDebounce {

    internal class RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>: RateLimitedFunc<TResult>, RateLimitedFunc<T1, TResult>,
        RateLimitedFunc<T1, T2, TResult>,
        RateLimitedFunc<T1, T2, T3, TResult>, RateLimitedFunc<T1, T2, T3, T4, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, TResult>,
        RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>,
        RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>,
        RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>,
        RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>,
        RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>, RateLimitedAction, RateLimitedAction<T1>, RateLimitedAction<T1, T2>,
        RateLimitedAction<T1, T2, T3>,
        RateLimitedAction<T1, T2, T3, T4>, RateLimitedAction<T1, T2, T3, T4, T5>, RateLimitedAction<T1, T2, T3, T4, T5, T6>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7>,
        RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>,
        RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>,
        RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>,
        RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> {

        private readonly Delegate rateLimitedCallback;
        private readonly bool     leading;
        private readonly bool     trailing;
        private readonly Timer    minTimer;
        private readonly Timer    maxTimer;

        private int               queuedInvocations;
        private object[]          mostRecentInvocationParameters;
        private TResult           mostRecentResult;

        internal RateLimiter(Delegate rateLimitedCallback, TimeSpan wait, bool leading, bool trailing, TimeSpan maxWait = default) {
            if (!leading && !trailing) {
                throw new ArgumentException("One or both of the leading and trailing arguments must be true, but both were false.");
            } else if (TimeSpan.Zero.Equals(wait)) {
                throw new ArgumentException("The wait argument must have a positive duration.");
            } else if (maxWait < TimeSpan.Zero) {
                throw new ArgumentException("The maxWait argument must not have a negative duration.");
            }

            this.rateLimitedCallback = rateLimitedCallback;
            this.leading = leading;
            this.trailing = trailing;

            minTimer = new Timer { AutoReset = false, Interval = wait.TotalMilliseconds };
            minTimer.Elapsed += delegate { WaitTimeHasElapsed(); };

            if (maxWait != default) {
                maxTimer = new Timer { AutoReset = false, Interval = maxWait.TotalMilliseconds };
                maxTimer.Elapsed += delegate { WaitTimeHasElapsed(); };
            }
        }

        private void WaitTimeHasElapsed() {
            if (Interlocked.Exchange(ref queuedInvocations, 0) > 0) {
                mostRecentResult = (TResult) rateLimitedCallback.DynamicInvoke(mostRecentInvocationParameters);

                minTimer.Stop();
                minTimer.Start();
                maxTimer?.Start();
            }
        }

        private TResult OnUserInvocation(object[] arguments) {
            mostRecentInvocationParameters = arguments;

            bool minTimerRunning = minTimer.Enabled;
            if (leading && !minTimerRunning) {
                mostRecentResult = (TResult) rateLimitedCallback.DynamicInvoke(arguments);
            } else if (trailing) {
                Interlocked.Add(ref queuedInvocations, 1);
            }

            minTimer.Stop();
            minTimer.Start();
            maxTimer?.Start();

            return mostRecentResult;
        }

        public void Dispose() {
            minTimer.Dispose();
            maxTimer?.Dispose();
            queuedInvocations = 0;
            mostRecentResult = default;
            mostRecentInvocationParameters = new object[0];
        }

        Action RateLimitedAction.RateLimitedAction => () => { OnUserInvocation(new object[0]); };

        Action<T1> RateLimitedAction<T1>.RateLimitedAction => arg1 => { OnUserInvocation(new object[] { arg1 }); };

        Action<T1, T2> RateLimitedAction<T1, T2>.RateLimitedAction => (arg1, arg2) => { OnUserInvocation(new object[] { arg1, arg2 }); };

        Action<T1, T2, T3> RateLimitedAction<T1, T2, T3>.RateLimitedAction => (arg1, arg2, arg3) => { OnUserInvocation(new object[] { arg1, arg2, arg3 }); };

        Action<T1, T2, T3, T4> RateLimitedAction<T1, T2, T3, T4>.RateLimitedAction => (arg1, arg2, arg3, arg4) => { OnUserInvocation(new object[] { arg1, arg2, arg3, arg4 }); };

        Action<T1, T2, T3, T4, T5> RateLimitedAction<T1, T2, T3, T4, T5>.RateLimitedAction => (arg1, arg2, arg3, arg4, arg5) => { OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5 }); };

        Action<T1, T2, T3, T4, T5, T6> RateLimitedAction<T1, T2, T3, T4, T5, T6>.RateLimitedAction => (arg1, arg2, arg3, arg4, arg5, arg6) => {
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        };

        Action<T1, T2, T3, T4, T5, T6, T7> RateLimitedAction<T1, T2, T3, T4, T5, T6, T7>.RateLimitedAction => (arg1, arg2, arg3, arg4, arg5, arg6, arg7) => {
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        };

        Action<T1, T2, T3, T4, T5, T6, T7, T8> RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8>.RateLimitedAction => (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => {
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        };

        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>.RateLimitedAction => (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => {
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        };

        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.RateLimitedAction =>
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => { OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 }); };

        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.RateLimitedAction =>
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => { OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 }); };

        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.RateLimitedAction =>
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) => {
                OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
            };

        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.RateLimitedAction =>
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) => {
                OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
            };

        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.RateLimitedAction =>
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) => {
                OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
            };

        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.RateLimitedAction =>
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) => {
                OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
            };

        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>.RateLimitedAction =>
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) => {
                OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
            };

        Func<TResult> RateLimitedFunc<TResult>.RateLimitedFunc => () => OnUserInvocation(new object[0]);

        Func<T1, TResult> RateLimitedFunc<T1, TResult>.RateLimitedFunc => arg1 => OnUserInvocation(new object[] { arg1 });

        Func<T1, T2, TResult> RateLimitedFunc<T1, T2, TResult>.RateLimitedFunc => (arg1, arg2) => OnUserInvocation(new object[] { arg1, arg2 });

        Func<T1, T2, T3, TResult> RateLimitedFunc<T1, T2, T3, TResult>.RateLimitedFunc => (arg1, arg2, arg3) => OnUserInvocation(new object[] { arg1, arg2, arg3 });

        Func<T1, T2, T3, T4, TResult> RateLimitedFunc<T1, T2, T3, T4, TResult>.RateLimitedFunc => (arg1, arg2, arg3, arg4) => OnUserInvocation(new object[] { arg1, arg2, arg3, arg4 });

        Func<T1, T2, T3, T4, T5, TResult> RateLimitedFunc<T1, T2, T3, T4, T5, TResult>.RateLimitedFunc =>
            (arg1, arg2, arg3, arg4, arg5) => OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5 });

        Func<T1, T2, T3, T4, T5, T6, TResult> RateLimitedFunc<T1, T2, T3, T4, T5, T6, TResult>.RateLimitedFunc =>
            (arg1, arg2, arg3, arg4, arg5, arg6) => OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6 });

        Func<T1, T2, T3, T4, T5, T6, T7, TResult> RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, TResult>.RateLimitedFunc =>
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7) => OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });

        Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>.RateLimitedFunc => (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>.RateLimitedFunc => (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>.RateLimitedFunc =>
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>.RateLimitedFunc =>
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>.RateLimitedFunc =>
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) => OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>.RateLimitedFunc =>
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
                OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>.RateLimitedFunc =>
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
                OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>.RateLimitedFunc =>
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
                OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>.
            RateLimitedFunc => (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });

    }

}