<img src="https://raw.githubusercontent.com/Aldaviva/ThrottleDebounce/master/ThrottleDebounce/icon.jpg" height="23" alt="ThrottleDebounce icon" /> ThrottleDebounce
===

[![Nuget](https://img.shields.io/nuget/v/ThrottleDebounce?logo=nuget)](https://www.nuget.org/packages/ThrottleDebounce/) [![GitHub Workflow Status](https://img.shields.io/github/workflow/status/Aldaviva/ThrottleDebounce/.NET?logo=github)](https://github.com/Aldaviva/ThrottleDebounce/actions/workflows/dotnetpackage.yml) [![Coveralls](https://img.shields.io/coveralls/github/Aldaviva/ThrottleDebounce?logo=coveralls)](https://coveralls.io/github/Aldaviva/ThrottleDebounce?branch=master)

*Rate-limit your actions and funcs by throttling and debouncing them. Retry when an exception is thrown.*

This is a .NET library that lets you rate-limit delegates so they are only executed at most once in a given interval, even if they are invoked multiple times in that interval. You can also invoke a delegate and automatically retry it if it fails.

<!-- MarkdownTOC autolink="true" bracket="round" autoanchor="true" levels="1,2,3" -->

- [Installation](#installation)
- [Rate limiting](#rate-limiting)
    - [Usage](#usage)
    - [Understanding throttling and debouncing](#understanding-throttling-and-debouncing)
    - [Examples](#examples)
- [Retrying](#retrying)
    - [Usage](#usage-1)
    - [Example](#example)

<!-- /MarkdownTOC -->

<a id="installation"></a>
## Installation
This package is [available on NuGet Gallery](https://www.nuget.org/packages/ThrottleDebounce/).
```powershell
dotnet add package ThrottleDebounce
```
```powershell
Install-Package ThrottleDebounce
```

It targets .NET Standard 2.0 and .NET Framework 4.5.2, so it should be compatible with many runtimes.

<a id="rate-limiting"></a>
## Rate limiting

<a id="usage"></a>
### Usage

```cs
Action originalAction;
Func<int> originalFunc;

TimeSpan wait = TimeSpan.FromMilliseconds(50);
using RateLimitedAction throttledAction = Throttler.Throttle(originalAction, wait, leading: true, trailing: true);
using RateLimitedFunc<int> debouncedFunc = Debouncer.Debounce(originalFunc, wait, leading: false, trailing: true);

throttledAction.Invoke();
int? result = debouncedFunc.Invoke();
```

1. Call **`Throttler.Throttle()`** to throttle your delegate, or **`Debouncer.Debounce()`** to debounce it. Pass
    1. **`Action action`/`Func func`** — your delegate to rate-limit
    1. **`TimeSpan wait`** — how long to wait between executions
    1. **`bool leading`** — `true` if the first invocation should be executed immediately, or `false` if it should be queued. Optional, defaults to `true` for throttling and `false` for debouncing.
    1. **`bool trailing`** — `true` if subsequent invocations in the waiting period should be enqueued for later execution once the waiting interval is over, or `false` if they should be discarded. Optional, defaults to `true`.
1. Call the resulting `RateLimitedAction`/`RateLimitedFunc` object's **`Invoke()`** method to enqueue an invocation.
    - `RateLimitedFunc.Invoke` will return `default` (e.g. `null`) if `leading` is `false` and the rate-limited `Func` has not been executed before. Otherwise, it will return the `Func`'s most recent return value.
1. Your delegate will be executed at the desired rate.
1. Optionally call the `RateLimitedAction`/`RateLimitedFunc` object's `Dispose()` method to prevent all queued executions from running when you are done.

<a id="understanding-throttling-and-debouncing"></a>
### Understanding throttling and debouncing

<a id="summary"></a>
#### Summary
Throttling and debouncing both restrict a function to not **execute** too often, no matter how frequently you **invoke** it.

This is useful if the function is invoked very frequently, like whenever the mouse moves, but you don't want to it to run every single time the pointer moves 1 pixel, because the function is expensive, such as rendering a user interface.

**Throttling** allows the function to still be executed periodically, even with a constant stream of invocations.

**Debouncing** prevents the function from being executed at all until it hasn't been invoked for a while.

An invocation can result in at most one execution. For example, if both `leading` and `trailing` are `true`, one single invocation will execute once on the leading edge and not on the trailing edge.

<a id="diagram"></a>
#### Diagram

[![Strategies for Rate-Limiting](https://i.imgur.com/ynlwKtm.png)](https://aldaviva.com/portfolio.html#ratelimiting)

<a id="lodash-documentation"></a>
#### Lodash documentation

- [`_.throttle()`](https://lodash.com/docs/#throttle)
- [`_.debounce()`](https://lodash.com/docs/#debounce)

<a id="article-and-demo"></a>
#### Article and demo
[*Debouncing and Throttling Explained Through Examples* by David Corbacho](https://css-tricks.com/debouncing-throttling-explained-examples/)

<a id="examples"></a>
### Examples

<a id="throttle-an-action-to-execute-at-most-every-1-second"></a>
#### Throttle an action to execute at most every 1 second
```cs
Action throttled = Throttler.Throttle(() => Console.WriteLine("hi"), TimeSpan.FromSeconds(1)).Invoke;

throttled(); //logs at 0s
throttled(); //logs at 1s
Thread.Sleep(1000);
throttled(); //logs at 2s
```

<a id="debounce-a-function-to-execute-after-no-invocations-for-200-milliseconds"></a>
#### Debounce a function to execute after no invocations for 200 milliseconds
```cs
Func<double, double, double> debounced = Debouncer.Debounce((double x, double y) => Math.Sqrt(x * x + y * y),
    TimeSpan.FromMilliseconds(200)).Invoke;

double? result;
result = debounced(1, 1); //never runs
result = debounced(2, 2); //never runs
result = debounced(3, 4); //runs at 200ms
```

<a id="canceling-a-rate-limited-action-so-any-queued-executions-wont-run"></a>
#### Canceling a rate-limited action so any queued executions won't run
```cs
RateLimitedAction rateLimited = Throttler.Throttle(() => Console.WriteLine("hello"), TimeSpan.FromSeconds(1));

rateLimited.Invoke(); //runs at 0s
rateLimited.Dispose();
rateLimited.Invoke(); //never runs
```

<a id="save-a-wpf-windows-position-to-the-registry-at-most-every-1-second"></a>
#### Save a WPF window's position to the registry at most every 1 second
```cs
static void SaveWindowLocation(double x, double y) => Registry.SetValue(@"HKEY_CURRENT_USER\Software\My Program", 
    "Window Location", $"{x},{y}");

Action<double, double> saveWindowLocationThrottled = Throttler.Throttle<double, double>(saveWindowLocation, 
    TimeSpan.FromSeconds(1)).Invoke;

LocationChanged += (sender, args) => SaveWindowLocationThrottled(Left, Top);
```

<a id="prevent-accidental-double-clicks-on-a-wpf-button"></a>
#### Prevent accidental double-clicks on a WPF button
```cs
public MainWindow(){
    InitializeComponent();

    Action<object, RoutedEventArgs> onButtonClickDebounced = Debouncer.Debounce<object, RoutedEventArgs>(
        OnButtonClick, TimeSpan.FromMilliseconds(40), true, false).Invoke;

    MyButton.Click += new RoutedEventHandler(onButtonClickDebounced);
}

private void OnButtonClick(object sender, RoutedEventArgs e) {
    MessageBox.Show("Button clicked");
}
```

<a id="retrying"></a>
## Retrying

Given a function or action, you can execute it and, if it threw an exception, automatically execute it again until it succeeds.

<a id="usage-1"></a>
### Usage
```cs
Retrier.Attempt(attempt => MyErrorProneAction(), maxAttempts: 2);
```

1. Call **`Retrier.Attempt()`**. Pass
    1. **`Action<int> action`/`Func<int, T> func`** — your delegate to attempt, and possibly retry if it throws exceptions. The attempt number will be passed as the `int` parameter, starting with `0` before the first attempt. If this func returns a `Task`, it will be awaited to determine if it threw an exception.
    1. **`int maxAttempts`** — the maximum number of times the delegate may be executed, including the initial attempt. Optional, defaults to `2`. Must be at least `1`.
    1. **`Func<TimeSpan> delay`** — how long to wait between attempts, as a function of the attempt number. The upcoming attempt number will be passed as a parameter, starting with `1` before the second attempt. You can return a constant `TimeSpan` for a fixed delay, or pass longer values for subsequent attempts to implement, for example, exponential backoff. Optional, defaults to no delay.
    1. **`Func<Exception, bool> isRetryAllowed`** — whether the delegate is permitted to execute again after a given `Exception` instance. Return `true` to allow or `false` to deny retries. For example, you may want to retry after HTTP 500 errors since subsequent requests may succeed, but stop after the first failure for an HTTP 403 error which probably won't succeed if the same request is sent again. Optional, defaults to retrying on all exceptions besides `OutOfMemoryException`.
    1. **`Action beforeRetry`/`Func<Task> beforeRetry`** — a delegate to run extra logic between attempts, for example, if you want to log a message or perform any cleanup before the next attempt. Optional, defaults to not running anything between attempts.
    1. **`CancellationToken cancellationToken`** — used to cancel the attempts and delays before they have all completed. Optional, defaults to no cancellation token. When cancelled, `Attempt()` throws a `TaskCancelledException`.
1. If your delegate returns a value, it will be returned by `Attempt()`.

<a id="example"></a>
### Example

#### Send at most 5 HTTP requests, 2 seconds apart, until a 200 response is received
```cs
using HttpClient httpClient = new();
HttpStatusCode statusCode = await Retrier.Attempt(async attempt => {
    Console.WriteLine($"Attempt #{attempt:N0}...");
    HttpResponseMessage response = await httpClient.GetAsync("https://httpbin.org/status/200%2C500");
    Console.WriteLine($"Received response status code {(int) response.StatusCode}.");
    response.EnsureSuccessStatusCode(); // throws HttpRequestException for status codes outside the range [200, 300)
    return response.StatusCode;
}, 5, _ => TimeSpan.FromSeconds(2));
Console.WriteLine($"Final response: {(int) statusCode}")
```
```text
Attempt #0...
Received response status code 500
Attempt #1...
Received response status code 500
Attempt #2...
Received response status code 200
Final response: 200
```