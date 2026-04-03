#nullable enable

namespace ThrottleDebounce;

/// <summary>
/// <para>An action that has been throttled or debounced to not execute as frequently as it is invoked.</para>
/// <para>To enqueue an invocation of this action, call <see cref="Invoke"/>. This invocation may or may not be executed, depending on when other invocations occur and the rate limiting parameters.</para>
/// <para>To prevent future executions of this action, call <see cref="IDisposable.Dispose"/>.</para>
/// </summary>
public interface RateLimitedAction: IDisposable {

    /// <summary>
    /// Enqueue an invocation of this throttled or debounced action. This invocation may or may not be executed, depending on when other invocations occur and the rate limiting parameters.
    /// </summary>
    void Invoke();

}

/// <inheritdoc cref="RateLimitedAction" />
public interface RateLimitedAction<in T1>: IDisposable {

    /// <inheritdoc cref="RateLimitedAction.Invoke" />
    void Invoke(T1 arg1);

}

/// <inheritdoc cref="RateLimitedAction" />
public interface RateLimitedAction<in T1, in T2>: IDisposable {

    /// <inheritdoc cref="RateLimitedAction.Invoke" />
    void Invoke(T1 arg1, T2 arg2);

}

/// <inheritdoc cref="RateLimitedAction" />
public interface RateLimitedAction<in T1, in T2, in T3>: IDisposable {

    /// <inheritdoc cref="RateLimitedAction.Invoke" />
    void Invoke(T1 arg1, T2 arg2, T3 arg3);

}

/// <inheritdoc cref="RateLimitedAction" />
public interface RateLimitedAction<in T1, in T2, in T3, in T4>: IDisposable {

    /// <inheritdoc cref="RateLimitedAction.Invoke" />
    void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

}

/// <inheritdoc cref="RateLimitedAction" />
public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5>: IDisposable {

    /// <inheritdoc cref="RateLimitedAction.Invoke" />
    void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

}

/// <inheritdoc cref="RateLimitedAction" />
public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6>: IDisposable {

    /// <inheritdoc cref="RateLimitedAction.Invoke" />
    void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

}

/// <inheritdoc cref="RateLimitedAction" />
public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7>: IDisposable {

    /// <inheritdoc cref="RateLimitedAction.Invoke" />
    void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

}

/// <inheritdoc cref="RateLimitedAction" />
public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8>: IDisposable {

    /// <inheritdoc cref="RateLimitedAction.Invoke" />
    void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

}

/// <inheritdoc cref="RateLimitedAction" />
public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9>: IDisposable {

    /// <inheritdoc cref="RateLimitedAction.Invoke" />
    void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

}

/// <inheritdoc cref="RateLimitedAction" />
public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10>: IDisposable {

    /// <inheritdoc cref="RateLimitedAction.Invoke" />
    void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

}

/// <inheritdoc cref="RateLimitedAction" />
public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11>: IDisposable {

    /// <inheritdoc cref="RateLimitedAction.Invoke" />
    void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);

}

/// <inheritdoc cref="RateLimitedAction" />
public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12>: IDisposable {

    /// <inheritdoc cref="RateLimitedAction.Invoke" />
    void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);

}

/// <inheritdoc cref="RateLimitedAction" />
public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13>: IDisposable {

    /// <inheritdoc cref="RateLimitedAction.Invoke" />
    void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);

}

/// <inheritdoc cref="RateLimitedAction" />
public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14>: IDisposable {

    /// <inheritdoc cref="RateLimitedAction.Invoke" />
    void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);

}

/// <inheritdoc cref="RateLimitedAction" />
public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15>: IDisposable {

    /// <inheritdoc cref="RateLimitedAction.Invoke" />
    void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);

}

/// <inheritdoc cref="RateLimitedAction" />
public interface RateLimitedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16>: IDisposable {

    /// <inheritdoc cref="RateLimitedAction.Invoke" />
    void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);

}

/// <summary>
/// <para>A function that has been throttled or debounced to not execute as frequently as it is invoked.</para>
/// <para>To enqueue an invocation of this function, call <see cref="Invoke"/>. This invocation may or may not be executed, depending on when other invocations occur and the rate limiting parameters.</para>
/// <para>To prevent future executions of this function, call <see cref="IDisposable.Dispose"/>.</para>
/// </summary>
public interface RateLimitedFunc<out TResult>: IDisposable {

