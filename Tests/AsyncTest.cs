using System.Globalization;

namespace Tests;

public class AsyncRetryOptionsTest {

    private readonly ITestOutputHelper _testOutputHelper;

    private RateLimitedFunc<Task> TestDebounce { get; }
    private TaskCompletionSource TestAsyncDone { get; } = new();

    public AsyncRetryOptionsTest(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
        TestDebounce      = Debouncer.Debounce(TestAsync, TimeSpan.FromMilliseconds(200));
    }

    private async Task TestAsync() {
        _testOutputHelper.WriteLine("debounced: " + DateTime.Now.ToString("O", CultureInfo.CurrentCulture));
        await Task.Delay(100);
        TestAsyncDone.SetResult();
    }

    [Fact]
    public async Task RunAsync() {
        _testOutputHelper.WriteLine("test method: " + DateTime.Now.ToString("O", CultureInfo.CurrentCulture));
        await (TestDebounce.Invoke() ?? Task.CompletedTask);

        await TestAsyncDone.Task;
        _testOutputHelper.WriteLine("done: " + DateTime.Now.ToString("O", CultureInfo.CurrentCulture));
    }

}