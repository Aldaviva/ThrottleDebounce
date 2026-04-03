#nullable enable

namespace ThrottleDebounce;

internal sealed partial class RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> {

    /**
     * Faster than Delegate.DynamicInvoke(object[]) by 6.0–8.3×, depending on the argument count
     */
    private TResult InvokeCallback(dynamic[] args) {
        dynamic callback  = _rateLimitedCallback;
        bool    hasReturn = typeof(TResult) != typeof(Void);
        switch (args.Length) {
            case 0:
                if (hasReturn) return callback();
                callback();
                break;
            case 1:
                if (hasReturn) return callback(args[0]);
                callback(args[0]);
                break;
            case 2:
                if (hasReturn) return callback(args[0], args[1]);
                callback(args[0], args[1]);
                break;
            case 3:
                if (hasReturn) return callback(args[0], args[1], args[2]);
                callback(args[0], args[1], args[2]);
                break;
            case 4:
                if (hasReturn) return callback(args[0], args[1], args[2], args[3]);
                callback(args[0], args[1], args[2], args[3]);
                break;
            case 5:
                if (hasReturn) return callback(args[0], args[1], args[2], args[3], args[4]);
                callback(args[0], args[1], args[2], args[3], args[4]);
                break;
            case 6:
                if (hasReturn) return callback(args[0], args[1], args[2], args[3], args[4], args[5]);
                callback(args[0], args[1], args[2], args[3], args[4], args[5]);
                break;
            case 7:
                if (hasReturn) return callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                break;
            case 8:
                if (hasReturn) return callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                break;
            case 9:
                if (hasReturn) return callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                break;
            case 10:
                if (hasReturn) return callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                break;
            case 11:
                if (hasReturn) return callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]);
                callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]);
                break;
            case 12:
                if (hasReturn) return callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]);
                callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]);
                break;
            case 13:
                if (hasReturn) return callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]);
                callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]);
                break;
            case 14:
                if (hasReturn) return callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13]);
                callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13]);
                break;
            case 15:
                if (hasReturn) return callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14]);
                callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14]);
                break;
            case 16:
                if (hasReturn) return callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15]);
                callback(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15]);
                break;
        }
        return default!;
    }

}