#nullable enable

namespace ThrottleDebounce;

/// <summary>Create a proxy delegate that debounces the given real delegate, dropping invocations that occur too many times in an interval. Unlike throttling, debouncing will not execute the delegate at all in the face of a constant stream of invocations.</summary>
/// <seealso cref="Throttler"/>
public static class Debouncer {

    /// <summary>
    /// Create a proxy action that debounces <paramref name="action"/>, dropping all but the latest of the invocations that occur too many times in <paramref name="wait"/>. Unlike throttling, debouncing will not execute the delegate at all in the face of a constant stream of invocations.
    /// </summary>
    /// <param name="action">The action you don't want to execute too frequently.</param>
    /// <param name="wait"><paramref name="action"/> will execute at most once in any given interval of this length.</param>
    /// <param name="leading"><c>true</c> to execute <paramref name="action"/> immediately as long as it hasn't executed in the last <paramref name="wait"/> interval, or <c>false</c> to always wait at least <paramref name="wait"/> before executing. <paramref name="leading"/> or <paramref name="trailing"/> must be <c>true</c>.</param>
    /// <param name="trailing"><c>true</c> to execute <paramref name="action"/>at the end of an interval of length <paramref name="wait"/> of no invocations if it didn't execute at the beginning, or <c>false</c> to drop all deferred executions.</param>
    /// <returns>An action that can be <see cref="RateLimitedAction.Invoke"/>d to enqueue an invocation or <see cref="IDisposable.Dispose"/>d to prevent deferred executions from running.</returns>
    public static RateLimitedAction Debounce(Action action, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(action, 0, wait, leading, trailing);

    /// <summary>
    /// Create a proxy function that debounces <paramref name="func"/>, dropping all but the latest of the invocations that occur too many times in <paramref name="wait"/>. Unlike throttling, debouncing will not execute the delegate at all in the face of a constant stream of invocations.
    /// </summary>
    /// <param name="func">The function you don't want to execute too frequently.</param>
    /// <param name="wait"><paramref name="func"/> will execute at most once in any given interval of this length.</param>
    /// <param name="leading"><c>true</c> to execute <paramref name="func"/> immediately as long as it hasn't executed in the last <paramref name="wait"/> interval, or <c>false</c> to always wait at least <paramref name="wait"/> before executing. <paramref name="leading"/> or <paramref name="trailing"/> must be <c>true</c>.</param>
    /// <param name="trailing"><c>true</c> to execute <paramref name="func"/> at the end of an interval of length <paramref name="wait"/> of no invocations if it didn't execute at the beginning, or <c>false</c> to drop all deferred executions.</param>
    /// <returns>A function that can be <see cref="RateLimitedFunc{T}.Invoke"/>d to enqueue an invocation or <see cref="IDisposable.Dispose"/>d to prevent deferred executions from running.</returns>
    public static RateLimitedFunc<TResult> Debounce<TResult>(Func<TResult> func, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, TResult>(func, 0, wait, leading, trailing);

    /// <inheritdoc cref="Debounce(Action,TimeSpan,bool,bool)" />
    public static RateLimitedAction<T1> Debounce<T1>(Action<T1> action, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(action, 1, wait, leading, trailing);

    /// <inheritdoc cref="Debounce(Action,TimeSpan,bool,bool)" />
    public static RateLimitedAction<T1, T2> Debounce<T1, T2>(Action<T1, T2> action, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(action, 2, wait, leading, trailing);

    /// <inheritdoc cref="Debounce(Action,TimeSpan,bool,bool)" />
    public static RateLimitedAction<T1, T2, T3> Debounce<T1, T2, T3>(Action<T1, T2, T3> action, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(action, 3, wait, leading, trailing);

    /// <inheritdoc cref="Debounce(Action,TimeSpan,bool,bool)" />
    public static RateLimitedAction<T1, T2, T3, T4> Debounce<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, object, object, object, object, object, object, object, object, object, object, object, object, object>(action, 4, wait, leading, trailing);

    /// <inheritdoc cref="Debounce(Action,TimeSpan,bool,bool)" />
    public static RateLimitedAction<T1, T2, T3, T4, T5> Debounce<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, object, object, object, object, object, object, object, object, object, object, object, object>(action, 5, wait, leading, trailing);

    /// <inheritdoc cref="Debounce(Action,TimeSpan,bool,bool)" />
    public static RateLimitedAction<T1, T2, T3, T4, T5, T6> Debounce<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, object, object, object, object, object, object, object, object, object, object, object>(action, 6, wait, leading, trailing);

    /// <inheritdoc cref="Debounce(Action,TimeSpan,bool,bool)" />
    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7> Debounce<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action, TimeSpan wait, bool leading = false,
                                                                                                     bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, object, object, object, object, object, object, object, object, object, object>(action, 7, wait, leading, trailing);

    /// <inheritdoc cref="Debounce(Action,TimeSpan,bool,bool)" />
    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8> Debounce<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, TimeSpan wait, bool leading = false,
                                                                                                             bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, object, object, object, object, object, object, object, object, object>(action, 8, wait, leading, trailing);

    /// <inheritdoc cref="Debounce(Action,TimeSpan,bool,bool)" />
    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, TimeSpan wait,
                                                                                                                     bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, object, object, object, object, object, object, object, object>(action, 9, wait, leading, trailing);

    /// <inheritdoc cref="Debounce(Action,TimeSpan,bool,bool)" />
    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object, object, object, object, object, object, object>(action, 10, wait, leading, trailing);

    /// <inheritdoc cref="Debounce(Action,TimeSpan,bool,bool)" />
    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object, object, object, object, object, object>(action, 11, wait, leading, trailing);

    /// <inheritdoc cref="Debounce(Action,TimeSpan,bool,bool)" />
    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, object, object, object, object, object>(action, 12, wait, leading, trailing);

    /// <inheritdoc cref="Debounce(Action,TimeSpan,bool,bool)" />
    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, object, object, object, object>(action, 13, wait, leading, trailing);

    /// <inheritdoc cref="Debounce(Action,TimeSpan,bool,bool)" />
    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, object, object, object>(action, 14, wait, leading, trailing);

    /// <inheritdoc cref="Debounce(Action,TimeSpan,bool,bool)" />
    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, object, object>(action, 15, wait, leading, trailing);

    /// <inheritdoc cref="Debounce(Action,TimeSpan,bool,bool)" />
    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, object>(action, 16, wait, leading, trailing);

    /// <inheritdoc cref="Debounce{TResult}(Func{TResult},TimeSpan,bool,bool)" />
    public static RateLimitedFunc<T1, TResult> Debounce<T1, TResult>(Func<T1, TResult> func, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, TResult>(func, 1, wait, leading, trailing);

    /// <inheritdoc cref="Debounce{TResult}(Func{TResult},TimeSpan,bool,bool)" />
    public static RateLimitedFunc<T1, T2, TResult> Debounce<T1, T2, TResult>(Func<T1, T2, TResult> func, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, object, object, object, object, object, object, object, object, object, object, object, object, object, object, TResult>(func, 2, wait, leading, trailing);

    /// <inheritdoc cref="Debounce{TResult}(Func{TResult},TimeSpan,bool,bool)" />
    public static RateLimitedFunc<T1, T2, T3, TResult> Debounce<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, object, object, object, object, object, object, object, object, object, object, object, object, object, TResult>(func, 3, wait, leading, trailing);

    /// <inheritdoc cref="Debounce{TResult}(Func{TResult},TimeSpan,bool,bool)" />
    public static RateLimitedFunc<T1, T2, T3, T4, TResult> Debounce<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, object, object, object, object, object, object, object, object, object, object, object, object, TResult>(func, 4, wait, leading, trailing);

    /// <inheritdoc cref="Debounce{TResult}(Func{TResult},TimeSpan,bool,bool)" />
    public static RateLimitedFunc<T1, T2, T3, T4, T5, TResult>
        Debounce<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, object, object, object, object, object, object, object, object, object, object, object, TResult>(func, 5, wait, leading, trailing);

    /// <inheritdoc cref="Debounce{TResult}(Func{TResult},TimeSpan,bool,bool)" />
    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, TResult> Debounce<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func, TimeSpan wait, bool leading = false,
                                                                                                             bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, object, object, object, object, object, object, object, object, object, object, TResult>(func, 6, wait, leading, trailing);

    /// <inheritdoc cref="Debounce{TResult}(Func{TResult},TimeSpan,bool,bool)" />
    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, TResult> Debounce<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, TimeSpan wait,
                                                                                                                     bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, object, object, object, object, object, object, object, object, object, TResult>(func, 7, wait, leading, trailing);

    /// <inheritdoc cref="Debounce{TResult}(Func{TResult},TimeSpan,bool,bool)" />
    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Debounce<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, object, object, object, object, object, object, object, object, TResult>(func, 8, wait, leading, trailing);

    /// <inheritdoc cref="Debounce{TResult}(Func{TResult},TimeSpan,bool,bool)" />
    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, object, object, object, object, object, object, object, TResult>(func, 9, wait, leading, trailing);

    /// <inheritdoc cref="Debounce{TResult}(Func{TResult},TimeSpan,bool,bool)" />
    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object, object, object, object, object, object, TResult>(func, 10, wait, leading, trailing);

    /// <inheritdoc cref="Debounce{TResult}(Func{TResult},TimeSpan,bool,bool)" />
    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object, object, object, object, object, TResult>(func, 11, wait, leading, trailing);

    /// <inheritdoc cref="Debounce{TResult}(Func{TResult},TimeSpan,bool,bool)" />
    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, object, object, object, object, TResult>(func, 12, wait, leading, trailing);

    /// <inheritdoc cref="Debounce{TResult}(Func{TResult},TimeSpan,bool,bool)" />
    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, object, object, object, TResult>(func, 13, wait, leading, trailing);

    /// <inheritdoc cref="Debounce{TResult}(Func{TResult},TimeSpan,bool,bool)" />
    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, object, object, TResult>(func, 14, wait, leading, trailing);

    /// <inheritdoc cref="Debounce{TResult}(Func{TResult},TimeSpan,bool,bool)" />
    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, TimeSpan wait, bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, object, TResult>(func, 15, wait, leading, trailing);

    /// <inheritdoc cref="Debounce{TResult}(Func{TResult},TimeSpan,bool,bool)" />
    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>
        Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func,
                                                                                                 TimeSpan wait,
                                                                                                 bool leading = false, bool trailing = true) =>
        new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(func, 16, wait, leading, trailing);

}