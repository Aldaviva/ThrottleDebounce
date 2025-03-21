#nullable enable

namespace ThrottleDebounce;

internal partial class RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>: RateLimitedFunc<TResult>, RateLimitedFunc<T1, TResult>,
    RateLimitedFunc<T1, T2, TResult>,
    RateLimitedFunc<T1, T2, T3, TResult>, RateLimitedFunc<T1, T2, T3, T4, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, TResult>,
    RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>,
    RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>,
    RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>,
    RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>,
    RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>,
    RateLimitedAction, RateLimitedAction<T1>, RateLimitedAction<T1, T2>,
    RateLimitedAction<T1, T2, T3>,
    RateLimitedAction<T1, T2, T3, T4>, RateLimitedAction<T1, T2, T3, T4, T5>, RateLimitedAction<T1, T2, T3, T4, T5, T6>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7>,
    RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>,
    RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>,
    RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>,
    RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> {

    void RateLimitedAction.Invoke() => OnUserInvocation(Throttler.NO_PARAMS);

    TResult? RateLimitedFunc<TResult>.Invoke() => OnUserInvocation(Throttler.NO_PARAMS);

    void RateLimitedAction<T1>.Invoke(T1 arg1) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        OnUserInvocation(parameters);
    }

    void RateLimitedAction<T1, T2>.Invoke(T1 arg1, T2 arg2) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        OnUserInvocation(parameters);
    }

    void RateLimitedAction<T1, T2, T3>.Invoke(T1 arg1, T2 arg2, T3 arg3) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        parameters[2] = arg3!;
        OnUserInvocation(parameters);
    }

    void RateLimitedAction<T1, T2, T3, T4>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        parameters[2] = arg3!;
        parameters[3] = arg4!;
        OnUserInvocation(parameters);
    }

    void RateLimitedAction<T1, T2, T3, T4, T5>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        parameters[2] = arg3!;
        parameters[3] = arg4!;
        parameters[4] = arg5!;
        OnUserInvocation(parameters);
    }

    void RateLimitedAction<T1, T2, T3, T4, T5, T6>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        parameters[2] = arg3!;
        parameters[3] = arg4!;
        parameters[4] = arg5!;
        parameters[5] = arg6!;
        OnUserInvocation(parameters);
    }

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        parameters[2] = arg3!;
        parameters[3] = arg4!;
        parameters[4] = arg5!;
        parameters[5] = arg6!;
        parameters[6] = arg7!;
        OnUserInvocation(parameters);
    }

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        parameters[2] = arg3!;
        parameters[3] = arg4!;
        parameters[4] = arg5!;
        parameters[5] = arg6!;
        parameters[6] = arg7!;
        parameters[7] = arg8!;
        OnUserInvocation(parameters);
    }

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        parameters[2] = arg3!;
        parameters[3] = arg4!;
        parameters[4] = arg5!;
        parameters[5] = arg6!;
        parameters[6] = arg7!;
        parameters[7] = arg8!;
        parameters[8] = arg9!;
        OnUserInvocation(parameters);
    }

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        parameters[2] = arg3!;
        parameters[3] = arg4!;
        parameters[4] = arg5!;
        parameters[5] = arg6!;
        parameters[6] = arg7!;
        parameters[7] = arg8!;
        parameters[8] = arg9!;
        parameters[9] = arg10!;
        OnUserInvocation(parameters);
    }

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0]  = arg1!;
        parameters[1]  = arg2!;
        parameters[2]  = arg3!;
        parameters[3]  = arg4!;
        parameters[4]  = arg5!;
        parameters[5]  = arg6!;
        parameters[6]  = arg7!;
        parameters[7]  = arg8!;
        parameters[8]  = arg9!;
        parameters[9]  = arg10!;
        parameters[10] = arg11!;
        OnUserInvocation(parameters);
    }

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0]  = arg1!;
        parameters[1]  = arg2!;
        parameters[2]  = arg3!;
        parameters[3]  = arg4!;
        parameters[4]  = arg5!;
        parameters[5]  = arg6!;
        parameters[6]  = arg7!;
        parameters[7]  = arg8!;
        parameters[8]  = arg9!;
        parameters[9]  = arg10!;
        parameters[10] = arg11!;
        parameters[11] = arg12!;
        OnUserInvocation(parameters);
    }

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11,
                                                                                          T12 arg12, T13 arg13) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0]  = arg1!;
        parameters[1]  = arg2!;
        parameters[2]  = arg3!;
        parameters[3]  = arg4!;
        parameters[4]  = arg5!;
        parameters[5]  = arg6!;
        parameters[6]  = arg7!;
        parameters[7]  = arg8!;
        parameters[8]  = arg9!;
        parameters[9]  = arg10!;
        parameters[10] = arg11!;
        parameters[11] = arg12!;
        parameters[12] = arg13!;
        OnUserInvocation(parameters);
    }

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11,
                                                                                               T12 arg12, T13 arg13, T14 arg14) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0]  = arg1!;
        parameters[1]  = arg2!;
        parameters[2]  = arg3!;
        parameters[3]  = arg4!;
        parameters[4]  = arg5!;
        parameters[5]  = arg6!;
        parameters[6]  = arg7!;
        parameters[7]  = arg8!;
        parameters[8]  = arg9!;
        parameters[9]  = arg10!;
        parameters[10] = arg11!;
        parameters[11] = arg12!;
        parameters[12] = arg13!;
        parameters[13] = arg14!;
        OnUserInvocation(parameters);
    }

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
                                                                                                    T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0]  = arg1!;
        parameters[1]  = arg2!;
        parameters[2]  = arg3!;
        parameters[3]  = arg4!;
        parameters[4]  = arg5!;
        parameters[5]  = arg6!;
        parameters[6]  = arg7!;
        parameters[7]  = arg8!;
        parameters[8]  = arg9!;
        parameters[9]  = arg10!;
        parameters[10] = arg11!;
        parameters[11] = arg12!;
        parameters[12] = arg13!;
        parameters[13] = arg14!;
        parameters[14] = arg15!;
        OnUserInvocation(parameters);
    }

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
                                                                                                         T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0]  = arg1!;
        parameters[1]  = arg2!;
        parameters[2]  = arg3!;
        parameters[3]  = arg4!;
        parameters[4]  = arg5!;
        parameters[5]  = arg6!;
        parameters[6]  = arg7!;
        parameters[7]  = arg8!;
        parameters[8]  = arg9!;
        parameters[9]  = arg10!;
        parameters[10] = arg11!;
        parameters[11] = arg12!;
        parameters[12] = arg13!;
        parameters[13] = arg14!;
        parameters[14] = arg15!;
        parameters[15] = arg16!;
        OnUserInvocation(parameters);
    }

    TResult? RateLimitedFunc<T1, TResult>.Invoke(T1 arg1) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        return OnUserInvocation(parameters);
    }

    TResult? RateLimitedFunc<T1, T2, TResult>.Invoke(T1 arg1, T2 arg2) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        return OnUserInvocation(parameters);
    }

    TResult? RateLimitedFunc<T1, T2, T3, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        parameters[2] = arg3!;
        return OnUserInvocation(parameters);
    }

    TResult? RateLimitedFunc<T1, T2, T3, T4, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        parameters[2] = arg3!;
        parameters[3] = arg4!;
        return OnUserInvocation(parameters);
    }

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        parameters[2] = arg3!;
        parameters[3] = arg4!;
        parameters[4] = arg5!;
        return OnUserInvocation(parameters);
    }

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        parameters[2] = arg3!;
        parameters[3] = arg4!;
        parameters[4] = arg5!;
        parameters[5] = arg6!;
        return OnUserInvocation(parameters);
    }

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        parameters[2] = arg3!;
        parameters[3] = arg4!;
        parameters[4] = arg5!;
        parameters[5] = arg6!;
        parameters[6] = arg7!;
        return OnUserInvocation(parameters);
    }

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        parameters[2] = arg3!;
        parameters[3] = arg4!;
        parameters[4] = arg5!;
        parameters[5] = arg6!;
        parameters[6] = arg7!;
        parameters[7] = arg8!;
        return OnUserInvocation(parameters);
    }

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        parameters[2] = arg3!;
        parameters[3] = arg4!;
        parameters[4] = arg5!;
        parameters[5] = arg6!;
        parameters[6] = arg7!;
        parameters[7] = arg8!;
        parameters[8] = arg9!;
        return OnUserInvocation(parameters);
    }

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0] = arg1!;
        parameters[1] = arg2!;
        parameters[2] = arg3!;
        parameters[3] = arg4!;
        parameters[4] = arg5!;
        parameters[5] = arg6!;
        parameters[6] = arg7!;
        parameters[7] = arg8!;
        parameters[8] = arg9!;
        parameters[9] = arg10!;
        return OnUserInvocation(parameters);
    }

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0]  = arg1!;
        parameters[1]  = arg2!;
        parameters[2]  = arg3!;
        parameters[3]  = arg4!;
        parameters[4]  = arg5!;
        parameters[5]  = arg6!;
        parameters[6]  = arg7!;
        parameters[7]  = arg8!;
        parameters[8]  = arg9!;
        parameters[9]  = arg10!;
        parameters[10] = arg11!;
        return OnUserInvocation(parameters);
    }

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11,
                                                                                                T12 arg12) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0]  = arg1!;
        parameters[1]  = arg2!;
        parameters[2]  = arg3!;
        parameters[3]  = arg4!;
        parameters[4]  = arg5!;
        parameters[5]  = arg6!;
        parameters[6]  = arg7!;
        parameters[7]  = arg8!;
        parameters[8]  = arg9!;
        parameters[9]  = arg10!;
        parameters[10] = arg11!;
        parameters[11] = arg12!;
        return OnUserInvocation(parameters);
    }

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
                                                                                                     T11 arg11, T12 arg12, T13 arg13) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0]  = arg1!;
        parameters[1]  = arg2!;
        parameters[2]  = arg3!;
        parameters[3]  = arg4!;
        parameters[4]  = arg5!;
        parameters[5]  = arg6!;
        parameters[6]  = arg7!;
        parameters[7]  = arg8!;
        parameters[8]  = arg9!;
        parameters[9]  = arg10!;
        parameters[10] = arg11!;
        parameters[11] = arg12!;
        parameters[12] = arg13!;
        return OnUserInvocation(parameters);
    }

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
                                                                                                          T11 arg11, T12 arg12, T13 arg13, T14 arg14) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0]  = arg1!;
        parameters[1]  = arg2!;
        parameters[2]  = arg3!;
        parameters[3]  = arg4!;
        parameters[4]  = arg5!;
        parameters[5]  = arg6!;
        parameters[6]  = arg7!;
        parameters[7]  = arg8!;
        parameters[8]  = arg9!;
        parameters[9]  = arg10!;
        parameters[10] = arg11!;
        parameters[11] = arg12!;
        parameters[12] = arg13!;
        parameters[13] = arg14!;
        return OnUserInvocation(parameters);
    }

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                                                                                                               T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0]  = arg1!;
        parameters[1]  = arg2!;
        parameters[2]  = arg3!;
        parameters[3]  = arg4!;
        parameters[4]  = arg5!;
        parameters[5]  = arg6!;
        parameters[6]  = arg7!;
        parameters[7]  = arg8!;
        parameters[8]  = arg9!;
        parameters[9]  = arg10!;
        parameters[10] = arg11!;
        parameters[11] = arg12!;
        parameters[12] = arg13!;
        parameters[13] = arg14!;
        parameters[14] = arg15!;
        return OnUserInvocation(parameters);
    }

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                                                                                                                    T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) {
        object[] parameters = parameterArrayPool.Borrow();
        parameters[0]  = arg1!;
        parameters[1]  = arg2!;
        parameters[2]  = arg3!;
        parameters[3]  = arg4!;
        parameters[4]  = arg5!;
        parameters[5]  = arg6!;
        parameters[6]  = arg7!;
        parameters[7]  = arg8!;
        parameters[8]  = arg9!;
        parameters[9]  = arg10!;
        parameters[10] = arg11!;
        parameters[11] = arg12!;
        parameters[12] = arg13!;
        parameters[13] = arg14!;
        parameters[14] = arg15!;
        parameters[15] = arg16!;
        return OnUserInvocation(parameters);
    }

}