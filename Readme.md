<img src="https://raw.githubusercontent.com/Aldaviva/ThrottleDebounce/master/ThrottleDebounce/icon.jpg" height="23" alt="ThrottleDebounce icon" /> ThrottleDebounce
===

[![Package Version](https://img.shields.io/nuget/v/ThrottleDebounce?logo=nuget&label=version)](https://www.nuget.org/packages/ThrottleDebounce/) [![NuGet Gallery Download Count](https://img.shields.io/nuget/dt/ThrottleDebounce?logo=nuget&color=blue
)](https://www.nuget.org/packages/ThrottleDebounce/) [![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/Aldaviva/ThrottleDebounce/dotnetpackage.yml?branch=master&logo=github)](https://github.com/Aldaviva/ThrottleDebounce/actions/workflows/dotnetpackage.yml) [![Testspace](https://img.shields.io/testspace/tests/Aldaviva/Aldaviva:ThrottleDebounce/master?passed_label=passing&failed_label=failing&logo=data%3Aimage%2Fsvg%2Bxml%3Bbase64%2CPHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCA4NTkgODYxIj48cGF0aCBkPSJtNTk4IDUxMy05NCA5NCAyOCAyNyA5NC05NC0yOC0yN3pNMzA2IDIyNmwtOTQgOTQgMjggMjggOTQtOTQtMjgtMjh6bS00NiAyODctMjcgMjcgOTQgOTQgMjctMjctOTQtOTR6bTI5My0yODctMjcgMjggOTQgOTQgMjctMjgtOTQtOTR6TTQzMiA4NjFjNDEuMzMgMCA3Ni44My0xNC42NyAxMDYuNS00NFM1ODMgNzUyIDU4MyA3MTBjMC00MS4zMy0xNC44My03Ni44My00NC41LTEwNi41UzQ3My4zMyA1NTkgNDMyIDU1OWMtNDIgMC03Ny42NyAxNC44My0xMDcgNDQuNXMtNDQgNjUuMTctNDQgMTA2LjVjMCA0MiAxNC42NyA3Ny42NyA0NCAxMDdzNjUgNDQgMTA3IDQ0em0wLTU1OWM0MS4zMyAwIDc2LjgzLTE0LjgzIDEwNi41LTQ0LjVTNTgzIDE5Mi4zMyA1ODMgMTUxYzAtNDItMTQuODMtNzcuNjctNDQuNS0xMDdTNDczLjMzIDAgNDMyIDBjLTQyIDAtNzcuNjcgMTQuNjctMTA3IDQ0cy00NCA2NS00NCAxMDdjMCA0MS4zMyAxNC42NyA3Ni44MyA0NCAxMDYuNVMzOTAgMzAyIDQzMiAzMDJ6bTI3NiAyODJjNDIgMCA3Ny42Ny0xNC44MyAxMDctNDQuNXM0NC02NS4xNyA0NC0xMDYuNWMwLTQyLTE0LjY3LTc3LjY3LTQ0LTEwN3MtNjUtNDQtMTA3LTQ0Yy00MS4zMyAwLTc2LjY3IDE0LjY3LTEwNiA0NHMtNDQgNjUtNDQgMTA3YzAgNDEuMzMgMTQuNjcgNzYuODMgNDQgMTA2LjVTNjY2LjY3IDU4NCA3MDggNTg0em0tNTU3IDBjNDIgMCA3Ny42Ny0xNC44MyAxMDctNDQuNXM0NC02NS4xNyA0NC0xMDYuNWMwLTQyLTE0LjY3LTc3LjY3LTQ0LTEwN3MtNjUtNDQtMTA3LTQ0Yy00MS4zMyAwLTc2LjgzIDE0LjY3LTEwNi41IDQ0UzAgMzkxIDAgNDMzYzAgNDEuMzMgMTQuODMgNzYuODMgNDQuNSAxMDYuNVMxMDkuNjcgNTg0IDE1MSA1ODR6IiBmaWxsPSIjZmZmIi8%2BPC9zdmc%2B)](https://aldaviva.testspace.com/spaces/298071) [![Coveralls](https://img.shields.io/coveralls/github/Aldaviva/ThrottleDebounce?logo=coveralls)](https://coveralls.io/github/Aldaviva/ThrottleDebounce?branch=master)

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

It targets [.NET Standard 2.0](https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-0) and .NET Framework 4.5.2, so it should be compatible with many runtimes.

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

1. Call **`Throttler.Throttle`** to throttle your delegate, or **`Debouncer.Debounce`** to debounce it. Pass
    1. **`Action action`/`Func func`** — your delegate to rate-limit
    1. **`TimeSpan wait`** — how long to wait between executions
    1. **`bool leading`** — `true` if the first invocation should be executed immediately, or `false` if it should be queued. Optional, defaults to `true` for throttling and `false` for debouncing.
    1. **`bool trailing`** — `true` if subsequent invocations in the waiting period should be enqueued for later execution once the waiting interval is over, or `false` if they should be discarded. Optional, defaults to `true`.
1. Call the resulting `RateLimitedAction`/`RateLimitedFunc` object's **`Invoke`** method to enqueue an invocation.
    - `RateLimitedFunc.Invoke` will return `default` (e.g. `null`) if `leading` is `false` and the rate-limited `Func` has not been executed before. Otherwise, it will return the `Func`'s most recent return value.
1. Your delegate will be executed at the desired rate.
1. Optionally call the `RateLimitedAction`/`RateLimitedFunc` object's `Dispose()` method to prevent all queued executions from running when you are done.

<a id="understanding-throttling-and-debouncing"></a>
### Understanding throttling and debouncing

#### Summary
Throttling and debouncing both restrict a function to not **execute** too often, no matter how frequently you **invoke** it.

This is useful if the function is invoked very frequently, like whenever the mouse moves, but you don't want to it to run every single time the pointer moves 1 pixel, because the function is expensive, such as rendering a user interface.

**Throttling** allows the function to still be executed periodically, even with a constant stream of invocations.

**Debouncing** prevents the function from being executed at all until it hasn't been invoked for a while.

An invocation can result in at most one execution. For example, if both `leading` and `trailing` are `true`, one single invocation will execute once on the leading edge and not on the trailing edge.

Not all extra invocations are queued to run on the trailing edge &mdash; only the latest extra invocation is saved, and the other extras are dropped. For example, if you throttle mouse movement and then quickly move your pointer across your screen, only a few of the move event callbacks will be executed, many pixels apart; it won't slowly execute thousands of callbacks all spread out over a long time.

#### Diagram

[![Strategies for Rate-Limiting](https://i.imgur.com/ynlwKtm.png)](https://aldaviva.com/portfolio.html#ratelimiting)

#### Lodash documentation

- [`_.throttle()`](https://lodash.com/docs/#throttle)
- [`_.debounce()`](https://lodash.com/docs/#debounce)

#### Article and demo
[*Debouncing and Throttling Explained Through Examples* by David Corbacho](https://css-tricks.com/debouncing-throttling-explained-examples/)

<a id="examples"></a>
### Examples

#### Throttle an action to execute at most every 1 second
```cs
Action throttled = Throttler.Throttle(() => Console.WriteLine("hi"), TimeSpan.FromSeconds(1)).Invoke;

throttled(); //logs at 0s
throttled(); //logs at 1s
Thread.Sleep(1000);
throttled(); //logs at 2s
```

#### Debounce a function to execute after no invocations for 200 milliseconds
```cs
Func<double, double, double> debounced = Debouncer.Debounce((double x, double y) => Math.Sqrt(x * x + y * y),
    TimeSpan.FromMilliseconds(200)).Invoke;

double? result;
result = debounced(1, 1); //never runs
result = debounced(2, 2); //never runs
result = debounced(3, 4); //runs at 200ms
```

#### Canceling a rate-limited action so any queued executions won't run
```cs
RateLimitedAction rateLimited = Throttler.Throttle(() => Console.WriteLine("hello"), TimeSpan.FromSeconds(1));

rateLimited.Invoke(); //runs at 0s
rateLimited.Dispose();
rateLimited.Invoke(); //never runs
```

#### Save a WPF window's position to the registry at most every 1 second
```cs
static void SaveWindowLocation(double x, double y) => Registry.SetValue(@"HKEY_CURRENT_USER\Software\My Program", 
    "Window Location", $"{x},{y}");

Action<double, double> saveWindowLocationThrottled = Throttler.Throttle<double, double>(saveWindowLocation, 
    TimeSpan.FromSeconds(1)).Invoke;

LocationChanged += (sender, args) => SaveWindowLocationThrottled(Left, Top);
```

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

1. Call **`Retrier.Attempt`**. Pass
    1. **`Action<int> action`/`Func<int, T> func`** — your delegate to attempt, and possibly retry if it throws exceptions. The attempt number will be passed as the `int` parameter, starting with `0` before the first attempt, and `1` before the first retry. If this func returns a `Task`, it will be awaited to determine if it threw an exception.
    1. **`int? maxAttempts`** — the total number of times the delegate is allowed to run in this invocation, equal to `1` initial attempt plus up to `maxAttempts - 1` retries if it throws an exception. Must be at least 1, if you pass 0 it will clip to 1. Defaults to 2. For infinite retries, pass `null`.
    1. **`Func<int, TimeSpan>? delay`** — how long to wait between attempts, as a function of the number of retries that have already run, starting with `0` after the first attempt and before the first retry. You can return a constant `TimeSpan` for a fixed delay, or pass longer values for subsequent attempts to implement, for example, exponential backoff. Optional, defaults to `null`, which means no delay. The minimum value is `0`, the maximum value is `int.MaxValue` (`uint.MaxValue - 1` starting in .NET 6), and values outside this range will be clipped. You can also pass the built-in implementations such as `Retrier.Delays.Constant` and `Retrier.Delays.Exponential`.
    1. **`Func<Exception, bool>? isRetryAllowed`** — whether the delegate is permitted to execute again after a given `Exception` instance. Return `true` to allow or `false` to deny retries. For example, you may want to retry after HTTP 500 errors since subsequent requests may succeed, but stop after the first failure for an HTTP 403 error which probably won't succeed if the same request is sent again. Optional, `null` defaults to retrying on all exceptions besides `OutOfMemoryException`.
    1. **`Action<int, Exception>? beforeRetry`/`Func<int, Exception, Task>? beforeRetry`** — a delegate to run extra logic between attempts, for example, if you want to log a message or perform any cleanup before the next attempt. Optional, defaults to not running anything between attempts. The `int` parameter is the attempt number that most recently failed, starting with `0` the first time this delegate is called. The most recent `Exception` is also passed. If this returns a `Task`, it will be awaited before the next retry.
    1. **`CancellationToken cancellationToken`** — used to cancel the attempts and delays before they have all completed. Optional, defaults to no cancellation token. When cancelled, `Attempt` throws a `TaskCancelledException`.
1. If your delegate returns a value, it will be returned by `Attempt`.

<a id="example"></a>
### Example

#### Send at most 5 HTTP requests, 2 seconds apart, until a 200 response is received
```cs
using HttpClient httpClient = new();
HttpStatusCode statusCode = await Retrier.Attempt(async attempt => {
    Console.WriteLine($"Attempt #{attempt:N0}...");
    using HttpResponseMessage response = await httpClient.GetAsync("https://httpbin.org/status/200%2C500");

    Console.WriteLine($"Received response status code {(int) response.StatusCode}.");
    response.EnsureSuccessStatusCode(); // throws HttpRequestException for status codes outside the range [200, 300)
    return response.StatusCode;
}, maxAttempts: 5, delay: Retrier.Delays.Constant(TimeSpan.FromSeconds(2)));
Console.WriteLine($"Final response: {(int) statusCode}");
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
