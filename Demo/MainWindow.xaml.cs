using System;
using System.Windows;
using System.Windows.Controls;
using ThrottleDebounce;
using ThrottleDebounce.RateLimitedDelegates;

#nullable enable

namespace Demo {

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

        public MainWindow() {
            InitializeComponent();

            TimeSpan waitTime = TimeSpan.FromSeconds(1);

            throttler = Throttler.Throttle<DateTime>(invokedTime => Dispatcher.Invoke(() => {
                DateTime now = DateTime.Now;
                UpdateLabelContent(throttled, invokedTime, now, lastThrottledExecutionTime);
                lastThrottledExecutionTime = now;
            }), waitTime, leading: true, trailing: true);

            debouncerTrailing = Debouncer.Debounce<DateTime>(invokedTime => Dispatcher.Invoke(() => {
                DateTime now = DateTime.Now;
                UpdateLabelContent(debouncedTrailing, invokedTime, now, lastDebouncedTrailingExecutionTime);
                lastDebouncedTrailingExecutionTime = now;
            }), waitTime, false, true);

            debouncerLeading = Debouncer.Debounce<DateTime>(invokedTime => Dispatcher.Invoke(() => {
                DateTime now = DateTime.Now;
                UpdateLabelContent(debouncedLeading, invokedTime, now, lastDebouncedLeadingExecutionTime);
                lastDebouncedLeadingExecutionTime = now;
            }), waitTime, true, false);

            debouncerBoth = Debouncer.Debounce<DateTime>(invokedTime => Dispatcher.Invoke(() => {
                DateTime now = DateTime.Now;
                UpdateLabelContent(debouncedBoth, invokedTime, now, lastDebouncedBothExecutionTime);
                lastDebouncedBothExecutionTime = now;
            }), waitTime, true, true);

            fireEventButton.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            DateTime now = DateTime.Now;

            UpdateLabelContent(original, now, now, lastOriginalExecutionTime);
            lastOriginalExecutionTime = now;

            throttler.RateLimitedAction(now);
            debouncerTrailing.RateLimitedAction(now);
            debouncerLeading.RateLimitedAction(now);
            debouncerBoth.RateLimitedAction(now);
        }

        // private static string GetNow() {
        //     return DateTime.Now.ToString("h:mm:ss.fff tt");
        // }

        private static void UpdateLabelContent(Label label, DateTime invokedTime, DateTime executedTime, DateTime previousExecutedTime) {
            label.Content = $"Invoked at {invokedTime.ToString(timeFormat)}, executed at {executedTime.ToString(timeFormat)}" +
                (previousExecutedTime != default ? $", since last execution {executedTime - previousExecutedTime:g}" : "");
        }

        public void Dispose() {
            throttler.Dispose();
            debouncerTrailing.Dispose();
            debouncerLeading.Dispose();
            debouncerBoth.Dispose();
        }

    }

}