using System;

namespace ThrottleDebounce {

    public interface DebouncedAction: IDisposable {

        void Run();

    }

    public interface DebouncedAction<in T1>: IDisposable {

        void Run(T1 arg1);

    }

    public interface DebouncedAction<in T1, in T2>: IDisposable {

        void Run(T1 arg1, T2 arg2);

    }

    public interface DebouncedAction<in T1, in T2, in T3>: IDisposable {

        void Run(T1 arg1, T2 arg2, T3 arg3);

    }

    public interface DebouncedAction<in T1, in T2, in T3, in T4>: IDisposable {

        void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

    }

    public interface DebouncedAction<in T1, in T2, in T3, in T4, in T5>: IDisposable {

        void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

    }

    public interface DebouncedAction<in T1, in T2, in T3, in T4, in T5, in T6>: IDisposable {

        void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

    }

    public interface DebouncedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7>: IDisposable {

        void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

    }

    public interface DebouncedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8>: IDisposable {

        void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

    }

    public interface DebouncedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9>: IDisposable {

        void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

    }

    public interface DebouncedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10>: IDisposable {

        void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

    }

    public interface DebouncedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11>: IDisposable {

        void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);

    }

    public interface DebouncedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12>: IDisposable {

        void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);

    }

    public interface DebouncedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13>: IDisposable {

        void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);

    }

    public interface DebouncedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14>: IDisposable {

        void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);

    }

    public interface DebouncedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15>: IDisposable {

        void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);

    }

    public interface DebouncedAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16>: IDisposable {

        void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);

    }

    public interface DebouncedFunc<out TResult>: IDisposable {

        TResult Call();

    }

    public interface DebouncedFunc<in T1, out TResult>: IDisposable {

        TResult Call(T1 arg1);

    }

    public interface DebouncedFunc<in T1, in T2, out TResult>: IDisposable {

        TResult Call(T1 arg1, T2 arg2);

    }

    public interface DebouncedFunc<in T1, in T2, in T3, out TResult>: IDisposable {

        TResult Call(T1 arg1, T2 arg2, T3 arg3);

    }

    public interface DebouncedFunc<in T1, in T2, in T3, in T4, out TResult>: IDisposable {

        TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

    }

    public interface DebouncedFunc<in T1, in T2, in T3, in T4, in T5, out TResult>: IDisposable {

        TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

    }

    public interface DebouncedFunc<in T1, in T2, in T3, in T4, in T5, in T6, out TResult>: IDisposable {

        TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

    }

    public interface DebouncedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, out TResult>: IDisposable {

        TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

    }

    public interface DebouncedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, out TResult>: IDisposable {

        TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

    }

    public interface DebouncedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, out TResult>: IDisposable {

        TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

    }

    public interface DebouncedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, out TResult>: IDisposable {

        TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

    }

    public interface DebouncedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, out TResult>: IDisposable {

        TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);

    }

    public interface DebouncedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, out TResult>: IDisposable {

        TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);

    }

    public interface DebouncedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, out TResult>: IDisposable {

        TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);

    }

    public interface DebouncedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, out TResult>: IDisposable {

        TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);

    }

    public interface DebouncedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, out TResult>: IDisposable {

        TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);

    }

    public interface DebouncedFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16, out TResult>: IDisposable {

        TResult Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);

    }

}