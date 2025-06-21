#nullable enable

using System.Collections.Concurrent;
using System.Threading;

namespace ThrottleDebounce;

internal class FixedSizeArrayPool<T>(int arrayLength, int preferredPoolSize) {

    private readonly ConcurrentStack<T[]> _available = new();

    private int _borrowedCount;

    public T[] Borrow() {
        if (!_available.TryPop(out T[] toLend)) {
            toLend = new T[arrayLength];
        }
        Interlocked.Increment(ref _borrowedCount);
        return toLend;
    }

    public void Return(T[] borrowed) {
        int borrowedCountAfterReturn = Interlocked.Decrement(ref _borrowedCount);
        while (borrowedCountAfterReturn < 0 && borrowedCountAfterReturn != Interlocked.CompareExchange(ref _borrowedCount, 0, borrowedCountAfterReturn)) {
            borrowedCountAfterReturn = _borrowedCount;
        }

        if (borrowedCountAfterReturn + AvailableCount < preferredPoolSize) {
            _available.Push(borrowed);
        }
    }

    private int AvailableCount => _available.Count;

}