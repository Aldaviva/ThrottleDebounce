using System;

namespace ThrottleDebounce.RateLimitedDelegates {

    public interface RateLimitedAction: IDisposable {

        Action RateLimitedAction { get; }

    }

    public interface RateLimitedAction<in T1>: IDisposable {

        Action<T1> RateLimitedAction { get; }

    }

    public interface RateLimitedAction<in T1, in T2>: IDisposable {

        Action<T1, T2> RateLimitedAction { get; }

    }

    public interface RateLimitedAction<in T1, in T2, in T3>: IDisposable {

        Action<T1, T2, T3> RateLimitedAction { get; }

    }

    public interface RateLimitedAction<in T1, in T2, in T3, in T4>: IDisposable {

        Action<T1, T2, T3, T4> RateLimitedAction { get; }

    }

    public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5>: IDisposable {

        Action<T1, T2, T3, T4, T5> RateLimitedAction { get; }

    }

    public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6>: IDisposable {

        Action<T1, T2, T3, T4, T5, T6> RateLimitedAction { get; }

    }

    public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7>: IDisposable {

        Action<T1, T2, T3, T4, T5, T6, T7> RateLimitedAction { get; }

    }

    public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8>: IDisposable {

        Action<T1, T2, T3, T4, T5, T6, T7, T8> RateLimitedAction { get; }

    }

    public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9>: IDisposable {

        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> RateLimitedAction { get; }

    }

    public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10>: IDisposable {

        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> RateLimitedAction { get; }

    }

    public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11>: IDisposable {

        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> RateLimitedAction { get; }

    }

    public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12>: IDisposable {

        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> RateLimitedAction { get; }

    }

    public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13>: IDisposable {

        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> RateLimitedAction { get; }

    }

    public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14>: IDisposable {

        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> RateLimitedAction { get; }

    }

    public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15>: IDisposable {

        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> RateLimitedAction { get; }

    }

    public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16>: IDisposable {

        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> RateLimitedAction { get; }

    }

    public interface RateLimitedFunc<out TResult>: IDisposable {

        Func<TResult> RateLimitedFunc { get; }

    }

    public interface RateLimitedFunc<in T1, out TResult>: IDisposable {

        Func<T1, TResult> RateLimitedFunc { get; }

    }

    public interface RateLimitedFunc<in T1, in T2, out TResult>: IDisposable {

        Func<T1, T2, TResult> RateLimitedFunc { get; }

    }

    public interface RateLimitedFunc<in T1, in T2, in T3, out TResult>: IDisposable {

        Func<T1, T2, T3, TResult> RateLimitedFunc { get; }

    }

    public interface RateLimitedFunc<in T1, in T2, in T3, in T4, out TResult>: IDisposable {

        Func<T1, T2, T3, T4, TResult> RateLimitedFunc { get; }

    }

    public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, out TResult>: IDisposable {

        Func<T1, T2, T3, T4, T5, TResult> RateLimitedFunc { get; }

    }

    public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, out TResult>: IDisposable {

        Func<T1, T2, T3, T4, T5, T6, TResult> RateLimitedFunc { get; }

    }

    public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, out TResult>: IDisposable {

        Func<T1, T2, T3, T4, T5, T6, T7, TResult> RateLimitedFunc { get; }

    }

    public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, out TResult>: IDisposable {

        Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> RateLimitedFunc { get; }

    }

    public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, out TResult>: IDisposable {

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> RateLimitedFunc { get; }

    }

    public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, out TResult>: IDisposable {

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> RateLimitedFunc { get; }

    }

    public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, out TResult>: IDisposable {

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> RateLimitedFunc { get; }

    }

    public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, out TResult>: IDisposable {

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> RateLimitedFunc { get; }

    }

    public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, out TResult>: IDisposable {

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> RateLimitedFunc { get; }

    }

    public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, out TResult>: IDisposable {

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> RateLimitedFunc { get; }

    }

    public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, out TResult>: IDisposable {

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> RateLimitedFunc { get; }

    }

    public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16, out TResult>: IDisposable {

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> RateLimitedFunc { get; }

    }

}