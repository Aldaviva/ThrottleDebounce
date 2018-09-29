using System;
using System.Windows;
using System.Windows.Controls;
using ThrottleDebounce;

namespace Demo {

    public partial class MainWindow : IDisposable {

        private readonly DebouncedAction<string> throttler;
        private readonly DebouncedAction<string> debouncerTrailing;
        private readonly DebouncedAction<string> debouncerLeading;
        private readonly DebouncedAction<string> debouncerBoth;

        public MainWindow() {
            InitializeComponent();
            throttler = Throttler.Throttle<string>(now => Dispatcher.Invoke(() => UpdateLabelContent(throttled, now)), TimeSpan.FromSeconds(1));
            debouncerTrailing = Debouncer.Debounce<string>(now => Dispatcher.Invoke(() => UpdateLabelContent(debouncedTrailing, now)), TimeSpan.FromSeconds(1), false, true);
            debouncerLeading = Debouncer.Debounce<string>(now => Dispatcher.Invoke(() => UpdateLabelContent(debouncedLeading, now)), TimeSpan.FromSeconds(1), true, false);
            debouncerBoth = Debouncer.Debounce<string>(now => Dispatcher.Invoke(() => UpdateLabelContent(debouncedBoth, now)), TimeSpan.FromSeconds(1), true, true);
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            string now = GetNow();

            UpdateLabelContent(original, now);
            throttler.Run(now);
            debouncerTrailing.Run(now);
            debouncerLeading.Run(now);
            debouncerBoth.Run(now);
        }

        private static string GetNow() {
            return DateTime.Now.ToString("h:mm:ss.fff tt");
        }

        private static void UpdateLabelContent(Label label, string content) {
            label.Content = content;
        }

        public void Dispose() {
            throttler?.Dispose();
            debouncerTrailing?.Dispose();
            debouncerLeading?.Dispose();
            debouncerBoth?.Dispose();
        }
    }
}
