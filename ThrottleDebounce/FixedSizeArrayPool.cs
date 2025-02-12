#nullable enable

using System.Collections.Concurrent;
using System.Threading;

namespace ThrottleDebounce;

internal class FixedSizeArrayPool<T>(int arrayLength, int preferredPoolSize) {

    private readonly ConcurrentStack<T[]> _available = new();

    private int BorrowedCount;

    public T[] Borrow() {
        if (!_available.TryPop(out T[] toLend)) {
            toLend = new T[arrayLength];
        }
        Interlocked.Increment(ref BorrowedCount);
        return toLend;
    }

    public void Return(T[] borrowed) {
        int borrowedCountAfterReturn = Interlocked.Decrement(ref BorrowedCount);
        while (borrowedCountAfterReturn < 0 && borrowedCountAfterReturn != Interlocked.CompareExchange(ref BorrowedCount, 0, borrowedCountAfterReturn)) {
            borrowedCountAfterReturn = BorrowedCount;
        }

        if (borrowedCountAfterReturn + AvailableCount < preferredPoolSize) {
            _available.Push(borrowed);
        }
    }

    private int AvailableCount => _available.Count;

}