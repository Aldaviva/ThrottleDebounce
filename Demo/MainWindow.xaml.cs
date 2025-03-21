#nullable enable

using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using ThrottleDebounce;

namespace Demo; 

public partial class MainWindow: IDisposable {

    private const string timeFormat = "h:mm:ss.fff tt";

    private readonly RateLimitedAction<DateTime> throttler;
    private readonly RateLimitedAction<DateTime> debouncerTrailing;
    private readonly RateLimitedAction<DateTime> debouncerLeading;
    private readonly RateLimitedAction<DateTime> debouncerBoth;

    private DateTime lastOriginalExecutionTime;
    private DateTime lastThrottledExecutionTime;
    private DateTime lastDebouncedTrailingExecutionTime;
    private DateTime lastDebouncedLeadingExecutionTime;
    private DateTime lastDebouncedBothExecutionTime;

    private int originalExecutionCount;
    private int throttledExecutionCount;
    private int debouncedTrailingExecutionCount;
    private int debouncedLeadingExecutionCount;
    private int debouncedBothExectionCount;

    public MainWindow() {
        InitializeComponent();

        TimeSpan waitTime = TimeSpan.FromSeconds(1);

        Action<DateTime> converter = invokedTime => Dispatcher.InvokeAsync(() => {
            DateTime now = DateTime.Now;
            UpdateLabelContent(throttled, invokedTime, now, lastThrottledExecutionTime, ++throttledExecutionCount);
            lastThrottledExecutionTime = now;
        });
        throttler = Throttler.Throttle(converter, waitTime, leading: true, trailing: true);

        debouncerTrailing = Debouncer.Debounce<DateTime>(invokedTime => Dispatcher.Invoke(() => {
            DateTime now = DateTime.Now;
            UpdateLabelContent(debouncedTrailing, invokedTime, now, lastDebouncedTrailingExecutionTime, ++debouncedTrailingExecutionCount);
            lastDebouncedTrailingExecutionTime = now;
        }), waitTime);

        debouncerLeading = Debouncer.Debounce<DateTime>(invokedTime => Dispatcher.Invoke(() => {
            DateTime now = DateTime.Now;
            UpdateLabelContent(debouncedLeading, invokedTime, now, lastDebouncedLeadingExecutionTime, ++debouncedLeadingExecutionCount);
            lastDebouncedLeadingExecutionTime = now;
        }), waitTime, true, false);

        debouncerBoth = Debouncer.Debounce<DateTime>(invokedTime => Dispatcher.Invoke(() => {
            DateTime now = DateTime.Now;
            UpdateLabelContent(debouncedBoth, invokedTime, now, lastDebouncedBothExecutionTime, ++debouncedBothExectionCount);
            lastDebouncedBothExecutionTime = now;
        }), waitTime, true);

        fireEventButton.Focus();
    }

    private void Button_Click(object sender, RoutedEventArgs e) {
        DateTime now = DateTime.Now;

        UpdateLabelContent(original, now, now, lastOriginalExecutionTime, ++originalExecutionCount);
        lastOriginalExecutionTime = now;

        throttler.Invoke(now);
        debouncerTrailing.Invoke(now);
        debouncerLeading.Invoke(now);
        debouncerBoth.Invoke(now);
    }

    // private static string GetNow() {
    //     return DateTime.Now.ToString("h:mm:ss.fff tt");
    // }

    private static void UpdateLabelContent(Label label, DateTime invokedTime, DateTime executedTime, DateTime previousExecutedTime, int executionCount) {
        label.Content = $"Invoked at {invokedTime.ToString(timeFormat)}, executed at {executedTime.ToString(timeFormat)}" +
            (previousExecutedTime != default ? $", since last execution {executedTime - previousExecutedTime:g}" : "") + $", {executionCount:N0} executions";
    }

    protected override void OnClosed(EventArgs e) {
        base.OnClosed(e);
        Dispose();
    }

    public void Dispose() {
        throttler.Dispose();
        debouncerTrailing.Dispose();
        debouncerLeading.Dispose();
        debouncerBoth.Dispose();
    }

    public void documentationExamples() {
        static void saveWindowLocation(double x, double y) => Registry.SetValue(@"HKEY_CURRENT_USER\Software\My Program", "Window Location", $"{x},{y}");
        RateLimitedAction<double, double> saveWindowLocationThrottled = Throttler.Throttle<double, double>(saveWindowLocation, TimeSpan.FromSeconds(1));
        LocationChanged += (sender, args) => saveWindowLocationThrottled.Invoke(Left, Top);

        RateLimitedAction<object, RoutedEventArgs> onButtonClickDebounced = Debouncer.Debounce<object, RoutedEventArgs>(onButtonClick, TimeSpan.FromMilliseconds(40), true, false);
        fireEventButton.Click += onButtonClickDebounced.Invoke;
    }

    private void onButtonClick(object sender, RoutedEventArgs e) {
        MessageBox.Show("Button clicked");
    }

}