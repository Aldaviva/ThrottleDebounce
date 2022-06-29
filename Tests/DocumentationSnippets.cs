using System;
using ThrottleDebounce;

// ReSharper disable RedundantAssignment
// ReSharper disable NotAccessedVariable

namespace Tests; 

public class DocumentationSnippets {

    public void throttle1Second() {
        Action throttled = Throttler.Throttle(() => Console.WriteLine("hello"), TimeSpan.FromSeconds(1)).Invoke;

        throttled(); //runs at 0s
        throttled(); //runs at 1s
    }

    public void debounce200Ms() {
        Func<double, double, double> debounced = Debouncer.Debounce((double x, double y) => Math.Sqrt(x * x + y * y),
            TimeSpan.FromMilliseconds(200)).Invoke;

        double result; //runs at 200ms
        result = debounced(1, 1);
        result = debounced(2, 2);
        result = debounced(3, 4);
    }

    public void dispose() {
        RateLimitedAction rateLimited = Throttler.Throttle(() => Console.WriteLine("hello"), TimeSpan.FromSeconds(1));

        rateLimited.Invoke(); //runs at 0s
        rateLimited.Dispose();
        rateLimited.Invoke(); //never runs
    }

}