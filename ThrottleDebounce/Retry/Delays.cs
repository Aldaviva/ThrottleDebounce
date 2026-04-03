#nullable enable

namespace ThrottleDebounce.Retry;

/// <summary>
/// Built-in implementations of different backoff strategies. These can be set on the <see cref="RetryOptions.Delay"/> property of <see cref="RetryOptions"/> and <see cref="AsyncRetryOptions"/>, which are passed to <see cref="Retrier.Attempt(System.Action{long},RetryOptions)"/> and its overloads.
/// </summary>
/// <remarks>To visualize and play with different strategies and values, check out <see href="https://dotnetfiddle.net/PrDP6x"/>.</remarks>
public static class Delays {

    private static TimeSpan Clip(Func<TimeSpan> calculateDelay, TimeSpan max) {
        try {
            TimeSpan delay = calculateDelay();
            return delay < TimeSpan.Zero ? TimeSpan.Zero : max != TimeSpan.Zero && delay > max ? max : delay;
        } catch (OverflowException) {
            return max != TimeSpan.Zero ? max : TimeSpan.MaxValue;
        }
    }

    /// <summary>
    /// Always wait a fixed duration before each retry.
    /// </summary>
    /// <remarks>To visualize and play with different strategies and values, check out <see href="https://dotnetfiddle.net/PrDP6x"/>.</remarks>
    /// <param name="delay">How long to wait before the next retry</param>
    /// <returns>A function that can be assigned to <see cref="RetryOptions.Delay"/></returns>
    public static Func<long, TimeSpan> Constant(TimeSpan delay) => _ => delay;

    /// <inheritdoc cref="Constant(System.TimeSpan)" />
    public static Func<long, TimeSpan> Constant(int delayMilliseconds) => Constant(TimeSpan.FromMilliseconds(delayMilliseconds));

    /// <summary>
    /// Wait a linearly-increasing duration before each retry (<c>delay = coefficient * retry + initial</c>). The initial value of <c>retry</c> is 0.
    /// </summary>
    /// <remarks>To visualize and play with different strategies and values, check out <see href="https://dotnetfiddle.net/PrDP6x"/>.</remarks>
    /// <param name="coefficient">A duration to multiply by the next retry's number (starting from 0) to get a duration for this retry delay</param>
    /// <param name="offset">A constant duration to add</param>
    /// <param name="max">An upper limit above which the duration will not increase (the line becomes horizontal at this y-position)</param>
    /// <returns>A function that can be assigned to <see cref="RetryOptions.Delay"/></returns>
    public static Func<long, TimeSpan> Linear(TimeSpan coefficient, TimeSpan offset = default, TimeSpan max = default) => retry => Clip(() => {
        checked {
            long ticks = coefficient.Ticks * retry;
            return new TimeSpan(ticks) + offset;
        }
    }, max);

    /// <summary>
    /// Wait an exponentially-increasing duration before each retry (<c>delay = coefficient * retry^power + initial</c>). The initial value of <c>retry</c> is 0.
    /// </summary>
    /// <remarks>To visualize and play with different strategies and values, check out <see href="https://dotnetfiddle.net/PrDP6x"/>.</remarks>
    /// <param name="coefficient">A duration to multiply by the exponential value to get a duration for this retry delay </param>
    /// <param name="power">The power to which the retry number (starting from 0) will be raised. Defaults to <c>2</c> to square it.</param>
    /// <param name="offset">A constant duration to add</param>
    /// <param name="max">An upper limit above which the duration will not increase (the line becomes horizontal at this y-position)</param>
    /// <returns>A function that can be assigned to <see cref="RetryOptions.Delay"/></returns>
    public static Func<long, TimeSpan> Exponential(TimeSpan coefficient, double power = 2, TimeSpan offset = default, TimeSpan max = default) => retry => Clip(() => {
        checked {
            long ticks = (long) (coefficient.Ticks * Math.Pow(retry, power));
            return new TimeSpan(ticks) + offset;
        }
    }, max);

    /// <summary>
    /// Wait a duration at increases with a power series before each retry (<c>delay = coefficient * base^retry + initial</c>). The initial value of <c>retry</c> is 0.
    /// </summary>
    /// <remarks>To visualize and play with different strategies and values, check out <see href="https://dotnetfiddle.net/PrDP6x"/>.</remarks>
    /// <param name="coefficient">A duration to multiply by the exponential value to get a duration for this retry delay</param>
    /// <param name="base">A number to raise to the power of the number of retries that have already been attempted (starting from 0). Defaults to <c>2</c> for a <c>2^x</c> power series.</param>
    /// <param name="offset">A constant duration to add</param>
    /// <param name="max">An upper limit above which the duration will not increase (the line becomes horizontal at this y-position)</param>
    /// <returns>A function that can be assigned to <see cref="RetryOptions.Delay"/></returns>
    public static Func<long, TimeSpan> Power(TimeSpan coefficient, double @base = 2, TimeSpan offset = default, TimeSpan max = default) => retry => Clip(() => {
        checked {
            long ticks = retry == 0 ? 0 : (long) (coefficient.Ticks * Math.Pow(@base, retry));
            return new TimeSpan(ticks) + offset;
        }
    }, max);

    /// <summary>
    /// Wait a logarithmically-increasing duration before each retry (<c>delay = coefficient * (log(retry, base) + 1) + offset</c>). The initial value of <c>retry</c> is 0, which results in a delay of <paramref name="offset"/> because the logarithm would be negative infinity otherwise.
    /// </summary>
    /// <remarks>To visualize and play with different strategies and values, check out <see href="https://dotnetfiddle.net/PrDP6x"/>.</remarks>
    /// <param name="coefficient">A duration to multiply by the logarithm to get a duration for this retry delay</param>
    /// <param name="base">The logarithmic base. Defaults to 10 for a base-10 logarithm when omitted or less than 1.</param>
    /// <param name="offset">A constant duration to add</param>
    /// <param name="max">An upper limit above which the duration will not increase (the line becomes horizontal at this y-position)</param>
    /// <returns>A function that can be assigned to <see cref="RetryOptions.Delay"/></returns>
    public static Func<long, TimeSpan> Logarithmic(TimeSpan coefficient, double @base = 10, TimeSpan offset = default, TimeSpan max = default) => retry => Clip(() => {
        checked {
            @base = @base > 1 ? @base : 10;
            long ticks = retry == 0 ? 0 : (long) (coefficient.Ticks * (Math.Log(retry, @base) + 1));
            return new TimeSpan(ticks) + offset;
        }
    }, max);

    /// <summary>
    /// Wait a random duration before each retry.
    /// </summary>
    /// <remarks>To visualize and play with different strategies and values, check out <see href="https://dotnetfiddle.net/PrDP6x"/>.</remarks>
    /// <param name="max">The upper limit of the duration to wait</param>
    /// <param name="min">The lower limit of the duration to wait. Defaults to 0 seconds.</param>
    /// <returns>A function that can be passed to the <c>delay</c> parameter of <c>Retrier.Attempt</c></returns>
    public static Func<long, TimeSpan> MonteCarlo(TimeSpan max, TimeSpan min = default) {
        Random random = new();
        long   range  = (max - min).Ticks;
        return _ => (range <= int.MaxValue ? new TimeSpan(random.Next((int) range)) : new TimeSpan((long) (random.NextDouble() * range))) + min;
    }

}