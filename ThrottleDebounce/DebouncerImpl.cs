using System;
using System.Threading;
using Timer = System.Timers.Timer;

namespace ThrottleDebounce {

    internal class DebouncerImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>: DebouncedFunc<TResult>, DebouncedFunc<T1, TResult>, DebouncedFunc<T1, T2, TResult>,
        DebouncedFunc<T1, T2, T3, TResult>, DebouncedFunc<T1, T2, T3, T4, TResult>, DebouncedFunc<T1, T2, T3, T4, T5, TResult>, DebouncedFunc<T1, T2, T3, T4, T5, T6, TResult>,
        DebouncedFunc<T1, T2, T3, T4, T5, T6, T7, TResult>, DebouncedFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, DebouncedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>,
        DebouncedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>, DebouncedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>,
        DebouncedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>, DebouncedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>,
        DebouncedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>, DebouncedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>,
        DebouncedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>, DebouncedAction, DebouncedAction<T1>, DebouncedAction<T1, T2>, DebouncedAction<T1, T2, T3>,
        DebouncedAction<T1, T2, T3, T4>, DebouncedAction<T1, T2, T3, T4, T5>, DebouncedAction<T1, T2, T3, T4, T5, T6>, DebouncedAction<T1, T2, T3, T4, T5, T6, T7>,
        DebouncedAction<T1, T2, T3, T4, T5, T6, T7, T8>, DebouncedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>, DebouncedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>,
        DebouncedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, DebouncedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>,
        DebouncedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, DebouncedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>,
        DebouncedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, DebouncedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> {

        private readonly Delegate rateLimitedCallback;
        private readonly bool leading;
        private readonly bool trailing;
        private readonly Timer minTimer = new Timer { AutoReset = false };
        private readonly Timer maxTimer = new Timer { AutoReset = false };

        private int queuedInvocations;
        private object[] mostRecentInvocationParameters;
        private TResult mostRecentResult;

        internal DebouncerImpl(Delegate rateLimitedCallback, TimeSpan wait, bool leading, bool trailing, TimeSpan maxWait = default) {
            if (!leading && !trailing) {
                throw new ArgumentException("One or both of leading and trailing must be true, but both were false.");
            } else if (TimeSpan.Zero.Equals(wait)) {
                throw new ArgumentException("The wait argument must have a positive duration.");
            } else if (maxWait < default(TimeSpan)) {
                throw new ArgumentException("The maxWait argument must have a non-negative duration.");
            }

            this.rateLimitedCallback = rateLimitedCallback;
            this.leading = leading;
            this.trailing = trailing;

            minTimer.Interval = wait.TotalMilliseconds;
            minTimer.Elapsed += delegate { WaitTimeHasElapsed(); };

            if (maxWait != default) {
                maxTimer.Interval = maxWait.TotalMilliseconds;
                maxTimer.Elapsed += delegate { WaitTimeHasElapsed(); };
            } else {
                maxTimer = null;
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

            TResult result = default;
            bool hasResult = false;

            bool minTimerRunning = minTimer.Enabled;
            if (leading && !minTimerRunning) {
                result = (TResult) rateLimitedCallback.DynamicInvoke(arguments);
                hasResult = true;
            } else if (trailing) {
                Interlocked.Add(ref queuedInvocations, 1);
            }

            minTimer.Stop();
            minTimer.Start();
            maxTimer?.Start();

            return hasResult ? result : mostRecentResult;
        }

        public void Dispose() {
            minTimer?.Dispose();
            maxTimer?.Dispose();
        }

        public TResult Call() {
            return OnUserInvocation(new object[0]);
        }

        public TResult Call(T1 arg1) {
            return OnUserInvocation(new object[] { arg1 });
        }

        public TResult Call(T1 arg1, T2 arg2) {
            return OnUserInvocation(new object[] { arg1, arg2 });
        }

        public TResult Call(T1 arg1, T2 arg2, T3 arg3) {
            return OnUserInvocation(new object[] { arg1, arg2, arg3 });
        }

        public TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            return OnUserInvocation(new object[] { arg1, arg2, arg3, arg4 });
        }

        public TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            return OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5 });
        }

        public TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            return OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        }

        public TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            return OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        }

        public TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
            return OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        }

        public TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
            return OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        }

        public TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) {
            return OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        }

        public TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) {
            return OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        }

        public TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) {
            return OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        }

        public TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) {
            return OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        }

        public TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) {
            return OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        }

        public TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) {
            return OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        }

        public TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) {
            return OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        }

        public void Run() {
            OnUserInvocation(new object[0]);
        }

        public void Run(T1 arg1) {
            OnUserInvocation(new object[] { arg1 });
        }

        public void Run(T1 arg1, T2 arg2) {
            OnUserInvocation(new object[] { arg1, arg2 });
        }

        public void Run(T1 arg1, T2 arg2, T3 arg3) {
            OnUserInvocation(new object[] { arg1, arg2, arg3 });
        }

        public void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4 });
        }

        public void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5 });
        }

        public void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        }

        public void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        }

        public void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        }

        public void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        }

        public void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) {
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        }

        public void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) {
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        }

        public void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) {
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        }

        public void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) {
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        }

        public void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) {
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        }

        public void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) {
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        }

        public void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) {
            OnUserInvocation(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        }

    }

}