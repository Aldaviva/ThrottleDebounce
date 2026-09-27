namespace Tests;

public class OverloadTests: BaseTest {

    [Fact]
    public void ThrottleAction1Inputs() {
        Action<int> rateLimited = Throttler.Throttle<int>(arg1 => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8);
        rateLimited(8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleAction2Inputs() {
        Action<int, int> rateLimited = Throttler.Throttle<int, int>((arg1, arg2) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8);
        rateLimited(8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleAction3Inputs() {
        Action<int, int, int> rateLimited = Throttler.Throttle<int, int, int>((arg1, arg2, arg3) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8);
        rateLimited(8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleAction4Inputs() {
        Action<int, int, int, int> rateLimited = Throttler.Throttle<int, int, int, int>((arg1, arg2, arg3, arg4) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8);
        rateLimited(8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleAction5Inputs() {
        Action<int, int, int, int, int> rateLimited = Throttler.Throttle<int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleAction6Inputs() {
        Action<int, int, int, int, int, int> rateLimited = Throttler.Throttle<int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleAction7Inputs() {
        Action<int, int, int, int, int, int, int> rateLimited = Throttler.Throttle<int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7) => { ++ExecutionCount; }, WaitTime)
            .Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleAction8Inputs() {
        Action<int, int, int, int, int, int, int, int> rateLimited =
            Throttler.Throttle<int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleAction9Inputs() {
        Action<int, int, int, int, int, int, int, int, int> rateLimited =
            Throttler.Throttle<int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleAction10Inputs() {
        Action<int, int, int, int, int, int, int, int, int, int> rateLimited = Throttler
            .Throttle<int, int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleAction11Inputs() {
        Action<int, int, int, int, int, int, int, int, int, int, int> rateLimited = Throttler
            .Throttle<int, int, int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleAction12Inputs() {
        Action<int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Throttler
            .Throttle<int, int, int, int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) => { ++ExecutionCount; }, WaitTime)
            .Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleAction13Inputs() {
        Action<int, int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Throttler
            .Throttle<int, int, int, int, int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) => { ++ExecutionCount; },
                WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleAction14Inputs() {
        Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Throttler
            .Throttle<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleAction15Inputs() {
        Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Throttler
            .Throttle<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleAction16Inputs() {
        Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Throttler
            .Throttle<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleFunc1Inputs() {
        Func<int, string?> rateLimited = Throttler.Throttle<int, string?>(arg1 => {
            ++ExecutionCount;
            return "";
        }, WaitTime).Invoke;
        rateLimited(8);
        rateLimited(8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleFunc2Inputs() {
        Func<int, int, string?> rateLimited = Throttler.Throttle<int, int, string?>((arg1, arg2) => {
            ++ExecutionCount;
            return "";
        }, WaitTime).Invoke;
        rateLimited(8, 8);
        rateLimited(8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleFunc3Inputs() {
        Func<int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, string?>((arg1, arg2, arg3) => {
            ++ExecutionCount;
            return "";
        }, WaitTime).Invoke;
        rateLimited(8, 8, 8);
        rateLimited(8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleFunc4Inputs() {
        Func<int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, string?>((arg1, arg2, arg3, arg4) => {
            ++ExecutionCount;
            return "";
        }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8);
        rateLimited(8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleFunc5Inputs() {
        Func<int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, string?>((arg1, arg2, arg3, arg4, arg5) => {
            ++ExecutionCount;
            return "";
        }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleFunc6Inputs() {
        Func<int, int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, int, string?>((arg1, arg2, arg3, arg4, arg5, arg6) => {
            ++ExecutionCount;
            return "";
        }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleFunc7Inputs() {
        Func<int, int, int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, int, int, string?>((arg1, arg2, arg3, arg4, arg5, arg6, arg7) => {
            ++ExecutionCount;
            return "";
        }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleFunc8Inputs() {
        Func<int, int, int, int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, int, int, int, string?>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => {
            ++ExecutionCount;
            return "";
        }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleFunc9Inputs() {
        Func<int, int, int, int, int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, int, int, int, int, string?>(
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => {
                ++ExecutionCount;
                return "";
            }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleFunc10Inputs() {
        Func<int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, int, int, int, int, int, string?>(
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => {
                ++ExecutionCount;
                return "";
            }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleFunc11Inputs() {
        Func<int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, int, int, int, int, int, int, string?>(
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => {
                ++ExecutionCount;
                return "";
            }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleFunc12Inputs() {
        Func<int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, int, int, int, int, int, int, int, string?>(
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) => {
                ++ExecutionCount;
                return "";
            }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleFunc13Inputs() {
        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, int, int, int, int, int, int, int, int, string?>(
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) => {
                ++ExecutionCount;
                return "";
            }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleFunc14Inputs() {
        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Throttler
            .Throttle<int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) => {
                ++ExecutionCount;
                return "";
            }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleFunc15Inputs() {
        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Throttler
            .Throttle<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?>(
                (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) => {
                    ++ExecutionCount;
                    return "";
                }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void ThrottleFunc16Inputs() {
        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Throttler
            .Throttle<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?>(
                (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) => {
                    ++ExecutionCount;
                    return "";
                }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(1);
    }

    [Fact]
    public void DebounceAction1Inputs() {
        Action<int> rateLimited = Debouncer.Debounce<int>(arg1 => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8);
        rateLimited(8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceAction2Inputs() {
        Action<int, int> rateLimited = Debouncer.Debounce<int, int>((arg1, arg2) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8);
        rateLimited(8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceAction3Inputs() {
        Action<int, int, int> rateLimited = Debouncer.Debounce<int, int, int>((arg1, arg2, arg3) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8);
        rateLimited(8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceAction4Inputs() {
        Action<int, int, int, int> rateLimited = Debouncer.Debounce<int, int, int, int>((arg1, arg2, arg3, arg4) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8);
        rateLimited(8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceAction5Inputs() {
        Action<int, int, int, int, int> rateLimited = Debouncer.Debounce<int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceAction6Inputs() {
        Action<int, int, int, int, int, int> rateLimited = Debouncer.Debounce<int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceAction7Inputs() {
        Action<int, int, int, int, int, int, int> rateLimited = Debouncer.Debounce<int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7) => { ++ExecutionCount; }, WaitTime)
            .Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceAction8Inputs() {
        Action<int, int, int, int, int, int, int, int> rateLimited =
            Debouncer.Debounce<int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceAction9Inputs() {
        Action<int, int, int, int, int, int, int, int, int> rateLimited =
            Debouncer.Debounce<int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceAction10Inputs() {
        Action<int, int, int, int, int, int, int, int, int, int> rateLimited = Debouncer
            .Debounce<int, int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceAction11Inputs() {
        Action<int, int, int, int, int, int, int, int, int, int, int> rateLimited = Debouncer
            .Debounce<int, int, int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceAction12Inputs() {
        Action<int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Debouncer
            .Debounce<int, int, int, int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) => { ++ExecutionCount; }, WaitTime)
            .Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceAction13Inputs() {
        Action<int, int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Debouncer
            .Debounce<int, int, int, int, int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) => { ++ExecutionCount; },
                WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceAction14Inputs() {
        Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Debouncer
            .Debounce<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceAction15Inputs() {
        Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Debouncer
            .Debounce<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceAction16Inputs() {
        Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Debouncer
            .Debounce<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) => { ++ExecutionCount; }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceFunc1Inputs() {
        Func<int, string?> rateLimited = Debouncer.Debounce<int, string?>(arg1 => {
            ++ExecutionCount;
            return "";
        }, WaitTime).Invoke;
        rateLimited(8);
        rateLimited(8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceFunc2Inputs() {
        Func<int, int, string?> rateLimited = Debouncer.Debounce<int, int, string?>((arg1, arg2) => {
            ++ExecutionCount;
            return "";
        }, WaitTime).Invoke;
        rateLimited(8, 8);
        rateLimited(8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceFunc3Inputs() {
        Func<int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, string?>((arg1, arg2, arg3) => {
            ++ExecutionCount;
            return "";
        }, WaitTime).Invoke;
        rateLimited(8, 8, 8);
        rateLimited(8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceFunc4Inputs() {
        Func<int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, string?>((arg1, arg2, arg3, arg4) => {
            ++ExecutionCount;
            return "";
        }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8);
        rateLimited(8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceFunc5Inputs() {
        Func<int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, string?>((arg1, arg2, arg3, arg4, arg5) => {
            ++ExecutionCount;
            return "";
        }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceFunc6Inputs() {
        Func<int, int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, int, string?>((arg1, arg2, arg3, arg4, arg5, arg6) => {
            ++ExecutionCount;
            return "";
        }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceFunc7Inputs() {
        Func<int, int, int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, int, int, string?>((arg1, arg2, arg3, arg4, arg5, arg6, arg7) => {
            ++ExecutionCount;
            return "";
        }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceFunc8Inputs() {
        Func<int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, int, int, int, string?>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => {
            ++ExecutionCount;
            return "";
        }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceFunc9Inputs() {
        Func<int, int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, int, int, int, int, string?>(
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => {
                ++ExecutionCount;
                return "";
            }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceFunc10Inputs() {
        Func<int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, int, int, int, int, int, string?>(
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => {
                ++ExecutionCount;
                return "";
            }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceFunc11Inputs() {
        Func<int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, int, int, int, int, int, int, string?>(
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => {
                ++ExecutionCount;
                return "";
            }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceFunc12Inputs() {
        Func<int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, int, int, int, int, int, int, int, string?>(
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) => {
                ++ExecutionCount;
                return "";
            }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceFunc13Inputs() {
        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, int, int, int, int, int, int, int, int, string?>(
            (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) => {
                ++ExecutionCount;
                return "";
            }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceFunc14Inputs() {
        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer
            .Debounce<int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) => {
                ++ExecutionCount;
                return "";
            }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceFunc15Inputs() {
        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer
            .Debounce<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?>(
                (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) => {
                    ++ExecutionCount;
                    return "";
                }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

    [Fact]
    public void DebounceFunc16Inputs() {
        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer
            .Debounce<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?>(
                (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) => {
                    ++ExecutionCount;
                    return "";
                }, WaitTime).Invoke;
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
        ExecutionCount.Should().Be(0);
    }

}