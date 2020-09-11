using System;
using FluentAssertions;
using ThrottleDebounce;
using Xunit;

namespace Tests {

    public class OverloadTests: BaseTest {

        public class ThrottleAction0InputsClass: OverloadTests {

            [Fact]
            public void ThrottleAction0Inputs() {
                Action rateLimited = Throttler.Throttle(() => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited();
                rateLimited();
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleFunc0InputsClass: OverloadTests {

            [Fact]
            public void ThrottleFunc0Inputs() {
                Func<string?> rateLimited = Throttler.Throttle(() => {
                    ++executionCount;
                    return "";
                }, WAIT_TIME).Invoke;
                rateLimited();
                rateLimited();
                executionCount.Should().Be(1);
            }

        }

        public class DebounceAction0InputsClass: OverloadTests {

            [Fact]
            public void DebounceAction0Inputs() {
                Action rateLimited = Debouncer.Debounce(() => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited();
                rateLimited();
                executionCount.Should().Be(0);
            }

        }

        public class DebounceFunc0InputsClass: OverloadTests {

            [Fact]
            public void DebounceFunc0Inputs() {
                Func<string?> rateLimited = Debouncer.Debounce(() => {
                    ++executionCount;
                    return "";
                }, WAIT_TIME).Invoke;
                rateLimited();
                rateLimited();
                executionCount.Should().Be(0);
            }

        }

        public class ThrottleAction1InputsClass: OverloadTests {

            [Fact]
            public void ThrottleAction1Inputs() {
                Action<int> rateLimited = Throttler.Throttle<int>(arg1 => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8);
                rateLimited(8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleAction2InputsClass: OverloadTests {

            [Fact]
            public void ThrottleAction2Inputs() {
                Action<int, int> rateLimited = Throttler.Throttle<int, int>((arg1, arg2) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8);
                rateLimited(8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleAction3InputsClass: OverloadTests {

            [Fact]
            public void ThrottleAction3Inputs() {
                Action<int, int, int> rateLimited = Throttler.Throttle<int, int, int>((arg1, arg2, arg3) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8);
                rateLimited(8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleAction4InputsClass: OverloadTests {

            [Fact]
            public void ThrottleAction4Inputs() {
                Action<int, int, int, int> rateLimited = Throttler.Throttle<int, int, int, int>((arg1, arg2, arg3, arg4) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8);
                rateLimited(8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleAction5InputsClass: OverloadTests {

            [Fact]
            public void ThrottleAction5Inputs() {
                Action<int, int, int, int, int> rateLimited = Throttler.Throttle<int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleAction6InputsClass: OverloadTests {

            [Fact]
            public void ThrottleAction6Inputs() {
                Action<int, int, int, int, int, int> rateLimited = Throttler.Throttle<int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6) => { ++executionCount; }, WAIT_TIME)
                    .Invoke;
                rateLimited(8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleAction7InputsClass: OverloadTests {

            [Fact]
            public void ThrottleAction7Inputs() {
                Action<int, int, int, int, int, int, int> rateLimited =
                    Throttler.Throttle<int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleAction8InputsClass: OverloadTests {

            [Fact]
            public void ThrottleAction8Inputs() {
                Action<int, int, int, int, int, int, int, int> rateLimited = Throttler
                    .Throttle<int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleAction9InputsClass: OverloadTests {

            [Fact]
            public void ThrottleAction9Inputs() {
                Action<int, int, int, int, int, int, int, int, int> rateLimited = Throttler
                    .Throttle<int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleAction10InputsClass: OverloadTests {

            [Fact]
            public void ThrottleAction10Inputs() {
                Action<int, int, int, int, int, int, int, int, int, int> rateLimited = Throttler
                    .Throttle<int, int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleAction11InputsClass: OverloadTests {

            [Fact]
            public void ThrottleAction11Inputs() {
                Action<int, int, int, int, int, int, int, int, int, int, int> rateLimited = Throttler
                    .Throttle<int, int, int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => { ++executionCount; }, WAIT_TIME)
                    .Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleAction12InputsClass: OverloadTests {

            [Fact]
            public void ThrottleAction12Inputs() {
                Action<int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Throttler
                    .Throttle<int, int, int, int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) => { ++executionCount; },
                        WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleAction13InputsClass: OverloadTests {

            [Fact]
            public void ThrottleAction13Inputs() {
                Action<int, int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Throttler
                    .Throttle<int, int, int, int, int, int, int, int, int, int, int, int, int>(
                        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleAction14InputsClass: OverloadTests {

            [Fact]
            public void ThrottleAction14Inputs() {
                Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Throttler
                    .Throttle<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleAction15InputsClass: OverloadTests {

            [Fact]
            public void ThrottleAction15Inputs() {
                Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Throttler
                    .Throttle<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleAction16InputsClass: OverloadTests {

            [Fact]
            public void ThrottleAction16Inputs() {
                Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Throttler
                    .Throttle<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleFunc1InputsClass: OverloadTests {

            [Fact]
            public void ThrottleFunc1Inputs() {
                Func<int, string?> rateLimited = Throttler.Throttle<int, string>(arg1 => {
                    ++executionCount;
                    return "";
                }, WAIT_TIME).Invoke;
                rateLimited(8);
                rateLimited(8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleFunc2InputsClass: OverloadTests {

            [Fact]
            public void ThrottleFunc2Inputs() {
                Func<int, int, string?> rateLimited = Throttler.Throttle<int, int, string>((arg1, arg2) => {
                    ++executionCount;
                    return "";
                }, WAIT_TIME).Invoke;
                rateLimited(8, 8);
                rateLimited(8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleFunc3InputsClass: OverloadTests {

            [Fact]
            public void ThrottleFunc3Inputs() {
                Func<int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, string>((arg1, arg2, arg3) => {
                    ++executionCount;
                    return "";
                }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8);
                rateLimited(8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleFunc4InputsClass: OverloadTests {

            [Fact]
            public void ThrottleFunc4Inputs() {
                Func<int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, string>((arg1, arg2, arg3, arg4) => {
                    ++executionCount;
                    return "";
                }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8);
                rateLimited(8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleFunc5InputsClass: OverloadTests {

            [Fact]
            public void ThrottleFunc5Inputs() {
                Func<int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, string>((arg1, arg2, arg3, arg4, arg5) => {
                    ++executionCount;
                    return "";
                }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleFunc6InputsClass: OverloadTests {

            [Fact]
            public void ThrottleFunc6Inputs() {
                Func<int, int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, int, string>((arg1, arg2, arg3, arg4, arg5, arg6) => {
                    ++executionCount;
                    return "";
                }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleFunc7InputsClass: OverloadTests {

            [Fact]
            public void ThrottleFunc7Inputs() {
                Func<int, int, int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, int, int, string>((arg1, arg2, arg3, arg4, arg5, arg6, arg7) => {
                    ++executionCount;
                    return "";
                }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleFunc8InputsClass: OverloadTests {

            [Fact]
            public void ThrottleFunc8Inputs() {
                Func<int, int, int, int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, int, int, int, string>(
                    (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => {
                        ++executionCount;
                        return "";
                    }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleFunc9InputsClass: OverloadTests {

            [Fact]
            public void ThrottleFunc9Inputs() {
                Func<int, int, int, int, int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, int, int, int, int, string>(
                    (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => {
                        ++executionCount;
                        return "";
                    }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleFunc10InputsClass: OverloadTests {

            [Fact]
            public void ThrottleFunc10Inputs() {
                Func<int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, int, int, int, int, int, string>(
                    (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => {
                        ++executionCount;
                        return "";
                    }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleFunc11InputsClass: OverloadTests {

            [Fact]
            public void ThrottleFunc11Inputs() {
                Func<int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, int, int, int, int, int, int, string>(
                    (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => {
                        ++executionCount;
                        return "";
                    }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleFunc12InputsClass: OverloadTests {

            [Fact]
            public void ThrottleFunc12Inputs() {
                Func<int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, int, int, int, int, int, int, int, string>(
                    (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) => {
                        ++executionCount;
                        return "";
                    }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleFunc13InputsClass: OverloadTests {

            [Fact]
            public void ThrottleFunc13Inputs() {
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Throttler.Throttle<int, int, int, int, int, int, int, int, int, int, int, int, int, string>(
                    (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) => {
                        ++executionCount;
                        return "";
                    }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleFunc14InputsClass: OverloadTests {

            [Fact]
            public void ThrottleFunc14Inputs() {
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Throttler
                    .Throttle<int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(
                        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) => {
                            ++executionCount;
                            return "";
                        }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleFunc15InputsClass: OverloadTests {

            [Fact]
            public void ThrottleFunc15Inputs() {
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Throttler
                    .Throttle<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(
                        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) => {
                            ++executionCount;
                            return "";
                        }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class ThrottleFunc16InputsClass: OverloadTests {

            [Fact]
            public void ThrottleFunc16Inputs() {
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Throttler
                    .Throttle<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(
                        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) => {
                            ++executionCount;
                            return "";
                        }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(1);
            }

        }

        public class DebounceAction1InputsClass: OverloadTests {

            [Fact]
            public void DebounceAction1Inputs() {
                Action<int> rateLimited = Debouncer.Debounce<int>(arg1 => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8);
                rateLimited(8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceAction2InputsClass: OverloadTests {

            [Fact]
            public void DebounceAction2Inputs() {
                Action<int, int> rateLimited = Debouncer.Debounce<int, int>((arg1, arg2) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8);
                rateLimited(8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceAction3InputsClass: OverloadTests {

            [Fact]
            public void DebounceAction3Inputs() {
                Action<int, int, int> rateLimited = Debouncer.Debounce<int, int, int>((arg1, arg2, arg3) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8);
                rateLimited(8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceAction4InputsClass: OverloadTests {

            [Fact]
            public void DebounceAction4Inputs() {
                Action<int, int, int, int> rateLimited = Debouncer.Debounce<int, int, int, int>((arg1, arg2, arg3, arg4) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8);
                rateLimited(8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceAction5InputsClass: OverloadTests {

            [Fact]
            public void DebounceAction5Inputs() {
                Action<int, int, int, int, int> rateLimited = Debouncer.Debounce<int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceAction6InputsClass: OverloadTests {

            [Fact]
            public void DebounceAction6Inputs() {
                Action<int, int, int, int, int, int> rateLimited = Debouncer.Debounce<int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6) => { ++executionCount; }, WAIT_TIME)
                    .Invoke;
                rateLimited(8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceAction7InputsClass: OverloadTests {

            [Fact]
            public void DebounceAction7Inputs() {
                Action<int, int, int, int, int, int, int> rateLimited =
                    Debouncer.Debounce<int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceAction8InputsClass: OverloadTests {

            [Fact]
            public void DebounceAction8Inputs() {
                Action<int, int, int, int, int, int, int, int> rateLimited = Debouncer
                    .Debounce<int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceAction9InputsClass: OverloadTests {

            [Fact]
            public void DebounceAction9Inputs() {
                Action<int, int, int, int, int, int, int, int, int> rateLimited = Debouncer
                    .Debounce<int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceAction10InputsClass: OverloadTests {

            [Fact]
            public void DebounceAction10Inputs() {
                Action<int, int, int, int, int, int, int, int, int, int> rateLimited = Debouncer
                    .Debounce<int, int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceAction11InputsClass: OverloadTests {

            [Fact]
            public void DebounceAction11Inputs() {
                Action<int, int, int, int, int, int, int, int, int, int, int> rateLimited = Debouncer
                    .Debounce<int, int, int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => { ++executionCount; }, WAIT_TIME)
                    .Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceAction12InputsClass: OverloadTests {

            [Fact]
            public void DebounceAction12Inputs() {
                Action<int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Debouncer
                    .Debounce<int, int, int, int, int, int, int, int, int, int, int, int>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) => { ++executionCount; },
                        WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceAction13InputsClass: OverloadTests {

            [Fact]
            public void DebounceAction13Inputs() {
                Action<int, int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Debouncer
                    .Debounce<int, int, int, int, int, int, int, int, int, int, int, int, int>(
                        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceAction14InputsClass: OverloadTests {

            [Fact]
            public void DebounceAction14Inputs() {
                Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Debouncer
                    .Debounce<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceAction15InputsClass: OverloadTests {

            [Fact]
            public void DebounceAction15Inputs() {
                Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Debouncer
                    .Debounce<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceAction16InputsClass: OverloadTests {

            [Fact]
            public void DebounceAction16Inputs() {
                Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> rateLimited = Debouncer
                    .Debounce<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) => { ++executionCount; }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceFunc1InputsClass: OverloadTests {

            [Fact]
            public void DebounceFunc1Inputs() {
                Func<int, string?> rateLimited = Debouncer.Debounce<int, string>(arg1 => {
                    ++executionCount;
                    return "";
                }, WAIT_TIME).Invoke;
                rateLimited(8);
                rateLimited(8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceFunc2InputsClass: OverloadTests {

            [Fact]
            public void DebounceFunc2Inputs() {
                Func<int, int, string?> rateLimited = Debouncer.Debounce<int, int, string>((arg1, arg2) => {
                    ++executionCount;
                    return "";
                }, WAIT_TIME).Invoke;
                rateLimited(8, 8);
                rateLimited(8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceFunc3InputsClass: OverloadTests {

            [Fact]
            public void DebounceFunc3Inputs() {
                Func<int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, string>((arg1, arg2, arg3) => {
                    ++executionCount;
                    return "";
                }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8);
                rateLimited(8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceFunc4InputsClass: OverloadTests {

            [Fact]
            public void DebounceFunc4Inputs() {
                Func<int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, string>((arg1, arg2, arg3, arg4) => {
                    ++executionCount;
                    return "";
                }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8);
                rateLimited(8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceFunc5InputsClass: OverloadTests {

            [Fact]
            public void DebounceFunc5Inputs() {
                Func<int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, string>((arg1, arg2, arg3, arg4, arg5) => {
                    ++executionCount;
                    return "";
                }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceFunc6InputsClass: OverloadTests {

            [Fact]
            public void DebounceFunc6Inputs() {
                Func<int, int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, int, string>((arg1, arg2, arg3, arg4, arg5, arg6) => {
                    ++executionCount;
                    return "";
                }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceFunc7InputsClass: OverloadTests {

            [Fact]
            public void DebounceFunc7Inputs() {
                Func<int, int, int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, int, int, string>((arg1, arg2, arg3, arg4, arg5, arg6, arg7) => {
                    ++executionCount;
                    return "";
                }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceFunc8InputsClass: OverloadTests {

            [Fact]
            public void DebounceFunc8Inputs() {
                Func<int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, int, int, int, string>(
                    (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => {
                        ++executionCount;
                        return "";
                    }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceFunc9InputsClass: OverloadTests {

            [Fact]
            public void DebounceFunc9Inputs() {
                Func<int, int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, int, int, int, int, string>(
                    (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => {
                        ++executionCount;
                        return "";
                    }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceFunc10InputsClass: OverloadTests {

            [Fact]
            public void DebounceFunc10Inputs() {
                Func<int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, int, int, int, int, int, string>(
                    (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => {
                        ++executionCount;
                        return "";
                    }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceFunc11InputsClass: OverloadTests {

            [Fact]
            public void DebounceFunc11Inputs() {
                Func<int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, int, int, int, int, int, int, string>(
                    (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => {
                        ++executionCount;
                        return "";
                    }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceFunc12InputsClass: OverloadTests {

            [Fact]
            public void DebounceFunc12Inputs() {
                Func<int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, int, int, int, int, int, int, int, string>(
                    (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) => {
                        ++executionCount;
                        return "";
                    }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceFunc13InputsClass: OverloadTests {

            [Fact]
            public void DebounceFunc13Inputs() {
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer.Debounce<int, int, int, int, int, int, int, int, int, int, int, int, int, string>(
                    (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) => {
                        ++executionCount;
                        return "";
                    }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceFunc14InputsClass: OverloadTests {

            [Fact]
            public void DebounceFunc14Inputs() {
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer
                    .Debounce<int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(
                        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) => {
                            ++executionCount;
                            return "";
                        }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceFunc15InputsClass: OverloadTests {

            [Fact]
            public void DebounceFunc15Inputs() {
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer
                    .Debounce<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(
                        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) => {
                            ++executionCount;
                            return "";
                        }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

        public class DebounceFunc16InputsClass: OverloadTests {

            [Fact]
            public void DebounceFunc16Inputs() {
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string?> rateLimited = Debouncer
                    .Debounce<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(
                        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) => {
                            ++executionCount;
                            return "";
                        }, WAIT_TIME).Invoke;
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                rateLimited(8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8);
                executionCount.Should().Be(0);
            }

        }

    }

}