    /// <summary>
    /// <para>Enqueue an invocation of this throttled or debounced function. This invocation may or may not be executed, depending on when other invocations occur and the rate limiting parameters.</para>
    /// <para>If this function has been debounced with leading edge executions disabled (which is the default) and this is a leading edge execution, this method will return <c>default</c>.</para>
    /// </summary>
    TResult? Invoke();

}

/// <inheritdoc cref="RateLimitedFunc{TResult}" />
public interface RateLimitedFunc<in T1, out TResult>: IDisposable {

    /// <inheritdoc cref="RateLimitedFunc{TResult}.Invoke" />
    TResult? Invoke(T1 arg1);

}

/// <inheritdoc cref="RateLimitedFunc{TResult}" />
public interface RateLimitedFunc<in T1, in T2, out TResult>: IDisposable {

    /// <inheritdoc cref="RateLimitedFunc{TResult}.Invoke" />
    TResult? Invoke(T1 arg1, T2 arg2);

}

/// <inheritdoc cref="RateLimitedFunc{TResult}" />
public interface RateLimitedFunc<in T1, in T2, in T3, out TResult>: IDisposable {

    /// <inheritdoc cref="RateLimitedFunc{TResult}.Invoke" />
    TResult? Invoke(T1 arg1, T2 arg2, T3 arg3);

}

/// <inheritdoc cref="RateLimitedFunc{TResult}" />
public interface RateLimitedFunc<in T1, in T2, in T3, in T4, out TResult>: IDisposable {

    /// <inheritdoc cref="RateLimitedFunc{TResult}.Invoke" />
    TResult? Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

}

/// <inheritdoc cref="RateLimitedFunc{TResult}" />
public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, out TResult>: IDisposable {

    /// <inheritdoc cref="RateLimitedFunc{TResult}.Invoke" />
    TResult? Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

}

/// <inheritdoc cref="RateLimitedFunc{TResult}" />
public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, out TResult>: IDisposable {

    /// <inheritdoc cref="RateLimitedFunc{TResult}.Invoke" />
    TResult? Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

}

/// <inheritdoc cref="RateLimitedFunc{TResult}" />
public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, out TResult>: IDisposable {

    /// <inheritdoc cref="RateLimitedFunc{TResult}.Invoke" />
    TResult? Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

}

/// <inheritdoc cref="RateLimitedFunc{TResult}" />
public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, out TResult>: IDisposable {

    /// <inheritdoc cref="RateLimitedFunc{TResult}.Invoke" />
    TResult? Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

}

/// <inheritdoc cref="RateLimitedFunc{TResult}" />
public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, out TResult>: IDisposable {

    /// <inheritdoc cref="RateLimitedFunc{TResult}.Invoke" />
    TResult? Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

}

/// <inheritdoc cref="RateLimitedFunc{TResult}" />
public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, out TResult>: IDisposable {

    /// <inheritdoc cref="RateLimitedFunc{TResult}.Invoke" />
    TResult? Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

}

/// <inheritdoc cref="RateLimitedFunc{TResult}" />
public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, out TResult>: IDisposable {

    /// <inheritdoc cref="RateLimitedFunc{TResult}.Invoke" />
    TResult? Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);

}

/// <inheritdoc cref="RateLimitedFunc{TResult}" />
public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, out TResult>: IDisposable {

    /// <inheritdoc cref="RateLimitedFunc{TResult}.Invoke" />
    TResult? Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);

}

/// <inheritdoc cref="RateLimitedFunc{TResult}" />
public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, out TResult>: IDisposable {

    /// <inheritdoc cref="RateLimitedFunc{TResult}.Invoke" />
    TResult? Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);

}

/// <inheritdoc cref="RateLimitedFunc{TResult}" />
public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, out TResult>: IDisposable {

    /// <inheritdoc cref="RateLimitedFunc{TResult}.Invoke" />
    TResult? Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);

}

/// <inheritdoc cref="RateLimitedFunc{TResult}" />
public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, out TResult>: IDisposable {

    /// <inheritdoc cref="RateLimitedFunc{TResult}.Invoke" />
    TResult? Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);

}

/// <inheritdoc cref="RateLimitedFunc{TResult}" />
public interface RateLimitedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16, out TResult>: IDisposable {

    /// <inheritdoc cref="RateLimitedFunc{TResult}.Invoke" />
    TResult? Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);

}