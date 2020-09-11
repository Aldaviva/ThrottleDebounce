ThrottleDebounce
===

*Rate limit your actions and funcs by throttling and debouncing them.*

This is a .NET library that lets you rate-limit functions so they are only executed at most once in a given interval, even if they are invoked multiple times in that interval.

<!-- MarkdownTOC autolink="true" bracket="round" autoanchor="true" levels="1,2" -->

- [Installation](#installation)
- [Usage](#usage)
- [Understanding throttling and debouncing](#understanding-throttling-and-debouncing)
- [Examples](#examples)

<!-- /MarkdownTOC -->

<a id="installation"></a>
## Installation
This package is [available on NuGet Gallery](https://www.nuget.org/packages/ThrottleDebounce/).
```powershell
Install-Package ThrottleDebounce
```
```powershell
dotnet add package ThrottleDebounce
```

It targets .NET Standard 2.0 and .NET Framework 4.0, so it should be compatible with many runtimes.

<a id="usage"></a>
## Usage

```cs
RateLimitedAction throttled = Throttler.Throttle(Action action, TimeSpan wait, bool leading, bool trailing);
RateLimitedFunc<T> debounced = Debouncer.Debounce(Func<T> func, TimeSpan wait, bool leading, bool trailing);
```

1. Call `Throttler.Throttle()` to throttle your action or func, or `Debouncer.Debounce()` to debounce it. Pass
    1. **`Action action`/`Func func`** — your action or func to rate-limit
    1. **`wait`** — how long to wait between executions
    1. **`leading`** — `true` if the first invocation should be executed immediately, or `false` if it should be queued. Optional, defaults to `true` for throttling and `false` for debouncing.
    1. **`trailing`** — `true` if subsequent invocations in the waiting period should be enqueued for later execution once the waiting interval is over, or `false` if they should be discarded. Optional, defaults to `true`.
1. Call the resulting object's `RateLimitedAction` or `RateLimitedFunc` to enqueue an invocation.
1. Your delegate will be executed at the desired rate.
1. Optionally call the resulting object's `Dispose()` method to prevent all queued executions from running when you are done.

<a id="understanding-throttling-and-debouncing"></a>
## Understanding throttling and debouncing

### tl;dr
Throttling and debouncing both restrict a function to not **execute** too often, no matter how frequently you **invoke** it.

This is useful if the function is invoked very frequently, like whenever the mouse moves, but you don't want to it to run every single time the pointer moves 1 pixel, because the function is expensive, such as rendering a user interface.

**Throttling** allows the function to still be executed periodically, even with a constant stream of invocations.

**Debouncing** prevents the function from being executed at all until it hasn't been invoked for a while.

### Diagram

[![Strategies for Rate-Limiting](https://aldaviva.com/portfolio/artwork/ratelimiting.png)](https://aldaviva.com/portfolio.html#ratelimiting)

### Lodash documentation

- [`_.throttle()`](https://lodash.com/docs/#throttle)
- [`_.debounce()`](https://lodash.com/docs/#debounce)

### Article and demo
[*Debouncing and Throttling Explained Through Examples* by David Corbacho](https://css-tricks.com/debouncing-throttling-explained-examples/)

<a id="examples"></a>
## Examples

### Throttle an action to execute at most every 1 second
```cs
Action throttled = Throttler.Throttle(() => Console.WriteLine("hi"), TimeSpan.FromSeconds(1)).RateLimitedAction;

throttled(); //runs at 0s
throttled(); //runs at 1s
throttled(); //runs at 2s
```

### Debounce a function to execute after no invocations for 200 milliseconds
```cs
Func<double, double, double> debounced = Debouncer.Debounce((double x, double y) => Math.Sqrt(x * x + y * y),
    TimeSpan.FromMilliseconds(200)).RateLimitedFunc;

double result;
result = debounced(1, 1); //never runs
result = debounced(2, 2); //never runs
result = debounced(3, 4); //runs at 200ms
```

### Canceling a rate-limited action so any queued executions won't run
```cs
RateLimitedAction rateLimited = Throttler.Throttle(() => Console.WriteLine("hello"), TimeSpan.FromSeconds(1));

rateLimited.RateLimitedAction(); //runs at 0s
rateLimited.Dispose();
rateLimited.RateLimitedAction(); //never runs
```

### Save a WPF window's position to the registry at most every 1 second
```cs
static void SaveWindowLocation(double x, double y) => Registry.SetValue(@"HKEY_CURRENT_USER\Software\My Program", 
    "Window Location", $"{x},{y}");

Action<double, double> saveWindowLocationThrottled = Throttler.Throttle<double, double>(saveWindowLocation, 
    TimeSpan.FromSeconds(1)).RateLimitedAction;

LocationChanged += (sender, args) => SaveWindowLocationThrottled(Left, Top);
```

### Prevent accidental double-clicks on a WPF button
```cs
public MainWindow(){
    InitializeComponent();

    Action<object, RoutedEventArgs> onButtonClickDebounced = Debouncer.Debounce<object, RoutedEventArgs>(
        OnButtonClick, TimeSpan.FromMilliseconds(40), true, false).RateLimitedAction;

    MyButton.Click += new RoutedEventHandler(onButtonClickDebounced);
}

private void OnButtonClick(object sender, RoutedEventArgs e) {
    MessageBox.Show("Button clicked");
}
```
