using System;
using System.Globalization;
using System.Threading.Tasks;
using ThrottleDebounce;
using Xunit;
using Xunit.Abstractions;

namespace Tests;

public class AsyncTest {

    private readonly ITestOutputHelper testOutputHelper;

    private RateLimitedFunc<Task> TestDebounce { get; }
    private TaskCompletionSource TestAsyncDone { get; } = new();

    public AsyncTest(ITestOutputHelper testOutputHelper) {
        this.testOutputHelper = testOutputHelper;
        TestDebounce          = Debouncer.Debounce(TestAsync, TimeSpan.FromMilliseconds(200));
    }

    private async Task TestAsync() {
        testOutputHelper.WriteLine("debounced: " + DateTime.Now.ToString("O", CultureInfo.CurrentCulture));
        await Task.Delay(100);
        TestAsyncDone.SetResult();
    }

    [Fact]
    public async Task RunAsync() {
        testOutputHelper.WriteLine("test method: " + DateTime.Now.ToString("O", CultureInfo.CurrentCulture));
        await (TestDebounce.Invoke() ?? Task.CompletedTask);

        await TestAsyncDone.Task;
        testOutputHelper.WriteLine("done: " + DateTime.Now.ToString("O", CultureInfo.CurrentCulture));
    }

}