namespace Tests;

public abstract class BaseTest {

    protected static readonly TimeSpan WaitTime = TimeSpan.FromMilliseconds(500);

    protected int ExecutionCount     = 0;
    protected int MostRecentArgument = -1;

}