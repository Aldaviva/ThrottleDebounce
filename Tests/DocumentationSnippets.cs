﻿using System;
using ThrottleDebounce;
using ThrottleDebounce.RateLimitedDelegates;

namespace Tests {

    public class DocumentationSnippets {

        public void throttle1Second() {
            Action throttled = Throttler.Throttle(() => Console.WriteLine("hello"), TimeSpan.FromSeconds(1)).RateLimitedAction;

            throttled(); //runs at 0s
            throttled(); //runs at 1s
        }

        public void debounce200Ms() {
            Func<double, double, double> debounced = Debouncer.Debounce((double x, double y) => Math.Sqrt(x * x + y * y),
                TimeSpan.FromMilliseconds(200)).RateLimitedFunc;

            double result = debounced(3, 4); //runs at 200ms
        }

        public void dispose() {
            RateLimitedAction rateLimited = Throttler.Throttle(() => Console.WriteLine("hello"), TimeSpan.FromSeconds(1));

            rateLimited.RateLimitedAction(); //runs at 0s
            rateLimited.Dispose();
            rateLimited.RateLimitedAction(); //never runs
        }

        public void mousePosition() { }

    }

}