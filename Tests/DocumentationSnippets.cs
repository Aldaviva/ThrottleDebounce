using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

    public async Task retry() {
        using HttpClient httpClient = new();
        HttpStatusCode statusCode = await Retrier.Attempt(async attempt => {
            Console.WriteLine($"Attempt #{attempt:N0}...");
            using HttpResponseMessage response = await httpClient.GetAsync("https://httpbin.org/status/200%2C500");

            Console.WriteLine($"Received response status code {(int) response.StatusCode}.");
            response.EnsureSuccessStatusCode(); // throws HttpRequestException for status codes outside the range [200, 300)
            return response.StatusCode;
        }, maxAttempts: 5, delay: Retrier.Delays.Constant(TimeSpan.FromSeconds(2)));
        Console.WriteLine($"Final response: {(int) statusCode}");
    }

}